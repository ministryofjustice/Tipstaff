using NUnit.Framework;
using Tipstaff.Services.Repositories;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Services.DynamoTables;
using TPLibrary.DynamoAPI;
using TPLibrary.GuidGenerator;

namespace Tipstaff.Tests.UnitTests
{
    [TestFixture]
    public class ContactNunitTests
    {
        private IContactsRepository _contactRepository;
        private IAuditEventRepository _auditRepo;
        private IDynamoAPI<Contact> _dynamoAPI;
        string contactIndex = string.Empty;
        Contact contact;

        [SetUp]
        public void SetUp()
        {

            _dynamoAPI = new DynamoAPI<Contact>();
            _auditRepo = new AuditEventRepository(new DynamoAPI<AuditEvent>(), new GuidGenerator());
            _contactRepository = new ContactsRepository(_dynamoAPI, _auditRepo);
            contactIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
        }

        [Test]
        public void Create_Should_Add_New_Contact()
        {
            _contactRepository.AddContact(new Contact() {
                Id = contactIndex,
                AddressLine1 = "Line 1",
                AddressLine2 = "Line 2",
                AddressLine3 = "Line 3",
                County = "County name",
                PhoneHome = "any phone home",
                Postcode = "postcode London",
                Town = "London",
                LastName = "Last name"

            });

            contact = _contactRepository.GetContact(contactIndex);
            Assert.AreEqual("Line 1", contact.AddressLine1);
            Assert.AreEqual("Line 2", contact.AddressLine2);
            Assert.AreEqual("Line 3", contact.AddressLine3);
            Assert.AreEqual("County name", contact.County);
            Assert.AreEqual("any phone home", contact.PhoneHome);
            Assert.AreEqual("postcode London", contact.Postcode);
            Assert.AreEqual("London", contact.Town);

        }

        [Test]
        public void Update_Should_Update_Contact()
        {
            _contactRepository.AddContact(new Contact()
            {
                Id = contactIndex,
                AddressLine1 = "Line 1",
                AddressLine2 = "Line 2",
                AddressLine3 = "Line 3",
                County = "County name",
                PhoneHome = "any phone home",
                Postcode = "postcode London",
                Town = "London",
                LastName = "Last name"

            });

            _contactRepository.UpdateContact(new Contact()
            {
                Id = contactIndex,
                AddressLine1 = "Line 1 modified",
                AddressLine2 = "Line 2 modified",
                AddressLine3 = "Line 3 modified",
                County = "County name modified",
                PhoneHome = "any phone home modified",
                Postcode = "postcode London modified",
                Town = "London modified",
                LastName = "Last name modified"

            });

            contact = _contactRepository.GetContact(contactIndex);
            Assert.AreEqual("Line 1 modified", contact.AddressLine1);
            Assert.AreNotEqual("Line 2", contact.AddressLine2);
            Assert.AreNotEqual("Line 3", contact.AddressLine3);
            Assert.AreEqual("County name modified", contact.County);
            Assert.AreNotEqual("any phone home", contact.PhoneHome);
            Assert.AreEqual("postcode London modified", contact.Postcode);
            Assert.AreNotEqual("London", contact.Town);

        }

        [TearDown]
        public void TearDown()
        {
            _contactRepository.Delete(contact);
        }
    }
}
