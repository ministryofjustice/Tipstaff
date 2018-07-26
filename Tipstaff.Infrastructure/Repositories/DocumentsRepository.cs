using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class DocumentsRepository : IDocumentsRepository
    {
        private readonly IDynamoAPI<Services.DynamoTables.Document> _dynamoAPI;
        private readonly IAuditEventRepository _auditRepo;

        public DocumentsRepository(IDynamoAPI<Services.DynamoTables.Document> dynamoAPI, IAuditEventRepository auditRepo)
        {
            _dynamoAPI = dynamoAPI;
            _auditRepo = auditRepo;
        }

        public void AddDocument(Services.DynamoTables.Document doc)
        {
            _dynamoAPI.Save(doc);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "Document added",
                EventDate = DateTime.Now,
                RecordChanged = doc.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public void DeleteDocument(Services.DynamoTables.Document doc)
        {
            _dynamoAPI.Delete(doc);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "Document deleted",
                EventDate = DateTime.Now,
                RecordChanged = doc.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public IEnumerable<Services.DynamoTables.Document> GetAllDocumentsByTipstaffRecordID(string id)
        {
            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition("TipstaffRecordID", ScanOperator.Equal, id)
                });
        }

        public Services.DynamoTables.Document GetDocument(string id)
        {
            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition("Id", ScanOperator.Equal, id)
                }).FirstOrDefault();
        }

        public Services.DynamoTables.Document GetDocumentByIdAndRange(string id, string range)
        {
            return _dynamoAPI.GetEntityByKeys(id, range);
        }

        public Services.DynamoTables.Document GetEntityByObjectKey(object hashKey, object rangeKey)
        {
            return _dynamoAPI.GetEntityByKeys(hashKey, rangeKey);
        }

        public Services.DynamoTables.Document GetEntityByObjectKey(object key)
        {
            return _dynamoAPI.GetEntityByKey(key);
        }
    }
}
