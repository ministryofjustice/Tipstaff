using NUnit.Framework;
using Tipstaff.Areas.Admin.Controllers;
using Tipstaff.Controllers;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Infrastructure.Services;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Tests.Integration
{
    [TestFixture]
    public class NationalityControllerTests
    {
        private NationalityController _sub;
        private INationalityRepository _nationalityRepository;
        private IDynamoAPI<Tipstaff.Services.DynamoTables.Nationality> _dynamoAPI;
        string nationalityIndex = string.Empty;
        Nationality nationality;

        [SetUp]
        public void SetUp()
        {

            _dynamoAPI = new DynamoAPI<Tipstaff.Services.DynamoTables.Nationality>();
            _nationalityRepository = new NationalityRepository(_dynamoAPI);
            nationalityIndex = GuidGenerator.GenerateTimeBasedGuid().ToString();
            _sub = new NationalityController(_nationalityRepository);
        }

        [Test]
        public void Create_Should_Add_New_Nationality()
        {
            var response = _sub.Create(new Models.Nationality()
            {
                nationalityID = nationalityIndex,
                Detail = "Detail-Test",
                active = true
            });

            nationality = _nationalityRepository.GetNationality(nationalityIndex);

            Assert.AreEqual(nationalityIndex, nationality.NationalityId);
        }

        [Test]
        public void Update_Should_Update_Nationality()
        {
            var response = _sub.Create(new Models.Nationality()
            {
                nationalityID = nationalityIndex,
                Detail = "Detail-Test",
                active = true
            });

            response = _sub.Edit(new Models.Nationality()
            {
                nationalityID = nationalityIndex,
                Detail = "Detail-Test modified",
                active = true
            });

            nationality = _nationalityRepository.GetNationality(nationalityIndex);

            Assert.AreEqual("Detail-Test modified", nationality.Detail);

        }

        [TearDown]
        public void TearDown()
        {
            _nationalityRepository.Delete(nationality);
        }
    }
}
