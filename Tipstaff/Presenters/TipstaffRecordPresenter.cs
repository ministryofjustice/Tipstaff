using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class TipstaffRecordPresenter : ITipstaffRecordPresenter, 
        IMapper<Models.TipstaffRecord, Tipstaff.Services.DynamoTables.TipstaffRecord>
    {
        private readonly ITipstaffRecordRepository _tipstaffRecordRepository;
        private readonly IRespondentPresenter _respondentPresenter;
        private readonly ICaseReviewPresenter _caseReviewPresenter;
        private readonly IAddressPresenter _addressPresenter;
        
        public TipstaffRecordPresenter(ITipstaffRecordRepository tipstaffRecordRepository, 
                                       IRespondentPresenter respondentPresenter, 
                                       ICaseReviewPresenter caseReviewPresenter, 
                                       IAddressPresenter addressPresenter)
        {
            _tipstaffRecordRepository = tipstaffRecordRepository;
            _respondentPresenter = respondentPresenter;
            _caseReviewPresenter = caseReviewPresenter;
            _addressPresenter = addressPresenter;
        }

        public void AddTipstaffRecord(Models.TipstaffRecord record)
        {
            var entity = GetDynamoTable(record);

            _tipstaffRecordRepository.Add(entity);
        }

        public IEnumerable<Models.TipstaffRecord> GetAll()
        {
            var entities = _tipstaffRecordRepository.GetAllByCondition<int>("CaseStatusId", 2);

            var records = entities.Select(x => GetModel(x));

            return records;
        }

        public Services.DynamoTables.TipstaffRecord GetDynamoTable(Models.TipstaffRecord model)
        {
            var entity = new Services.DynamoTables.TipstaffRecord()
            {
                //SentSCD26 = model.
            };

            return entity;
        }

        public Models.TipstaffRecord GetModel(Services.DynamoTables.TipstaffRecord table)
        {
            var model = new Models.TipstaffRecord()
            {
                arrestCount = table.ArrestCount,
                createdBy = table.CreatedBy,
                createdOn = table.CreatedOn,
                nextReviewDate = table.NextReviewDate,
                NPO = table.NPO,
                DateExecuted = table.DateExecuted,
                prisonCount = table.PrisonCount,
                resultDate = table.ResultDate,
                tipstaffRecordID = table.Id,
                resultEnteredBy = table.ResultEnteredBy,
               
                 addresses = _addressPresenter.GetAddressesByTipstaffRecordId(table.Id),
                ////AttendanceNotes = _attendanceNotePresenter.GetAllById(table.Id),
                //caseReviews = _caseReviewPresenter.GetAllById(table.Id),
                Respondents = _respondentPresenter.GetAllById(table.Id),
                Discriminator = table.Discriminator,
                result = MemoryCollections.ResultsList.GetResultList().FirstOrDefault(x=>x.ResultId == table.ResultId),
                caseStatus = MemoryCollections.CaseStatusList.GetCaseStatusList().FirstOrDefault(x=>x.CaseStatusId == table.CaseStatusId),
                protectiveMarking = MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().FirstOrDefault(x=>x.ProtectiveMarkingId == table.ProtectiveMarkingId)
            };

            return model;
        }

        public Models.TipstaffRecord GetTipStaffRecord(string id)
        {
            var record = _tipstaffRecordRepository.GetEntityByHashKey(id);

            var model = GetModel(record);

            return model;
        }

        public void UpdateTipstaffRecord(Models.TipstaffRecord record)
        {
            var entity = new Services.DynamoTables.TipstaffRecord()
            {
                ArrestCount = record.arrestCount,
                Discriminator = record.Discriminator,
                DateExecuted = record.DateExecuted,
                CreatedBy = record.createdBy,
                NPO = record.NPO,
                Id = record.tipstaffRecordID,
                NextReviewDate = record.nextReviewDate,
                PrisonCount = record.prisonCount,
                ResultDate = record.resultDate,
                ProtectiveMarkingId = record.protectiveMarking?.ProtectiveMarkingId,
                CaseStatusId = record.caseStatus.CaseStatusId,
                CreatedOn = record.createdOn
            };
            
            _tipstaffRecordRepository.Update(entity);
        }
    }
}