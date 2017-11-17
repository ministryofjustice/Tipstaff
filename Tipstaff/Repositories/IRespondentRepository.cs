using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface IRespondentRepository
    {
        void Add(Respondent respondent);

        Respondent GetRespondent(string id);

        void Delete(Respondent respondent);

        void Update(Respondent respondent);

        IEnumerable<Respondent> GetAll();
    }
}
