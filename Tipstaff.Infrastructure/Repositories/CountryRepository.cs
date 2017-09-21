using System.Collections.Generic;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IDynamoAPI<Country> _dynamoAPI;

        public CountryRepository(IDynamoAPI<Country> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public void AddCountry(Country country)
        {
            _dynamoAPI.Save(country);
        }

        public void Delete(Country country)
        {
            _dynamoAPI.Delete(country);
        }

        public IEnumerable<Country> GetAllCountries()
        {
            return _dynamoAPI.GetAll();
        }

        public Country GetCountry(string id)
        {
            return _dynamoAPI.GetEntityByHashKey(id);
        }

        public void Update(Country country)
        {
            var entity = _dynamoAPI.GetEntityByHashKey(country.CountryId);
            entity.Detail = country.Detail;
            entity.Active = country.Active;
            entity.Deactivated = country.Deactivated;
            entity.DeactivatedBy = country.DeactivatedBy;
            _dynamoAPI.Save(entity);
        }
    }
}
