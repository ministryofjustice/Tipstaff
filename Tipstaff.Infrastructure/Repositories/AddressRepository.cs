using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Infrastructure.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly IDynamoAPI<Address> _dynamoAPI;

        public AddressRepository(IDynamoAPI<Address> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }


        public void AddAddress(Address address)
        {
            _dynamoAPI.Save(address);
        }

        public void DeleteAddress(Address address)
        {
            _dynamoAPI.Delete(address);
        }

        public Address GetAddress(string id)
        {
            return _dynamoAPI.GetEntityByHashKey(id);
        }
        

        public void UpdateRepository(Address address)
        {
          // _dynamoAPI.
        }

        public IEnumerable<Address> GetAllByCondition<T>(string name, T value)
        {
            return _dynamoAPI.GetResultsByCondition(name, Amazon.DynamoDBv2.DocumentModel.ScanOperator.GreaterThan, value);
        }
    }
}
