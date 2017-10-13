using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Models;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class TipstaffRecordPresenter : ITipstaffRecordPresenter
    {
        private readonly ITipstaffRecordRepository _tipstaffRecordRepository;
        private readonly IAddressPresenter _addressPresernter;
        private readonly IAttendanceNotePresenter _attendanceNotePresenter;
        private readonly IRespondentPresenter _respondentPresenter;


        public TipstaffRecordPresenter(ITipstaffRecordRepository tipstaffRecordRepository, 
            IAddressPresenter addressPresernter, IAttendanceNotePresenter attendanceNotePresenter, 
            IRespondentPresenter respondentPresenter)
        {
            _tipstaffRecordRepository = tipstaffRecordRepository;
            _addressPresernter = addressPresernter;
            _attendanceNotePresenter = attendanceNotePresenter;
            _respondentPresenter = respondentPresenter;
        }
        
        public TipstaffRecord GetTipStaffRecord(string id)
        {
            var record = _tipstaffRecordRepository.GetEntityByHashKey(id);

            var model = new TipstaffRecord()
            {
                addresses = _addressPresernter.GetAddressByTipstaffRecordId(id),
                arrestCount = record.ArrestCount,
                createdBy = record.CreatedBy,
                createdOn = record.CreatedOn,
                nextReviewDate = record.NextReviewDate,
                NPO= record.NPO,
                DateExecuted = record.DateExecuted,
                prisonCount = record.PrisonCount,
                resultDate = record.ResultDate,
                resultEnteredBy = record.ResultEnteredBy,
                AttendanceNotes = _attendanceNotePresenter.GetAllById(id),
                Respondents = _respondentPresenter.GetAllById(id)
            };

            return model;
        }

        public void UpdateTipstaffRecord(TipstaffRecord record)
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