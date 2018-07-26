using NUnit.Framework;
using Tipstaff.Services.Repositories;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Services.DynamoTables;
using TPLibrary.DynamoAPI;
using TPLibrary.GuidGenerator;

namespace Tipstaff.Tests.UnitTests
{
    [TestFixture]
    public class FAQNunitTests
    {
        private IFAQRepository _faqRepository;
        private IAuditEventRepository _auditRepo;
        private IDynamoAPI<FAQ> _dynamoAPI;
        string faqIndex = string.Empty;
        FAQ faq;

        [SetUp]
        public void SetUp()
        {
            _dynamoAPI = new DynamoAPI<FAQ>();
            _auditRepo = new AuditEventRepository(new DynamoAPI<AuditEvent>(), new GuidGenerator());
            _faqRepository = new FAQRepository(_dynamoAPI, _auditRepo);
            faqIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
        }

        [Test]
        public void Create_Should_Add_New_FAQ()
        {
            _faqRepository.AddFaQ(new FAQ() {
                Id = faqIndex,
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
                Id = faqIndex,
                Question = "this is the question",
                Answer = "this is the answer",
                LoggedInUser = true
            });

            _faqRepository.Update(new FAQ()
            {
                Id = faqIndex,
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
