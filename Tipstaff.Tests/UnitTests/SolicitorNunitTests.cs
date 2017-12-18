using NUnit.Framework;
using Tipstaff.Services.Repositories;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Services.DynamoTables;
using TPLibrary.DynamoAPI;
using TPLibrary.GuidGenerator;

namespace Tipstaff.Tests.UnitTests
{
    [TestFixture]
    public class SolicitorNunitTests
    {
        private ISolicitorRepository _solicitorRepository;
        private IDynamoAPI<Solicitor> _dynamoAPI;
        string solicitorIndex = string.Empty;
        string firmIndex = string.Empty;
        Solicitor sol;

        [SetUp]
        public void SetUp()
        {

            _dynamoAPI = new DynamoAPI<Solicitor>();
            _solicitorRepository = new SolicitorRepository(_dynamoAPI);
            solicitorIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
            firmIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
        }

        [Test]
        public void Create_Should_Add_New_Solicitor()
        {
            _solicitorRepository.AddSolicitor(new Solicitor()
            {
                Id = solicitorIndex,
                SolicitorFirmID = firmIndex,
                FirstName = "Solicitor first name",
                LastName = "Solicitor last name"

            });

            sol = _solicitorRepository.GetSolicitorByIdAndRange(solicitorIndex, firmIndex);

            Assert.AreEqual(firmIndex, sol.SolicitorFirmID);
            Assert.AreEqual("Solicitor last name", sol.LastName);
            Assert.AreEqual("Solicitor first name", sol.FirstName);
        }

        [Test]
        public void Update_Should_Update_Solicitor()
        {
            _solicitorRepository.AddSolicitor(new Solicitor()
            {
                Id = solicitorIndex,
                SolicitorFirmID = firmIndex,
                FirstName = "Solicitor first name",
                LastName = "Solicitor last name"

            });

            _solicitorRepository.Update(new Solicitor()
            {
                Id = solicitorIndex,
                SolicitorFirmID = firmIndex,
                FirstName = "Solicitor first name modified",
                LastName = "Solicitor last name modified"

            });

            sol = _solicitorRepository.GetSolicitorByIdAndRange(solicitorIndex, firmIndex);

            Assert.AreEqual(firmIndex, sol.SolicitorFirmID);
            Assert.AreEqual("Solicitor last name modified", sol.LastName);
            Assert.AreNotEqual("Solicitor first name", sol.FirstName);
        }

        [TearDown]
        public void TearDown()
        {
            _solicitorRepository.Delete(sol);
        }
    }
}
