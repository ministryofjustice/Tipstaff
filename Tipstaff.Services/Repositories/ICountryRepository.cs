using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface ICountryRepository
    {
        void AddCountry(Country country);

        Country GetCountry(string id);

        IEnumerable<Country> GetAllCountries();

        void Update(Country country);

        void Delete(Country country);

    }
}
