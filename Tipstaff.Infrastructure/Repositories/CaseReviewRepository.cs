using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class CaseReviewRepository : ICaseReviewRepository
    {
        private readonly IDynamoAPI<CaseReview> _dynamoAPI;
        private readonly IAuditEventRepository _auditRepo;

        public CaseReviewRepository(IDynamoAPI<CaseReview> dynamoAPI, IAuditEventRepository auditRepo)
        {
            _dynamoAPI = dynamoAPI;
            _auditRepo = auditRepo;
        }

        public void Add(CaseReview caseReview)
        {
            _dynamoAPI.Save(caseReview);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "CaseReview added",
                EventDate = DateTime.Now,
                RecordChanged = caseReview.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public void Delete(CaseReview caseReview)
        {
            _dynamoAPI.Delete(caseReview);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "CaseReview deleted",
                EventDate = DateTime.Now,
                RecordChanged = caseReview.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public IEnumerable<CaseReview> GetAllById(string id)
        {
            //return _dynamoAPI.GetResultsByKey(id);
            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition("TipstaffRecordID", ScanOperator.Equal, id)
                });
        }

        public CaseReview GetEntityByKeys(string hashKey, string rangeKey)
        {
            return _dynamoAPI.GetEntityByKeys(hashKey, rangeKey);
        }
    }
}
