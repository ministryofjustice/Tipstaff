using System;
using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class DeletedTipstaffRecordRepository : IDeletedTipstaffRecordRepository
    {
        private readonly IDynamoAPI<DeletedTipstaffRecord> _dynamoAPI;
        private readonly IAuditEventRepository _auditRepo;

        public DeletedTipstaffRecordRepository(IDynamoAPI<DeletedTipstaffRecord> dynamoAPI, IAuditEventRepository auditRepo)
        {
            _dynamoAPI = dynamoAPI;
            _auditRepo = auditRepo;
        }

        public void Add(DeletedTipstaffRecord record)
        {
            _dynamoAPI.Save(record);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "DeletedTipstaffRecord added",
                EventDate = DateTime.Now,
                RecordChanged = record.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public IEnumerable<DeletedTipstaffRecord> GetAll()
        {
            return _dynamoAPI.GetAll();
        }

        public void Remove(DeletedTipstaffRecord record)
        {
            _dynamoAPI.Delete(record);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "DeletedTipstaffRecord deleted",
                EventDate = DateTime.Now,
                RecordChanged = record.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }
    }
}
