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
    public class TipstaffPoliceForcesRepository : ITipstaffPoliceForcesRepository
    {
        private readonly IDynamoAPI<Tipstaff_PoliceForces> _dynamoAPI;
        private readonly IAuditEventRepository _auditRepo;

        public TipstaffPoliceForcesRepository(IDynamoAPI<Tipstaff_PoliceForces> dynamoAPI, IAuditEventRepository auditRepo)
        {
            _dynamoAPI = dynamoAPI;
            _auditRepo = auditRepo;
        }
        public void Add(Tipstaff_PoliceForces tpf)
        {
            _dynamoAPI.Save(tpf);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "TipstaffPoliceForce added",
                EventDate = DateTime.Now,
                RecordChanged = tpf.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public void Delete(Tipstaff_PoliceForces tpf)
        {
            _dynamoAPI.Delete(tpf);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "TipstaffPoliceForce deleted",
                EventDate = DateTime.Now,
                RecordChanged = tpf.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public Tipstaff_PoliceForces GetTipstaffPoliceForces(string id)
        {
            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition("Id", ScanOperator.Equal, id)
                }).FirstOrDefault();
        }

        public IEnumerable<Tipstaff_PoliceForces> GetTipstaffPoliceForcesByTipstaffRecordID(string id)
        {
            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition("TipstaffRecordID", ScanOperator.Equal, id)
                });
        }

        public void Update(Tipstaff_PoliceForces tpf)
        {
            var entity = _dynamoAPI.GetEntityByKeys(tpf.Id, tpf.TipstaffRecordID);
            if (entity.PoliceForceID != tpf.PoliceForceID)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "TipstaffPoliceForce amended",
                    EventDate = DateTime.Now,
                    RecordChanged = tpf.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PoliceForceID",
                    Was = entity.PoliceForceID,
                    Now = tpf.PoliceForceID
                });
            }
            entity.PoliceForceID = tpf.PoliceForceID;

            _dynamoAPI.Save(entity);
        }
    }
}
