using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using System.Linq;
using TPLibrary.DynamoAPI;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;

namespace Tipstaff.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IDynamoAPI<User> _dynamoAPI;
        private readonly IAuditEventRepository _auditRepo;

        public UsersRepository(IDynamoAPI<User> dynamoAPI, IAuditEventRepository auditRepo)
        {
            _dynamoAPI = dynamoAPI;
            _auditRepo = auditRepo;
        }

        public void Add(User user)
        {
            _dynamoAPI.Save(user);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "User added",
                EventDate = DateTime.Now,
                RecordChanged = user.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public IEnumerable<User> GetAll()
        {
            return _dynamoAPI.GetAll();
        }

        public User GetUserByID(string id)
        {
            return _dynamoAPI.GetEntityByKey(id);
        }

        public User GetUserByLoginName(string name)
        {
            var users =  _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition("Name", ScanOperator.Equal, name)
                });
            return users.FirstOrDefault();
        }

        public void Update(User user)
        {
            var entity = _dynamoAPI.GetEntityByKey(user.Id);
            if (entity.DisplayName != user.DisplayName)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "User amended",
                    EventDate = DateTime.Now,
                    RecordChanged = user.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "DisplayName",
                    Was = entity.DisplayName,
                    Now = user.DisplayName
                });
            }
            if (entity.LastActive != user.LastActive)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "User amended",
                    EventDate = DateTime.Now,
                    RecordChanged = user.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "LastActive",
                    Was = entity.LastActive.ToString(),
                    Now = user.LastActive.ToString()
                });
            }
            if (entity.Name != user.Name)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "User amended",
                    EventDate = DateTime.Now,
                    RecordChanged = user.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Name",
                    Was = entity.Name,
                    Now = user.Name
                });
            }
            if (entity.Role != user.Role)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "User amended",
                    EventDate = DateTime.Now,
                    RecordChanged = user.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Role",
                    Was = entity.Role,
                    Now = user.Role
                });
            }
            if (entity.RoleStrength != user.RoleStrength)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "User amended",
                    EventDate = DateTime.Now,
                    RecordChanged = user.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "RoleStrength",
                    Was = entity.RoleStrength.ToString(),
                    Now = user.RoleStrength.ToString()
                });
            }
            entity.DisplayName = user.DisplayName;
            entity.LastActive = user.LastActive;
            entity.Name = user.Name;
            entity.Role = user.Role;
            entity.RoleStrength = user.RoleStrength;
            
            _dynamoAPI.Save(entity);
        }
    }
}
