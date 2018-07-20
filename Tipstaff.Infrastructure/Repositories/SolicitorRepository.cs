using System;
using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class SolicitorRepository : ISolicitorRepository
    {
        private readonly IDynamoAPI<Solicitor> _dynamoAPI;
        private readonly IAuditEventRepository _auditRepo;

        public SolicitorRepository(IDynamoAPI<Solicitor> dynamoAPI, IAuditEventRepository auditRepo)
        {
            _dynamoAPI = dynamoAPI;
            _auditRepo = auditRepo;
        }

        public void AddSolicitor(Solicitor solicitor)
        {
            _dynamoAPI.Save(solicitor);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "Solicitor added",
                EventDate = DateTime.Now,
                RecordChanged = solicitor.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public void Delete(Solicitor solicitor)
        {
            _dynamoAPI.Delete(solicitor);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "Solicitor deleted",
                EventDate = DateTime.Now,
                RecordChanged = solicitor.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public Solicitor GetSolicitor(string id)
        {
            return _dynamoAPI.GetEntityByKey(id);
        }

        public Solicitor GetSolicitorByIdAndRange(string id, string range)
        {
            return _dynamoAPI.GetEntityByKeys(id, range);
        }

        public IEnumerable<Solicitor> GetSolicitors()
        {
            return _dynamoAPI.GetAll();
        }

        public void Update(Solicitor solicitor)
        {
            var entity = _dynamoAPI.GetEntityByKeys(solicitor.Id, solicitor.SolicitorFirmID);
            if (entity.Active != solicitor.Active)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Solicitor amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitor.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Active",
                    Was = entity.Active.ToString(),
                    Now = solicitor.Active.ToString()
                });
            }
            if (entity.FirstName != solicitor.FirstName)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Solicitor amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitor.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "FirstName",
                    Was = entity.FirstName,
                    Now = solicitor.FirstName
                });
            }
            if (entity.LastName != solicitor.LastName)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Solicitor amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitor.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "LastName",
                    Was = entity.LastName,
                    Now = solicitor.LastName
                });
            }
            if (entity.PhoneDayTime != solicitor.PhoneDayTime)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Solicitor amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitor.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PhoneDayTime",
                    Was = entity.PhoneDayTime,
                    Now = solicitor.PhoneDayTime
                });
            }
            if (entity.PhoneOutOfHours != solicitor.PhoneOutOfHours)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Solicitor amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitor.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PhoneOutOfHours",
                    Was = entity.PhoneOutOfHours,
                    Now = solicitor.PhoneOutOfHours
                });
            }
            if (entity.Email != solicitor.Email)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Solicitor amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitor.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Email",
                    Was = entity.Email,
                    Now = solicitor.Email
                });
            }
            if (entity.Salutation != solicitor.Salutation)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Solicitor amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitor.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Salutation",
                    Was = entity.Salutation,
                    Now = solicitor.Salutation
                });
            }
            if (entity.SolicitorFirmID != solicitor.SolicitorFirmID)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Solicitor amended",
                    EventDate = DateTime.Now,
                    RecordChanged = solicitor.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "SolicitorFirmID",
                    Was = entity.SolicitorFirmID,
                    Now = solicitor.SolicitorFirmID
                });
            }
            entity.Active = solicitor.Active;
            entity.FirstName = solicitor.FirstName;
            entity.LastName = solicitor.LastName;
            entity.PhoneDayTime = solicitor.PhoneDayTime;
            entity.PhoneOutOfHours = solicitor.PhoneOutOfHours;
            entity.Dectivated = solicitor.Dectivated;
            entity.DectivatedBy = solicitor.DectivatedBy;
            entity.Email = solicitor.Email;
            entity.Salutation = solicitor.Salutation;
            entity.SolicitorFirmID = solicitor.SolicitorFirmID;
            entity.Id = solicitor.Id;
            _dynamoAPI.Save(entity);
        }
    }
}
