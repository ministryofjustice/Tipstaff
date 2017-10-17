using System.Collections.Generic;
using Tipstaff.Mappers;
using Tipstaff.Presenters.Interfaces;
using Tipstaff.Services.Repositories;
using System.Linq;

namespace Tipstaff.Presenters
{
    public class FAQPresenter : IFAQPresenter, IMapper<Models.FAQ, Services.DynamoTables.FAQ>
    {
        private readonly IFAQRepository _faqRepository;

        public FAQPresenter(IFAQRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }

        public void Add(Models.FAQ faq)
        {
            var entity = GetDynamoTable(faq);

            _faqRepository.AddFaQ(entity);
        }

        public IEnumerable<Models.FAQ> GetAll()
        {
            var list = _faqRepository.GetAllFAQ();

            var faqList = list.Select(x => GetModel(x));

            return faqList;
        }

        public Models.FAQ GetById(string id)
        {
            var entity = _faqRepository.GetFAQ(id);

            var model = GetModel(entity);

            return model;
        }

        public Services.DynamoTables.FAQ GetDynamoTable(Models.FAQ model)
        {
            var table = new Services.DynamoTables.FAQ()
            {
                Id = model.faqID,
                Answer = model.answer,
                LoggedInUser = model.loggedInUser,
                Question = model.question
             };

            return table;
        }

        public Models.FAQ GetModel(Services.DynamoTables.FAQ entity)
        {
            var model = new Models.FAQ()
            {
                faqID = entity.Id,
                answer = entity.Answer,
                question = entity.Question,
                loggedInUser = entity.LoggedInUser
            };

            return model;
        }

        public void Update(Models.FAQ faq)
        {
            var entity = GetDynamoTable(faq);

            _faqRepository.Update(entity);
        }
    }
}