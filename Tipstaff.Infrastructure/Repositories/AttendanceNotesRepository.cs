using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class AttendanceNotesRepository : IAttendanceNotesRepository  
    {
        private readonly IDynamoAPI<AttendanceNote> _dynamoAPI;
        private readonly IAuditEventRepository _auditRepo;

        public AttendanceNotesRepository(IDynamoAPI<AttendanceNote> dynamoAPI, IAuditEventRepository auditRepo)
        {
            _dynamoAPI = dynamoAPI;
            _auditRepo = auditRepo;
        }

        public void AddAttendanceNote(AttendanceNote note)
        {
            _dynamoAPI.Save(note);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "AttendanceNote added",
                EventDate = DateTime.Now,
                RecordChanged = note.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public void DeleteAttendanceNote(AttendanceNote note)
        {
            _dynamoAPI.Delete(note);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "AttendanceNote deleted",
                EventDate = DateTime.Now,
                RecordChanged = note.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public IEnumerable<AttendanceNote> GetAllById(string id)
        {
            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition("TipstaffRecordID", ScanOperator.Equal, id)
                });
        }

        public AttendanceNote GetAttendanceNote(string id)
        {
            return _dynamoAPI.GetEntityByKey(id);
        }

        public AttendanceNote GetAttendanceNoteByIdAndRange(string id, string range)
        {
            return _dynamoAPI.GetEntityByKeys(id, range);
        }

        public AttendanceNote GetEntityByObjectKey(object hashKey, object rangeKey)
        {
            return _dynamoAPI.GetEntityByKeys(hashKey, rangeKey);
        }

        public AttendanceNote GetEntityByObjectKey(object key)
        {
            return _dynamoAPI.GetEntityByKey(key);
        }
    }
}
