using System;
using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class SolicitorFirmRepository : ISolicitorFirmRepository
    {
        private readonly IDynamoAPI<SolicitorFirm> _dynamoAPI;
        private readonly IAuditEventRepository _auditRepo;

        public SolicitorFirmRepository(IDynamoAPI<SolicitorFirm> dynamoAPI, IAuditEventRepository auditRepo)
        {
            _dynamoAPI = dynamoAPI;
            _auditRepo = auditRepo;
        }

        public void AddSolicitorFirm(SolicitorFirm solicitorFirm)
        {
            _dynamoAPI.Save(solicitorFirm);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "SolicitorFirm added",
                EventDate = DateTime.Now,
                RecordChanged = solicitorFirm.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public void Delete(SolicitorFirm solicitorFirm)
        {
            _dynamoAPI.Delete(solicitorFirm);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "SolicitorFirm deleted",
                EventDate = DateTime.Now,
                RecordChanged = solicitorFirm.Id,
                UserId = solicitorFirm.DeactivatedBy
            });
        }

        public IEnumerable<SolicitorFirm> GetAllSolicitorFirms()
        {
            return _dynamoAPI.GetAll();
        }

        public SolicitorFirm GetSolicitorFirm(string id)
        {
            return _dynamoAPI.GetEntityByKey(id);
        }

        public void Update(SolicitorFirm solicitorFirm)
        {
            var entity = _dynamoAPI.GetEntityByKey(solicitorFirm.Id);
            if (entity.FirmName != solicitorFirm.FirmName)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "SolicitorFirm amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitorFirm.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PoliceForce FirmName",
                    Was = entity.FirmName,
                    Now = solicitorFirm.FirmName
                });
            }
            if (entity.AddressLine1 != solicitorFirm.AddressLine1)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "SolicitorFirm amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitorFirm.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PoliceForce AddressLine1",
                    Was = entity.AddressLine1,
                    Now = solicitorFirm.AddressLine1
                });
            }
            if (entity.AddressLine2 != solicitorFirm.AddressLine2)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "SolicitorFirm amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitorFirm.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PoliceForce AddressLine2",
                    Was = entity.AddressLine2,
                    Now = solicitorFirm.AddressLine2
                });
            }
            if (entity.AddressLine3 != solicitorFirm.AddressLine3)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "SolicitorFirm amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitorFirm.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PoliceForce AddressLine3",
                    Was = entity.AddressLine3,
                    Now = solicitorFirm.AddressLine3
                });
            }
            if (entity.Town != solicitorFirm.Town)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "SolicitorFirm amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitorFirm.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PoliceForce Town",
                    Was = entity.Town,
                    Now = solicitorFirm.Town
                });
            }
            if (entity.County != solicitorFirm.County)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "SolicitorFirm amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitorFirm.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PoliceForce County",
                    Was = entity.County,
                    Now = solicitorFirm.County
                });
            }
            if (entity.Postcode != solicitorFirm.Postcode)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "SolicitorFirm amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitorFirm.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PoliceForce Postcode",
                    Was = entity.Postcode,
                    Now = solicitorFirm.Postcode
                });
            }
            if (entity.DX != solicitorFirm.DX)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "SolicitorFirm amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitorFirm.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PoliceForce DX",
                    Was = entity.DX,
                    Now = solicitorFirm.DX
                });
            }
            if (entity.PhoneDayTime != solicitorFirm.PhoneDayTime)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "SolicitorFirm amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitorFirm.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PoliceForce PhoneDayTime",
                    Was = entity.PhoneDayTime,
                    Now = solicitorFirm.PhoneDayTime
                });
            }
            if (entity.PhoneOutofHours != solicitorFirm.PhoneOutofHours)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "SolicitorFirm amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitorFirm.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PoliceForce PhoneOutofHours",
                    Was = entity.PhoneOutofHours,
                    Now = solicitorFirm.PhoneOutofHours
                });
            }
            if (entity.Email != solicitorFirm.Email)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "SolicitorFirm amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitorFirm.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PoliceForce Email",
                    Was = entity.Email,
                    Now = solicitorFirm.Email
                });
            }
            if (entity.Active != solicitorFirm.Active)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "SolicitorFirm amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitorFirm.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PoliceForce Active",
                    Was = entity.Active.ToString(),
                    Now = solicitorFirm.Active.ToString()
                });
            }
            entity.FirmName = solicitorFirm.FirmName;
            entity.AddressLine1 = solicitorFirm.AddressLine1;
            entity.AddressLine2 = solicitorFirm.AddressLine2;
            entity.AddressLine3 = solicitorFirm.AddressLine3;
            entity.Town = solicitorFirm.Town;
            entity.County = solicitorFirm.County;
            entity.Postcode = solicitorFirm.Postcode;
            entity.DX = solicitorFirm.DX;
            entity.PhoneDayTime = solicitorFirm.PhoneDayTime;
            entity.PhoneOutofHours = solicitorFirm.PhoneOutofHours;
            entity.Email = solicitorFirm.Email;
            entity.Active = solicitorFirm.Active;
            entity.Deactivated = solicitorFirm.Deactivated;
            entity.DeactivatedBy = solicitorFirm.DeactivatedBy;

            _dynamoAPI.Save(entity);
        }
    }
}
