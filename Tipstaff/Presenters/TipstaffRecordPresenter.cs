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

        public TipstaffRecordPresenter(ITipstaffRecordRepository tipstaffRecordRepository, IAddressPresenter addressPresernter)
        {
            _tipstaffRecordRepository = tipstaffRecordRepository;
            _addressPresernter = addressPresernter;
        }

        public IEnumerable<TipstaffRecord> GettAllRecords()
        {
            throw new NotImplementedException();
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
                resultEnteredBy = record.ResultEnteredBy
            };

            return model;
        }

        public void UpdateTipstaffRecord(TipstaffRecord record)
        {
            throw new NotImplementedException();
        }
    }
}