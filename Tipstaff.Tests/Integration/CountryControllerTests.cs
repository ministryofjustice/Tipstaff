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
    public class CountryControllerTests
    {
        private CountryController _sub;
        private ICountryRepository _countryRepository;
        private IDynamoAPI<Tipstaff.Services.DynamoTables.Country> _dynamoAPI;
        string countryIndex = string.Empty;
        Country country;

        [SetUp]
        public void SetUp()
        {

            _dynamoAPI = new DynamoAPI<Tipstaff.Services.DynamoTables.Country>();
            _countryRepository = new CountryRepository(_dynamoAPI);
            countryIndex = GuidGenerator.GenerateTimeBasedGuid().ToString();
            _sub = new CountryController(_countryRepository);
        }

        [Test]
        public void Create_Should_Add_New_Country()
        {
            var response = _sub.Create(new Models.Country()
            {
                countryID = countryIndex,
                Detail = "Detail-Test",
                active = true
            });

            country = _countryRepository.GetCountry(countryIndex);

            Assert.AreEqual(countryIndex, country.CountryId);
        }

        [Test]
        public void Update_Should_Update_Country()
        {
            var response = _sub.Create(new Models.Country()
            {
                countryID = countryIndex,
                Detail = "Detail-Test",
                active = true
            });

            response = _sub.Edit(new Models.Country()
            {
                countryID = countryIndex,
                Detail = "Detail-Test modified",
                active = true
            });

            country = _countryRepository.GetCountry(countryIndex);

            Assert.AreEqual("Detail-Test modified", country.Detail);

        }

        [TearDown]
        public void TearDown()
        {
            _countryRepository.Delete(country);
        }
    }
}
