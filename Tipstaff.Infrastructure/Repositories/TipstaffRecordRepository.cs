using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using Tipstaff.Infrastructure.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class TipstaffRecordRepository : ITipstaffRecordRepository
    {
        private readonly IDynamoAPI<TipstaffRecord> _dynamoAPI;

        public TipstaffRecordRepository(IDynamoAPI<TipstaffRecord> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
           
        }

        public void Add(TipstaffRecord record)
        {
            _dynamoAPI.Save(record);
        }
        
        public TipstaffRecord GetEntityByObjectKey(object hashKey, object rangeKey)
        {
            return _dynamoAPI.GetEntity(hashKey, rangeKey);
        }

        public TipstaffRecord GetEntityByHashKey(object hashKey)
        {
            return _dynamoAPI.GetEntityByHashKey(hashKey);
        }
    }
}
