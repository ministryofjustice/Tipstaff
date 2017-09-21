using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Infrastructure.DynamoAPI
{
    public interface IDynamoAPI<T>
    {
        //void CreateItem(Document document, string table);

        //UpdateItemResponse UpdateItem(UpdateItemRequest updateRequest, string table);

        //Document GetItem(int key, string table, int rangeKey = 0);

        void Save(T entity);

        T GetEntity(object hashKey, object rangeKey);

        void Delete(T entity);
        
        IEnumerable<T> GetResultsByCondition(string name, ScanOperator scanOp, object value);

        IEnumerable<T> GetResultsByKey(object key);

        IEnumerable<T> GetAll();

        T GetEntityByHashKey(object hashKey);
        
    }
}
