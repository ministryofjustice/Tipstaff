using System;
using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;
using TPLibrary.GuidGenerator;

namespace Tipstaff.Infrastructure.Repositories
{
    public class PoliceForcesRepository : IPoliceForcesRepository
    {
        private readonly IDynamoAPI<PoliceForces> _dynamoAPI;
        private readonly IAuditEventRepository _auditRepo;

        public PoliceForcesRepository(IDynamoAPI<PoliceForces> dynamoAPI, IAuditEventRepository auditRepo)
        {
            _dynamoAPI = dynamoAPI;
            _auditRepo = auditRepo;
        }

        public void AddPoliceForces(PoliceForces policeForce)
        {
            _dynamoAPI.Save(policeForce);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "PoliceForce added",
                EventDate = DateTime.Now,
                RecordChanged = policeForce.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }


        public void Delete(PoliceForces policeforces)
        {
            _dynamoAPI.Delete(policeforces);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "PoliceForce deleted",
                EventDate = DateTime.Now,
                RecordChanged = policeforces.Id,
                UserId = policeforces.DeactivatedBy
            });
        }

        public IEnumerable<PoliceForces> GetAllPoliceForces()
        {
            return _dynamoAPI.GetAll();
        }

        public PoliceForces GetPoliceForces(string id)
        {
            return _dynamoAPI.GetEntityByKey(id);
        }

        public void Update(PoliceForces policeforces)
        {
            var entity = _dynamoAPI.GetEntityByKey(policeforces.Id);
            if (entity.Active != policeforces.Active)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "PoliceForce amended",
                    EventDate = DateTime.Now,
                    RecordChanged = policeforces.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Active",
                    Was = entity.Active.ToString(),
                    Now = policeforces.Active.ToString()
                });
            }
            if (entity.PoliceForceEMail != policeforces.PoliceForceEMail)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "PoliceForce amended",
                    EventDate = DateTime.Now,
                    RecordChanged = policeforces.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PoliceForce EMail",
                    Was = entity.PoliceForceEMail,
                    Now = policeforces.PoliceForceEMail
                });
            }
            if (entity.PoliceForceName != policeforces.PoliceForceName)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "PoliceForce amended",
                    EventDate = DateTime.Now,
                    RecordChanged = policeforces.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PoliceForce Name",
                    Was = entity.PoliceForceName,
                    Now = policeforces.PoliceForceName
                });
            }
            if (entity.LoggedInUser != policeforces.LoggedInUser)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "PoliceForce amended",
                    EventDate = DateTime.Now,
                    RecordChanged = policeforces.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PoliceForce LoggedInUser",
                    Was = entity.LoggedInUser.ToString(),
                    Now = policeforces.LoggedInUser.ToString()
                });
            }
            entity.Active= policeforces.Active;
            entity.Deactivated = policeforces.Deactivated;
            entity.DeactivatedBy = policeforces.DeactivatedBy;
            entity.PoliceForceEMail = policeforces.PoliceForceEMail;
            entity.PoliceForceName = policeforces.PoliceForceName;
            entity.LoggedInUser = policeforces.LoggedInUser;
            entity.Id = policeforces.Id;
            _dynamoAPI.Save(entity);
        }
    }
}
