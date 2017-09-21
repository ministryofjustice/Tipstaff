using System;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Tipstaff.Infrastructure.DynamoAPI
{
    public class DynamoAPI<T> : IDynamoAPI<T>
    {
        //private Table _table;
        private AmazonDynamoDBConfig _awsDynamoDBConfig;
        private AmazonDynamoDBClient _awsDynamoDBClient;
        private DynamoDBContext _dynamoDBContext;
        
        public DynamoAPI()
        {
            _awsDynamoDBConfig = new AmazonDynamoDBConfig();
           
            try
            {
                _awsDynamoDBConfig.ServiceURL = "ec2.eu-west-2.amazonaws.com";
                _awsDynamoDBConfig.RegionEndpoint = Amazon.RegionEndpoint.EUWest2;
                _awsDynamoDBClient = new AmazonDynamoDBClient( "AKIAIYOJJPVKTI5E6DLA", "h61Diom/SlmOuHu7LlOLDWsbnHKa6tqnZ0BN+A9C", _awsDynamoDBConfig);
                _dynamoDBContext = new DynamoDBContext(_awsDynamoDBClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n Error: failed to create a DynamoDB client; " + ex.Message);
            }
        }
        
        public void Save(T entity)
        {
            _dynamoDBContext.Save(entity);
        }

        public void Delete(T entity)
        {
            _dynamoDBContext.Delete(entity);
        }

        public T GetEntity(object hashKey, object rangeKey )
        {
           return  _dynamoDBContext.Load<T>(hashKey, rangeKey);
        }

        public T GetEntityByHashKey(object hashKey)
        {
            return _dynamoDBContext.Load<T>(hashKey);
        }

        public IEnumerable<T> GetResultsByCondtion(object key, QueryOperator op, object range)
        {
            return _dynamoDBContext.Query<T>(key, op, range);
        }

        public IEnumerable<T> GetResultsByKey(object key)
        {
            return _dynamoDBContext.Query<T>(key);
        }
        
        public IEnumerable<T> GetResultsByCondition(string name, ScanOperator scanOp, object value)
        {
            return _dynamoDBContext.Scan<T>(new ScanCondition(name, scanOp, value));
        }

        public IEnumerable<T> GetAll()
        {
            return _dynamoDBContext.Scan<T>();
        }
        
    }
}
