using System.Collections.Generic;
using System.Linq;
using Tipstaff.Mappers;
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

        public Services.DynamoTables.CaseReview GetDynamoTable(Models.CaseReview model)
        {
            var entity = new Services.DynamoTables.CaseReview()
            {
                ActionTaken = model.actionTaken,
                Id = model.caseReviewID,
                NextReviewDate = model.nextReviewDate,
                ReviewDate = model.reviewDate.Value,
                TipstaffRecordId = model.tipstaffRecordID,
                CaseReviewStatus = MemoryCollections.CaseReviewStatusList.GetCaseReviewStatusList().FirstOrDefault(x=> x.Detail == model.caseReviewStatus.Detail).Detail,
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
                tipstaffRecordID = table.TipstaffRecordId,
                caseReviewStatus = MemoryCollections.CaseReviewStatusList.GetCaseReviewStatusList().FirstOrDefault(x=>x.Detail == table.CaseReviewStatus),
            };

            return model;
        }
    }
}