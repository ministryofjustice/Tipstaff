using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Infrastructure.Repositories
{
    public class CaseReviewRepository : ICaseReviewRepository
    {
        private readonly IDynamoAPI<CaseReview> _dynamoAPI;

        public CaseReviewRepository(IDynamoAPI<CaseReview> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public void Add(CaseReview caseReview)
        {
            _dynamoAPI.Save(caseReview);
        }

        public IEnumerable<CaseReview> GetAllById(string id)
        {
            return _dynamoAPI.GetResultsByKey(id);
        }
    }
}
