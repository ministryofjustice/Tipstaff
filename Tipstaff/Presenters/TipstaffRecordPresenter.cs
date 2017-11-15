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
        
        public TipstaffRecordPresenter(ITipstaffRecordRepository tipstaffRecordRepository)
        {
            _tipstaffRecordRepository = tipstaffRecordRepository;
        }

        public void AddTipstaffRecord(Models.TipstaffRecord record)
        {
            var entity = GetDynamoTable(record);

            _tipstaffRecordRepository.Add(entity);
        }

        public IEnumerable<Models.TipstaffRecord> GetAll()
        {
            var entities = _tipstaffRecordRepository.GetAll();

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
               // addresses = _addressPresenter.GetAddressesByTipstaffRecordId(table.Id),
                arrestCount = table.ArrestCount,
                createdBy = table.CreatedBy,
                createdOn = table.CreatedOn,
                nextReviewDate = table.NextReviewDate,
                NPO = table.NPO,
                DateExecuted = table.DateExecuted,
                prisonCount = table.PrisonCount,
                resultDate = table.ResultDate,
                resultEnteredBy = table.ResultEnteredBy,
               // AttendanceNotes = _attendanceNotePresenter.GetAllById(table.Id),
                //Respondents = _respondentPresenter.GetAllById(table.Id),
                //caseReviews = _caseReviewPresenter.GetAllById(table.Id),
                Descriminator = table.Discriminator,
                result = MemoryCollections.ResultsList.GetResultByDetail(table.Result),
                tipstaffRecordID = table.Id,
                caseStatus = MemoryCollections.CaseStatusList.GetCaseStatusByDetail(table.CaseStatus),
                protectiveMarking = MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingByDetail(table.ProtectiveMarking)
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
            var entity = new Tipstaff.Services.DynamoTables.TipstaffRecord()
            {
                ArrestCount = record.arrestCount,
                Discriminator = record.Descriminator,
                DateExecuted = record.DateExecuted,
                CreatedBy = record.createdBy,
                NPO = record.NPO,
                Id = record.tipstaffRecordID,
                NextReviewDate = record.nextReviewDate,
                PrisonCount = record.prisonCount,
                ResultDate = record.resultDate,
                ProtectiveMarking = record.protectiveMarking?.Detail,
                CaseStatus = record.caseStatus?.Detail,
                CreatedOn = record.createdOn
            };
            
            _tipstaffRecordRepository.Update(entity);
        }
    }
}