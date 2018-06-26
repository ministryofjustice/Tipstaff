using System;
using System.Collections.Generic;
using System.Linq;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class WarrantPresenter : IWarrantPresenter, IMapper<Warrant, Services.DynamoTables.TipstaffRecord>
    {
        private readonly ITipstaffRecordRepository _tipstaffRecordRepository;
        private readonly IAddressPresenter _addressPresenter;
        private readonly ICaseReviewPresenter _casereviewPresenter;
        private readonly IRespondentPresenter _respondentPresenter;
        private Object _lock = new Object();

        public WarrantPresenter(ITipstaffRecordRepository tipstaffRecordRepository, IAddressPresenter addressPresenter, ICaseReviewPresenter casereviewPresenter, IRespondentPresenter respondentPresenter)
        {
            _tipstaffRecordRepository = tipstaffRecordRepository;
            _addressPresenter = addressPresenter;
            _casereviewPresenter = casereviewPresenter;
            _respondentPresenter = respondentPresenter;
        }
        
        public void AddWarrant(Warrant warrant)
        {
            lock (_lock)
            {
                var entity = GetDynamoTable(warrant);

                var count = _tipstaffRecordRepository.GetAll().Count();

                entity.Id = $"{count++}";

                warrant.tipstaffRecordID = entity.Id;

                _tipstaffRecordRepository.Add(entity);
            }
        }

        public IEnumerable<Warrant> GetAllWarrants()
        {
            var records = _tipstaffRecordRepository.GetAllByCondition("Discriminator", "Warrant");

            var warrants = records.Select(x => GetModel(x));

            return warrants;
        }

        public Services.DynamoTables.TipstaffRecord GetDynamoTable(Warrant model)
        {
            var record = new Services.DynamoTables.TipstaffRecord()
            {
                Id = model.tipstaffRecordID,
                CaseNumber = model.caseNumber,
                ExpiryDate = model.expiryDate,
                RespondentName = model.RespondentName,
                DivisionId = MemoryCollections.DivisionsList.GetDivisionByID(model.Division.DivisionId)?.DivisionId,
                Discriminator = model.Discriminator,
                DateCirculated = model.DateCirculated,
                CaseStatusId = MemoryCollections.CaseStatusList.GetCaseStatusList().FirstOrDefault(x => x.CaseStatusId == model.caseStatusID)?.CaseStatusId,
                CreatedOn = model.createdOn,
                CreatedBy = model.createdBy
            };

            return record;
        }
        
        public Warrant GetWarrant(string id)
        {
            var record = _tipstaffRecordRepository.GetEntityByHashKey(id);

            var warrant = GetModel(record);

            return warrant;
        }

        public void RemoveWarrant(Warrant warrant)
        {
            var entity = GetDynamoTable(warrant);

            _tipstaffRecordRepository.Delete(entity);
        }


        public Warrant GetModel(Services.DynamoTables.TipstaffRecord table)
        {
            var entity = _tipstaffRecordRepository.GetEntityByHashKey(table.Id);

            var model = new Warrant()
            {
                Discriminator = table.Discriminator,
                tipstaffRecordID = table.Id,
                Division = MemoryCollections.DivisionsList.GetResultList().FirstOrDefault(x => x.DivisionId == table.DivisionId),
                caseNumber = table.CaseNumber,
                expiryDate = table.ExpiryDate,
                RespondentName = table.RespondentName,
                DateCirculated = table.DateCirculated,
                addresses = _addressPresenter.GetAddressesByTipstaffRecordId(table.Id),
                caseReviews = _casereviewPresenter.GetAllById(table.Id),
                caseStatus = MemoryCollections.CaseStatusList.GetCaseStatusList().FirstOrDefault(x => x.CaseStatusId == table.CaseStatusId),
                Respondents = _respondentPresenter.GetAllById(table.Id),
                createdBy = table.CreatedBy,
                createdOn = table.CreatedOn
            };

            return model;
        }

        public void UpdateWarrant(Warrant warrant)
        {
            var entity = GetDynamoTable(warrant);

            _tipstaffRecordRepository.Update(entity);
        }
    }
}
