using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public interface IRespondentPresenter
    {
        void Add(Respondent respondent);

        Respondent GetRespondent(string id);

        void Delete(Respondent respondent);

        void Update(Respondent respondent);
        
        IEnumerable<Respondent> GetAllById(string id);

        Models.Respondent GetRespondentByKeys(string id, string rangeKey);
    }
}
