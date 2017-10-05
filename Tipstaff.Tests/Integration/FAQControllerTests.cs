//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Tipstaff.Controllers;
//using Tipstaff.Infrastructure.DynamoAPI;
//using Tipstaff.Infrastructure.Repositories;
//using Tipstaff.Infrastructure.Services;
//using Tipstaff.Services.DynamoTables;
//using Tipstaff.Services.Repositories;

//namespace Tipstaff.Tests.Integration
//{

//    [TestFixture]
//    public class FAQControllerTests
//    {
//        private FAQController _sub;
//        private IFAQRepository _faqRepository;
//        private IDynamoAPI<Tipstaff.Services.DynamoTables.FAQ> _dynamoAPI;
//        string faqIndex = string.Empty;
//        FAQ faq;

//        [SetUp]
//        public void SetUp()
//        {

//            _dynamoAPI = new DynamoAPI<Tipstaff.Services.DynamoTables.FAQ>();
//            _faqRepository = new FAQRepository(_dynamoAPI);
//            faqIndex = GuidGenerator.GenerateTimeBasedGuid().ToString();
//            _sub = new FAQController(_faqRepository);
//        }

//        [Test]
//        public void Create_Should_Add_New_FAQ()
//        {
//            var response = _sub.Create(new Models.FAQ()
//            {
//                faqID = faqIndex,
//                answer = "Answer-Test",
//                loggedInUser = true,
//                question = "Question-Test?"
//            });

//            faq = _faqRepository.GetFAQ(faqIndex);

//            Assert.AreEqual(faqIndex, faq.FaqId);
//        }

//        [Test]
//        public void Update_Should_Update_FAQ()
//        {
//            var response = _sub.Create(new Models.FAQ()
//            {
//                faqID = faqIndex,
//                answer = "Answer-Test",
//                loggedInUser = true,
//                question = "Question-Test?"
//            });

//            response = _sub.Edit(new Models.FAQ()
//            {
//                faqID = faqIndex,
//                answer = "Answer-Test modified",
//                loggedInUser = true,
//                question = "Question-Test?"
//            });

//            faq = _faqRepository.GetFAQ(faqIndex);

//            Assert.AreEqual("Answer-Test modified", faq.Answer);

//        }

//        [TearDown]
//        public void TearDown()
//        {
//            _faqRepository.Delete(faq);
//        }
//    }
//}
