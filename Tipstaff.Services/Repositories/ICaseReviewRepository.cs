using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface ICaseReviewRepository
    {
        void Add(CaseReview caseReview);

        IEnumerable<CaseReview> GetAllById(string id);
    }
}
