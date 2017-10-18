using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public interface ICaseReviewPresenter
    {
        void Add(CaseReview caseReview);

        IEnumerable<CaseReview> GetAllById(string id);

        CaseReview GetCaseReviewByCompositeKey(string hashKey, string rangeKey);
    }
}
