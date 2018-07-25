using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly IDynamoAPI<Address> _dynamoAPI;
        private readonly IAuditEventRepository _auditRepo;

        public AddressRepository(IDynamoAPI<Address> dynamoAPI, IAuditEventRepository auditRepo)
        {
            _dynamoAPI = dynamoAPI;
            _auditRepo = auditRepo;
        }


        public void AddAddress(Address address)
        {
            _dynamoAPI.Save(address);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "Address added",
                EventDate = DateTime.Now,
                RecordChanged = address.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public void DeleteAddress(Address address)
        {
            _dynamoAPI.Delete(address);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "Address deleted",
                EventDate = DateTime.Now,
                RecordChanged = address.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public Address GetAddress(string id)
        {
            return _dynamoAPI.GetEntityByKey(id);
        }
        

        public void UpdateRepository(Address address)
        {
            var entity = _dynamoAPI.GetEntityByKeys(address.Id, address.TipstaffRecordID);
            if (entity.AddresseeName != address.AddresseeName)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Address amended",
                    EventDate = DateTime.Now,
                    RecordChanged = address.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "AddresseeName",
                    Was = entity.AddresseeName,
                    Now = address.AddresseeName
                });
            }
            if (entity.AddressLine1 != address.AddressLine1)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Address amended",
                    EventDate = DateTime.Now,
                    RecordChanged = address.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "AddressLine1",
                    Was = entity.AddressLine1,
                    Now = address.AddressLine1
                });
            }
            if (entity.AddressLine2 != address.AddressLine2)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Address amended",
                    EventDate = DateTime.Now,
                    RecordChanged = address.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "AddressLine2",
                    Was = entity.AddressLine2,
                    Now = address.AddressLine2
                });
            }
            if (entity.AddressLine3 != address.AddressLine3)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Address amended",
                    EventDate = DateTime.Now,
                    RecordChanged = address.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "AddressLine3",
                    Was = entity.AddressLine3,
                    Now = address.AddressLine3
                });
            }
            if (entity.County != address.County)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Address amended",
                    EventDate = DateTime.Now,
                    RecordChanged = address.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "County",
                    Was = entity.County,
                    Now = address.County
                });
            }
            if (entity.Phone != address.Phone)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Address amended",
                    EventDate = DateTime.Now,
                    RecordChanged = address.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Phone",
                    Was = entity.Phone,
                    Now = address.Phone
                });
            }
            if (entity.Town != address.Town)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Address amended",
                    EventDate = DateTime.Now,
                    RecordChanged = address.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Town",
                    Was = entity.Town,
                    Now = address.Town
                });
            }
            if (entity.PostCode != address.PostCode)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Address amended",
                    EventDate = DateTime.Now,
                    RecordChanged = address.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PostCode",
                    Was = entity.PostCode,
                    Now = address.PostCode
                });
            }
            entity.AddresseeName = address.AddresseeName;
            entity.AddressLine1 = address.AddressLine1;
            entity.AddressLine2 = address.AddressLine2;
            entity.AddressLine3 = address.AddressLine3;
            entity.County = address.County;
            entity.Phone = address.Phone;
            entity.PostCode = address.PostCode;
            entity.Town = address.Town;

            _dynamoAPI.Save(entity);
        }

        public IEnumerable<Address> GetAllByCondition<T>(string name, T value)
        {
            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition(name, ScanOperator.Equal, value)
                });
        }

        public Address GetAddressByIDAndRange(string id, string range)
        {
            return _dynamoAPI.GetEntityByKeys(id, range);
        }
    }
}
