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
    public class TipstaffRecordPresenter : ITipstaffRecordPresenter, IMapper<Models.TipstaffRecord, Tipstaff.Services.DynamoTables.TipstaffRecord>
    {
        private readonly ITipstaffRecordRepository _tipstaffRecordRepository;
        private readonly IAddressPresenter _addressPresenter;
        private readonly IAttendanceNotePresenter _attendanceNotePresenter;
        private readonly IRespondentPresenter _respondentPresenter;
        private readonly ICaseReviewPresenter _caseReviewPresenter;


        public TipstaffRecordPresenter(ITipstaffRecordRepository tipstaffRecordRepository, 
                                       IAddressPresenter addressPresernter, 
                                       IAttendanceNotePresenter attendanceNotePresenter, 
                                       IRespondentPresenter respondentPresenter, 
                                       ICaseReviewPresenter caseReviewPresenter)
        {
            _tipstaffRecordRepository = tipstaffRecordRepository;
            _addressPresenter = addressPresernter;
            _attendanceNotePresenter = attendanceNotePresenter;
            _respondentPresenter = respondentPresenter;
            _caseReviewPresenter = caseReviewPresenter;
        }

        public IEnumerable<Models.TipstaffRecord> GetAll()
        {
            var entities = _tipstaffRecordRepository.GetAll();

            var records = entities.Select(x => GetModel(x));

            return records;
        }

        public Services.DynamoTables.TipstaffRecord GetDynamoTable(Models.TipstaffRecord table)
        {
            throw new NotImplementedException();
        }

        public Models.TipstaffRecord GetModel(Services.DynamoTables.TipstaffRecord table)
        {
            var model = new Models.TipstaffRecord()
            {
                addresses = _addressPresenter.GetAddressesByTipstaffRecordId(table.Id),
                arrestCount = table.ArrestCount,
                createdBy = table.CreatedBy,
                createdOn = table.CreatedOn,
                nextReviewDate = table.NextReviewDate,
                NPO = table.NPO,
                DateExecuted = table.DateExecuted,
                prisonCount = table.PrisonCount,
                resultDate = table.ResultDate,
                resultEnteredBy = table.ResultEnteredBy,
                AttendanceNotes = _attendanceNotePresenter.GetAllById(table.Id),
                Respondents = _respondentPresenter.GetAllById(table.Id),
                caseReviews = _caseReviewPresenter.GetAllById(table.Id),
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
                ProtectiveMarking = record.protectiveMarking.Detail,
                CaseStatus = record.caseStatus.Detail
            };
            
            _tipstaffRecordRepository.Update(entity);
        }
    }
}