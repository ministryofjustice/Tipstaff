using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface IAddressRepository
    {
        Address GetAddress(string id);
        
        void DeleteAddress(Address address);

        void AddAddress(Address address);

        void UpdateRepository(Address address);

        IEnumerable<Address> GetAllByCondition<T>(string name, T value);
    }
}
