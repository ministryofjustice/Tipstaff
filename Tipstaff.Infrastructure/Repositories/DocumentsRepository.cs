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
    public class DocumentsRepository : IDocumentsRepository
    {
        private readonly IDynamoAPI<Document> _dynamoAPI;

        public DocumentsRepository(IDynamoAPI<Document> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public void AddDocument(Document doc)
        {
            _dynamoAPI.Save(doc);
         }

        public void DeleteDocument(Document doc)
        {
            _dynamoAPI.Delete(doc);
        }

        public Document GetDocument(string id)
        {
            return _dynamoAPI.GetEntityByHashKey(id);
        }

        public Document GetEntityByObjectKey(object hashKey, object rangeKey)
        {
            return _dynamoAPI.GetEntity(hashKey, rangeKey);
        }

        public Document GetEntityByObjectKey(object key)
        {
            return _dynamoAPI.GetEntityByHashKey(key);
        }
    }
}
