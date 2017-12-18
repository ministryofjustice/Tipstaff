using System.Collections.Generic;
using System.Linq;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

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

        public IEnumerable<Document> GetAllDocumentssByTipstaffRecordID(string id)
        {
            return _dynamoAPI.GetAll().Where(c => c.TipstaffRecordID == id);
        }

        public Document GetDocument(string id)
        {
            return _dynamoAPI.GetEntityByHashKey(id);
        }

        public Document GetDocumentByIdAndRange(string id, string range)
        {
            return _dynamoAPI.GetEntity(id, range);
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
