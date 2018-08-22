using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPLibrary.DynamoAPI;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Infrastructure.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDynamoAPI<CacheStore> _dynamoAPI;

        public CacheRepository(IDynamoAPI<CacheStore> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public void Add(CacheStore cache)
        {
            _dynamoAPI.Save(cache);
        }
    }
}
