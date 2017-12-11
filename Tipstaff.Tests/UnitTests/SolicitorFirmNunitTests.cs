using NUnit.Framework;
using Tipstaff.Services.Repositories;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Services.DynamoTables;
using TPLibrary.DynamoAPI;
using TPLibrary.GuidGenerator;

namespace Tipstaff.Tests.UnitTests
{
    [TestFixture]
    public class SolicitorFirmNunitTests
    {
        private ISolicitorFirmRepository _firmRepository;
        private IDynamoAPI<SolicitorFirm> _dynamoAPI;
        string firmIndex = string.Empty;
        SolicitorFirm firm;

        [SetUp]
        public void SetUp()
        {

            _dynamoAPI = new DynamoAPI<SolicitorFirm>();
            _firmRepository = new SolicitorFirmRepository(_dynamoAPI);
            firmIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
        }

        [Test]
        public void Create_Should_Add_New_SolicitorFirm()
        {
            _firmRepository.AddSolicitorFirm(new SolicitorFirm() {
                Id = firmIndex,
                AddressLine1 = "Line 1",
                AddressLine2 = "Line 2",
                AddressLine3 = "Line 3",
                County = "County name",
                PhoneDayTime = "any phone home",
                Postcode = "postcode London",
                Town = "London",
                FirmName="Firm Name",
                Active = true

            });

            firm = _firmRepository.GetSolicitorFirm(firmIndex);

            Assert.AreEqual("Line 1", firm.AddressLine1);
            Assert.AreEqual("Line 2", firm.AddressLine2);
            Assert.AreEqual("Line 3", firm.AddressLine3);
            Assert.AreEqual("County name", firm.County);
            Assert.AreEqual("any phone home", firm.PhoneDayTime);
            Assert.AreEqual("postcode London", firm.Postcode);
            Assert.AreEqual("London", firm.Town);
            Assert.AreEqual("Firm Name", firm.FirmName);

        }

        [Test]
        public void Update_Should_Update_SolicitorFirm()
        {
            _firmRepository.AddSolicitorFirm(new SolicitorFirm()
            {
                Id = firmIndex,
                AddressLine1 = "Line 1",
                AddressLine2 = "Line 2",
                AddressLine3 = "Line 3",
                County = "County name",
                PhoneDayTime = "any phone home",
                Postcode = "postcode London",
                Town = "London",
                FirmName = "Firm Name",
                Active = true

            });

            _firmRepository.Update(new SolicitorFirm()
            {
                Id = firmIndex,
                AddressLine1 = "Line 1 modified",
                AddressLine2 = "Line 2",
                AddressLine3 = "Line 3 modified",
                County = "County name",
                PhoneDayTime = "any phone home",
                Postcode = "postcode London modified",
                Town = "London",
                FirmName = "Firm Name modified",
                Active = true

            });

            firm = _firmRepository.GetSolicitorFirm(firmIndex);

            Assert.AreNotEqual("Line 1", firm.AddressLine1);
            Assert.AreEqual("Line 2", firm.AddressLine2);
            Assert.AreEqual("Line 3 modified", firm.AddressLine3);
            Assert.AreEqual("County name", firm.County);
            Assert.AreNotEqual("any phone home modified", firm.PhoneDayTime);
            Assert.AreNotEqual("postcode London", firm.Postcode);
            Assert.AreEqual("London", firm.Town);
            Assert.AreEqual("Firm Name modified", firm.FirmName);
        }

        [TearDown]
        public void TearDown()
        {
            _firmRepository.Delete(firm);
        }
    }
}
