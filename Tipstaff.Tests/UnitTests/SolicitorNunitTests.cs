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
    public class SolicitorNunitTests
    {
        private ISolicitorRepository _solicitorRepository;
        private ISolicitorFirmRepository _firmRepository;
        private IDynamoAPI<Solicitor> _dynamoAPI;
        private IDynamoAPI<SolicitorFirm> _firmDynamoAPI;
        string solicitorIndex = string.Empty;
        string firmIndex = string.Empty;
        Solicitor sol;
        SolicitorFirm sf;

        [SetUp]
        public void SetUp()
        {

            _dynamoAPI = new DynamoAPI<Solicitor>();
            _firmDynamoAPI = new DynamoAPI<SolicitorFirm>();
            _solicitorRepository = new SolicitorRepository(_dynamoAPI);
            _firmRepository = new SolicitorFirmRepository(_firmDynamoAPI);
            solicitorIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
            firmIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
        }

        [Test]
        public void Create_Should_Add_New_Solicitor()
        {
            _firmRepository.AddSolicitorFirm(new SolicitorFirm()
            {
                SolicitorFirmID = firmIndex,
                FirmName = "Firm"
            });

            _solicitorRepository.AddSolicitor(new Solicitor() {
                SolicitorID = solicitorIndex,
                SolicitorFirmID = firmIndex,
                FirstName ="Solicitor first name",
                LastName = "Solicitor last name"

            });

            sol = _solicitorRepository.GetSolicitor(solicitorIndex);

            Assert.AreEqual(firmIndex, sol.SolicitorFirmID);
            Assert.AreEqual("Solicitor last name", sol.LastName);
            Assert.AreEqual("Solicitor first name", sol.FirstName);
        }

        [Test]
        public void Update_Should_Update_Solicitor()
        {
            _firmRepository.AddSolicitorFirm(new SolicitorFirm()
            {
                SolicitorFirmID = firmIndex,
                FirmName = "Firm"
            });

            _solicitorRepository.AddSolicitor(new Solicitor()
            {
                SolicitorID = solicitorIndex,
                SolicitorFirmID = firmIndex,
                FirstName = "Solicitor first name",
                LastName = "Solicitor last name"

            });

            _solicitorRepository.Update(new Solicitor()
            {
                SolicitorID = solicitorIndex,
                SolicitorFirmID = firmIndex,
                FirstName = "Solicitor first name modified",
                LastName = "Solicitor last name modified"

            });

            sol = _solicitorRepository.GetSolicitor(solicitorIndex);

            Assert.AreEqual(firmIndex, sol.SolicitorFirmID);
            Assert.AreEqual("Solicitor last name modified", sol.LastName);
            Assert.AreNotEqual("Solicitor first name", sol.FirstName);
        }

        [TearDown]
        public void TearDown()
        {
            _solicitorRepository.Delete(sol);
            _firmRepository.Delete(sf);
        }
    }
}
