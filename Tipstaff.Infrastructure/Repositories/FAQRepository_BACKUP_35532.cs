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
<<<<<<< HEAD
            var entity = _dynamoAPI.GetEntityByHashKey(faq.Id);
=======
            var entity = _dynamoAPI.GetEntityByHashKey(faq.FaqID);
>>>>>>> 1df7d64cb0c0f334f9ab93eaab05ca2466ed9d49
            entity.Answer = faq.Answer;
            entity.LoggedInUser = faq.LoggedInUser;
            entity.Question = faq.Question;
            _dynamoAPI.Save(entity);
        }
    }
}
