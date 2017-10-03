using NUnit.Framework;
using System;
using Tipstaff.Controllers;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Infrastructure.Services;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Tests.Integration
{
    public class PoliceForcesControllerTests
    {
        [TestFixture]
        public class PoliceControllerTests
        {
            private PoliceForcesController _sub;
            private IPoliceForcesRepository _policeforcesRepository;
            private IDynamoAPI<Tipstaff.Services.DynamoTables.PoliceForces> _dynamoAPI;
            string policeforcesIndex = string.Empty;
            PoliceForces policeforces;


            [SetUp]
            public void SetUp()
            {
                _dynamoAPI = new DynamoAPI<Tipstaff.Services.DynamoTables.PoliceForces>();
                _policeforcesRepository = new PoliceForcesRepository(_dynamoAPI);
                policeforcesIndex = GuidGenerator.GenerateTimeBasedGuid().ToString();
                _sub = new PoliceForcesController(_policeforcesRepository);
            }

            [Test]
            public void Create_Should_Add_New_PoliceForce()
            {
                var response = _sub.Create(new Models.PoliceForces()
                {
                   policeForceID = policeforcesIndex,
                   loggedInUser = true, 
                   policeForceName = "Police force name - test",
                   policeForceEmail = "Police force email - test",
                   active = true,
                   deactivated = DateTime.Now,
                   deactivatedBy = "Deactivated by - test"
        });

                policeforces = _policeforcesRepository.GetPoliceForces(policeforcesIndex);
                Assert.AreEqual(policeforcesIndex, policeforces.PoliceForceId);
            }

            [Test]
            public void Update_Should_Update_PoliceForces()
            {
            }

            [TearDown]
            public void TearDown()
            {
                _policeforcesRepository.Delete(policeforces);
            }
        }
    }
}
