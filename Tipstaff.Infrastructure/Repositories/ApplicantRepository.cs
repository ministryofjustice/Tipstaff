using System;
using System.Collections.Generic;
using System.Linq;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class ApplicantRepository : IApplicantRepository
    {
        private readonly IDynamoAPI<Applicant> _dynamoAPI;
        private readonly IAuditEventRepository _auditRepo;

        public ApplicantRepository(IDynamoAPI<Applicant> dynamoAPI, IAuditEventRepository auditRepo)
        {
            _dynamoAPI = dynamoAPI;
            _auditRepo = auditRepo;
        }

        public void AddApplicant(Applicant applicant)
        {
            _dynamoAPI.Save(applicant);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "Applicant added",
                EventDate = DateTime.Now,
                RecordChanged = applicant.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public void Delete(Applicant applicant)
        {
            _dynamoAPI.Delete(applicant);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "Applicant deleted",
                EventDate = DateTime.Now,
                RecordChanged = applicant.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public IEnumerable<Applicant> GetAllApplicants()
        {
            return _dynamoAPI.GetAll();
        }

        public IEnumerable<Applicant> GetAllApplicantsByTipstaffRecordID(string id)
        {
            return _dynamoAPI.GetAll().Where(c => c.TipstaffRecordID == id);
        }

        public Applicant GetApplicant(string id)
        {
            return _dynamoAPI.GetEntityByKey(id);
        }

        public void Update(Applicant applicant)
        {
            var entity = _dynamoAPI.GetEntityByKey(applicant.Id);
            if (entity.NameFirst != applicant.NameFirst)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Applicant amended",
                    EventDate = DateTime.Now,
                    RecordChanged = applicant.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "NameFirst",
                    Was = entity.NameFirst,
                    Now = applicant.NameFirst
                });
            }
            if (entity.NameLast != applicant.NameLast)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Applicant amended",
                    EventDate = DateTime.Now,
                    RecordChanged = applicant.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "NameLast",
                    Was = entity.NameLast,
                    Now = applicant.NameLast
                });
            }
            if (entity.AddressLine1 != applicant.AddressLine1)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Applicant amended",
                    EventDate = DateTime.Now,
                    RecordChanged = applicant.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "AddressLine1",
                    Was = entity.AddressLine1,
                    Now = applicant.AddressLine1
                });
            }
            if (entity.AddressLine2 != applicant.AddressLine2)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Applicant amended",
                    EventDate = DateTime.Now,
                    RecordChanged = applicant.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "AddressLine2",
                    Was = entity.AddressLine2,
                    Now = applicant.AddressLine2
                });
            }
            if (entity.AddressLine3 != applicant.AddressLine3)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Applicant amended",
                    EventDate = DateTime.Now,
                    RecordChanged = applicant.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "AddressLine3",
                    Was = entity.AddressLine3,
                    Now = applicant.AddressLine3
                });
            }
            if (entity.Town != applicant.Town)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Applicant amended",
                    EventDate = DateTime.Now,
                    RecordChanged = applicant.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Town",
                    Was = entity.Town,
                    Now = applicant.Town
                });
            }
            if (entity.County != applicant.County)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Applicant amended",
                    EventDate = DateTime.Now,
                    RecordChanged = applicant.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "County",
                    Was = entity.County,
                    Now = applicant.County
                });
            }
            if (entity.Postcode != applicant.Postcode)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Applicant amended",
                    EventDate = DateTime.Now,
                    RecordChanged = applicant.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Postcode",
                    Was = entity.Postcode,
                    Now = applicant.Postcode
                });
            }
            if (entity.Phone != applicant.Phone)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Applicant amended",
                    EventDate = DateTime.Now,
                    RecordChanged = applicant.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Phone",
                    Was = entity.Phone,
                    Now = applicant.Phone
                });
            }
            if (entity.Salutation != applicant.Salutation)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Applicant amended",
                    EventDate = DateTime.Now,
                    RecordChanged = applicant.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Salutation",
                    Was = entity.Salutation,
                    Now = applicant.Salutation
                });
            }
            if (entity.TipstaffRecordID != applicant.TipstaffRecordID)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Applicant amended",
                    EventDate = DateTime.Now,
                    RecordChanged = applicant.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "TipstaffRecordID",
                    Was = entity.TipstaffRecordID,
                    Now = applicant.TipstaffRecordID
                });
            }
            entity.NameFirst = applicant.NameFirst;
            entity.NameLast = applicant.NameLast;
            entity.AddressLine1 = applicant.AddressLine1;
            entity.AddressLine2 = applicant.AddressLine2;
            entity.AddressLine3 = applicant.AddressLine3;
            entity.Town = applicant.Town;
            entity.County = applicant.County;
            entity.Postcode = applicant.Postcode;
            entity.Phone = applicant.Phone;
            entity.Salutation = applicant.Salutation;
            entity.TipstaffRecordID = applicant.TipstaffRecordID;

            _dynamoAPI.Save(entity);
        }
    }
}
