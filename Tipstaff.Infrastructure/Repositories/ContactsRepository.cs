using System;
using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class ContactsRepository : IContactsRepository
    {
        private readonly IDynamoAPI<Contact> _dynamoAPI;
        private readonly IAuditEventRepository _auditRepo;

        public ContactsRepository(IDynamoAPI<Contact> dynamoAPI, IAuditEventRepository auditRepo)
        {
            _dynamoAPI = dynamoAPI;
            _auditRepo = auditRepo;
        }

        public void AddContact(Contact contact)
        {
            _dynamoAPI.Save(contact);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "Contact added",
                EventDate = DateTime.Now,
                RecordChanged = contact.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }
        
        public Contact GetContact(string id)
        {
            return _dynamoAPI.GetEntityByKey(id);
        }
        
        public IEnumerable<Contact> GetContacts()
        {
            return _dynamoAPI.GetAll();
        }

        public void UpdateContact(Contact contact)
        {
            var entity = _dynamoAPI.GetEntityByKey(contact.Id);
            if (entity.SalutationId != contact.SalutationId)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Contact amended",
                    EventDate = DateTime.Now,
                    RecordChanged = contact.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "SalutationId",
                    Was = entity.SalutationId.ToString(),
                    Now = contact.SalutationId.ToString()
                });
            }
            if (entity.FirstName != contact.FirstName)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Contact amended",
                    EventDate = DateTime.Now,
                    RecordChanged = contact.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "FirstName",
                    Was = entity.FirstName,
                    Now = contact.FirstName
                });
            }
            if (entity.LastName != contact.LastName)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Contact amended",
                    EventDate = DateTime.Now,
                    RecordChanged = contact.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "LastName",
                    Was = entity.LastName,
                    Now = contact.LastName
                });
            }
            if (entity.AddressLine1 != contact.AddressLine1)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Contact amended",
                    EventDate = DateTime.Now,
                    RecordChanged = contact.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "AddressLine1",
                    Was = entity.AddressLine1,
                    Now = contact.AddressLine1
                });
            }
            if (entity.AddressLine2 != contact.AddressLine2)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Contact amended",
                    EventDate = DateTime.Now,
                    RecordChanged = contact.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "AddressLine2",
                    Was = entity.AddressLine2,
                    Now = contact.AddressLine2
                });
            }
            if (entity.AddressLine3 != contact.AddressLine3)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Contact amended",
                    EventDate = DateTime.Now,
                    RecordChanged = contact.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "AddressLine3",
                    Was = entity.AddressLine3,
                    Now = contact.AddressLine3
                });
            }
            if (entity.Town != contact.Town)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Contact amended",
                    EventDate = DateTime.Now,
                    RecordChanged = contact.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Town",
                    Was = entity.Town,
                    Now = contact.Town
                });
            }
            if (entity.County != contact.County)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Contact amended",
                    EventDate = DateTime.Now,
                    RecordChanged = contact.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "County",
                    Was = entity.County,
                    Now = contact.County
                });
            }
            if (entity.Postcode != contact.Postcode)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Contact amended",
                    EventDate = DateTime.Now,
                    RecordChanged = contact.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Postcode",
                    Was = entity.Postcode,
                    Now = contact.Postcode
                });
            }
            if (entity.DX != contact.DX)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Contact amended",
                    EventDate = DateTime.Now,
                    RecordChanged = contact.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "DX",
                    Was = entity.DX,
                    Now = contact.DX
                });
            }
            if (entity.PhoneHome != contact.PhoneHome)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Contact amended",
                    EventDate = DateTime.Now,
                    RecordChanged = contact.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PhoneHome",
                    Was = entity.PhoneHome,
                    Now = contact.PhoneHome
                });
            }
            if (entity.PhoneMobile != contact.PhoneMobile)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Contact amended",
                    EventDate = DateTime.Now,
                    RecordChanged = contact.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PhoneMobile",
                    Was = entity.PhoneMobile,
                    Now = contact.PhoneMobile
                });
            }
            if (entity.Notes != contact.Notes)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Contact amended",
                    EventDate = DateTime.Now,
                    RecordChanged = contact.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Notes",
                    Was = entity.Notes,
                    Now = contact.Notes
                });
            }
            if (entity.Email != contact.Email)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Contact amended",
                    EventDate = DateTime.Now,
                    RecordChanged = contact.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Email",
                    Was = entity.Email,
                    Now = contact.Email
                });
            }
            if (entity.ContactType != contact.ContactType)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Contact amended",
                    EventDate = DateTime.Now,
                    RecordChanged = contact.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "ContactType",
                    Was = entity.ContactType,
                    Now = contact.ContactType
                });
            }
            entity.SalutationId = contact.SalutationId;
            entity.FirstName = contact.FirstName;
            entity.LastName = contact.LastName;
            entity.AddressLine1 = contact.AddressLine1;
            entity.AddressLine2 = contact.AddressLine2;
            entity.AddressLine3 = contact.AddressLine3;
            entity.Town = contact.Town;
            entity.County = contact.County;
            entity.Postcode = contact.Postcode;
            entity.DX = contact.DX;
            entity.PhoneHome = contact.PhoneHome;
            entity.PhoneMobile = contact.PhoneMobile;
            entity.Email = contact.Email;
            entity.Notes = contact.Notes;
            entity.ContactType = contact.ContactType;
            _dynamoAPI.Save(entity);
        }

        public void Delete(Contact contact)
        {
            _dynamoAPI.Delete(contact);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "Contact deleted",
                EventDate = DateTime.Now,
                RecordChanged = contact.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }
    }
}
