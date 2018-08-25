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
        private readonly IDynamoAPI<Services.DynamoTables.Cache> _dynamoAPI;

        public CacheRepository(IDynamoAPI<Services.DynamoTables.Cache> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public void Add(Services.DynamoTables.Cache cache)
        {
            cache.Id = Guid.NewGuid().ToString();
            _dynamoAPI.Save(cache);
        }
    }
}
