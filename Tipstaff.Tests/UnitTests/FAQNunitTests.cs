using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Tipstaff.Services.Repositories;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Infrastructure.Services;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Tests.UnitTests
{
    [TestFixture]
    public class FAQNunitTests
    {
        private IFAQRepository _faqRepository;
        private IDynamoAPI<FAQ> _dynamoAPI;
        string faqIndex = string.Empty;
        FAQ faq;

        [SetUp]
        public void SetUp()
        {

            _dynamoAPI = new DynamoAPI<FAQ>();
            _faqRepository = new FAQRepository(_dynamoAPI);
            faqIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
        }

        [Test]
        public void Create_Should_Add_New_FAQ()
        {
            _faqRepository.AddFaQ(new FAQ() {
                FaqID = faqIndex,
                Question="this is the question",
                Answer = "this is the answer",
                LoggedInUser = true
            });

            faq = _faqRepository.GetFAQ(faqIndex);

            Assert.AreEqual("this is the question", faq.Question);
            Assert.AreEqual("this is the answer", faq.Answer);
            Assert.AreEqual(true, faq.LoggedInUser);
        }

        [Test]
        public void Update_Should_Update_FAQ()
        {
            _faqRepository.AddFaQ(new FAQ()
            {
                FaqID = faqIndex,
                Question = "this is the question",
                Answer = "this is the answer",
                LoggedInUser = true
            });

            _faqRepository.Update(new FAQ()
            {
                FaqID = faqIndex,
                Question = "this is the question updated",
                Answer = "this is the answer updated",
                LoggedInUser = false
            });

            faq = _faqRepository.GetFAQ(faqIndex);

            Assert.AreEqual("this is the question updated", faq.Question);
            Assert.AreEqual("this is the answer updated", faq.Answer);
            Assert.AreEqual(false, faq.LoggedInUser);

        }

        [TearDown]
        public void TearDown()
        {
            _faqRepository.Delete(faq);
        }
    }
}
