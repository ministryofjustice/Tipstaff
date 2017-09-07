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
        private Table _table;
        private AmazonDynamoDBConfig _awsDynamoDBConfig;
        private AmazonDynamoDBClient _awsDynamoDBClient;
        private DynamoDBContext _dynamoDBContext;
        
        public DynamoAPI()
        {
            _awsDynamoDBConfig = new AmazonDynamoDBConfig();

            try
            {
                _awsDynamoDBClient = new AmazonDynamoDBClient(_awsDynamoDBConfig);
                _dynamoDBContext = new DynamoDBContext(_awsDynamoDBClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n Error: failed to create a DynamoDB client; " + ex.Message);
            }
        }

        //public void CreateItem(Document document, string table)
        //{
        //    _table = Table.LoadTable(_awsDynamoDBClient, table);
        //    _table.PutItem(document);
        //}

        //public CreateTableResponse CreateTable(CreateTableRequest request, string table)
        //{
        //    var response = _awsDynamoDBClient.CreateTable(request);
        //    return response;
        //}

        //public Document GetItem(int key,string table, int rangeKey = 0)
        //{
        //    _table = Table.LoadTable(_awsDynamoDBClient, table);
        //    var doc = _table.GetItem(key, rangeKey);
        //    return doc;
        //}

        //public UpdateItemResponse UpdateItem(UpdateItemRequest updateRequest, string table)
        //{
        //    var response = _awsDynamoDBClient.UpdateItem(updateRequest);
        //    return response;
        //}
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

        public IEnumerable<T> GetResultsByCondtion(object key, QueryOperator op, object range)
        {
            return _dynamoDBContext.Query<T>(key, op, range);
        }

        public IEnumerable<T> GetResultsByKey(object key)
        {
            return _dynamoDBContext.Query<T>(key);
        }

        public IEnumerable<T> GetResultsByCondition(string name, ScanOperator scanOp, object value, IList<T> list)
        {
            return _dynamoDBContext.Scan<T>(new ScanCondition(name, scanOp, value));
        }

        public IEnumerable<T> GetResultsByCondition(object key, QueryOperator op, object range)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetResultsByCondition(string name, ScanOperator scanOp, object value)
        {
            throw new NotImplementedException();
        }
    }
}
