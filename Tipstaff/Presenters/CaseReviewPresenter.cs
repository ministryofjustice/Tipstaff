using System;
using System.Collections.Generic;
using System.Linq;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class CaseReviewPresenter : ICaseReviewPresenter , IMapper<Models.CaseReview, Tipstaff.Services.DynamoTables.CaseReview>
    {
        private readonly ICaseReviewRepository _caseReviewRepository;

        public CaseReviewPresenter(ICaseReviewRepository caseReviewRepository)
        {
            _caseReviewRepository = caseReviewRepository;
        }

        public void Add(Models.CaseReview caseReview)
        {
            var entity = GetDynamoTable(caseReview);

            _caseReviewRepository.Add(entity);
        }

        public IEnumerable<Models.CaseReview> GetAllById(string id)
        {
            var entities = _caseReviewRepository.GetAllById(id);

            var caseRevies = entities.Select(x => GetModel(x));

            return caseRevies;
        }

        public CaseReview GetCaseReviewByCompositeKey(string hashKey, string rangeKey)
        {
            var entity = _caseReviewRepository.GetEntityByKeys(hashKey, rangeKey);

            var model = GetModel(entity);

            return model;
        }

        public Services.DynamoTables.CaseReview GetDynamoTable(Models.CaseReview model)
        {
            var entity = new Services.DynamoTables.CaseReview()
            {
                ActionTaken = model.actionTaken,
                Id = model.caseReviewID,
                NextReviewDate = model.nextReviewDate,
                ReviewDate = model.reviewDate.Value,
                TipstaffRecordID = model.tipstaffRecordID,
                CaseReviewStatusId = MemoryCollections.CaseReviewStatusList.GetCaseReviewStatusList().FirstOrDefault(x=> x.Detail == model.caseReviewStatus.Detail).CaseReviewStatusId,
            };

            return entity;
        }

        public Models.CaseReview GetModel(Services.DynamoTables.CaseReview table)
        {
            var model = new Models.CaseReview()
            {
                actionTaken = table.ActionTaken,
                caseReviewID = table.Id,
                nextReviewDate = table.NextReviewDate,
                reviewDate = table.ReviewDate,
                tipstaffRecordID = table.TipstaffRecordID,
                caseReviewStatus = MemoryCollections.CaseReviewStatusList.GetCaseReviewStatusList().FirstOrDefault(x=>x.CaseReviewStatusId == table.CaseReviewStatusId),
            };

            return model;
        }
    }
}