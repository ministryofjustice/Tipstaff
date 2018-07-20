using NUnit.Framework;
using Tipstaff.Services.Repositories;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Services.DynamoTables;
using TPLibrary.DynamoAPI;
using TPLibrary.GuidGenerator;

namespace Tipstaff.Tests.UnitTests
{
    [TestFixture]
    public class PoliceForcesNunitTests
    {
        private IPoliceForcesRepository _pfRepository;
        private IAuditEventRepository _auditRepo;
        private IDynamoAPI<PoliceForces> _dynamoAPI;
        string pfIndex = string.Empty;
        PoliceForces pf;

        [SetUp]
        public void SetUp()
        {
            _dynamoAPI = new DynamoAPI<PoliceForces>();
            _auditRepo = new AuditEventRepository(new DynamoAPI<AuditEvent>(), new GuidGenerator());
            _pfRepository = new PoliceForcesRepository(_dynamoAPI, _auditRepo);
            pfIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
        }

        [Test]
        public void Create_Should_Add_New_Policeforce()
        {
            _pfRepository.AddPoliceForces(new PoliceForces() {
                Id = pfIndex,
                Active = true,
                PoliceForceEMail = "pf@test.nunit.com",
                PoliceForceName = "Police Force name!!",
                LoggedInUser = true
            });

            pf = _pfRepository.GetPoliceForces(pfIndex);

            Assert.AreEqual("pf@test.nunit.com", pf.PoliceForceEMail);
            Assert.AreEqual("Police Force name!!", pf.PoliceForceName);
            Assert.AreEqual(true, pf.LoggedInUser);
        }

        [Test]
        public void Update_Should_Update_PoliceForce()
        {
            _pfRepository.AddPoliceForces(new PoliceForces()
            {
                Id = pfIndex,
                Active = true,
                PoliceForceEMail = "pf@test.nunit.com",
                PoliceForceName = "Police Force name!!",
                LoggedInUser = true
            });

            _pfRepository.Update(new PoliceForces()
            {
                Id = pfIndex,
                Active = true,
                PoliceForceEMail = "pf@test.nunit.com modified",
                PoliceForceName = "Police Force name!! modified",
                LoggedInUser = false
            });

            pf = _pfRepository.GetPoliceForces(pfIndex);

            Assert.AreNotEqual("pf@test.nunit.com", pf.PoliceForceEMail);
            Assert.AreEqual("Police Force name!! modified", pf.PoliceForceName);
            Assert.AreNotEqual(true, pf.LoggedInUser);

        }

        [TearDown]
        public void TearDown()
        {
            _pfRepository.Delete(pf);
        }
    }
}
