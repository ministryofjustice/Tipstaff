using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class FAQRepository : IFAQRepository
    {
        private readonly IDynamoAPI<FAQ> _dynamoAPI;

        public FAQRepository(IDynamoAPI<FAQ> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }
        
        public void AddFaQ(FAQ faq)
        {
            _dynamoAPI.Save(faq);
        }

        public void Delete(FAQ faq)
        {
            _dynamoAPI.Delete(faq);
        }

        public IEnumerable<FAQ> GetAllFAQ()
        {
            return _dynamoAPI.GetAll();
        }

        public FAQ GetFAQ(string id)
        {
            return _dynamoAPI.GetEntityByHashKey(id);
        }

        public void Update(FAQ faq)
        {
            var entity = _dynamoAPI.GetEntityByHashKey(faq.Id);
            entity.Answer = faq.Answer;
            entity.LoggedInUser = faq.LoggedInUser;
            entity.Question = faq.Question;
            _dynamoAPI.Save(entity);
        }
    }
}
