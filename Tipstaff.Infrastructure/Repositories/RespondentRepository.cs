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
    public class RespondentRepository : IRespondentRepository
    {
        private readonly IDynamoAPI<Respondent> _dynamoAPI;
        
        public RespondentRepository(IDynamoAPI<Respondent> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }
        
        public void Add(Respondent respondent)
        {
            _dynamoAPI.Save(respondent);
        }

        public void Delete(Respondent respondent)
        {
            _dynamoAPI.Delete(respondent);
        }

        public Respondent GetRespondent(string id)
        {
            return _dynamoAPI.GetEntityByHashKey(id);
        }
    }
}
