using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface IRespondentRepository
    {
        void Add(Respondent respondent);

        Respondent GetRespondent(string id);

        void Delete(Respondent respondent);

        void Update(Respondent respondent);
        
        IEnumerable<Respondent> GetAllRespondentsByTipstaffRecordID(string id);
    }
}
