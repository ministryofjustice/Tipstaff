using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class TemplateRepository : ITemplateRepository
    {
        private readonly IDynamoAPI<Template> _dynamoAPI;
        private readonly IAuditEventRepository _auditRepo;

        public TemplateRepository(IDynamoAPI<Template> dynamoAPI, IAuditEventRepository auditRepo)
        {
            _dynamoAPI = dynamoAPI;
            _auditRepo = auditRepo;
        }

        public void AddTemplate(Template template)
        {
            _dynamoAPI.Save(template);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "Template added",
                EventDate = DateTime.Now,
                RecordChanged = template.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public void Delete(Template template)
        {
            _dynamoAPI.Delete(template);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "Template deleted",
                EventDate = DateTime.Now,
                RecordChanged = template.Id,
                UserId = template.DeactivatedBy
            });
        }

        public IEnumerable<Template> GetAllTemplates()
        {
            return _dynamoAPI.GetAll();
        }

        public Template GetTemplate(string id)
        {
            return _dynamoAPI.GetEntityByKey(id);
        }

        public IEnumerable<Template> GetTemplatesForRecordType(string type)
        {
            string[] obj = new string[2];
            obj[0] = type;
            obj[1] = "All";
            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition("Discriminator", ScanOperator.In, obj),
                    new ScanCondition("Active", ScanOperator.Equal, true)
                });
        }

        public void Update(Template template)
        {
            var entity = _dynamoAPI.GetEntityByKey(template.Id);
            if (entity.Active != template.Active)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Template amended",
                    EventDate = DateTime.Now,
                    RecordChanged = template.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Active",
                    Was = entity.Active.ToString(),
                    Now = template.Active.ToString()
                });
            }
            if (entity.Discriminator != template.Discriminator)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Template amended",
                    EventDate = DateTime.Now,
                    RecordChanged = template.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Discriminator",
                    Was = entity.Discriminator,
                    Now = template.Discriminator
                });
            }
            if (entity.TemplateName != template.TemplateName)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Template amended",
                    EventDate = DateTime.Now,
                    RecordChanged = template.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "TemplateName",
                    Was = entity.TemplateName,
                    Now = template.TemplateName
                });
            }
            if (entity.FilePath != template.FilePath)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Template amended",
                    EventDate = DateTime.Now,
                    RecordChanged = template.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "FilePath",
                    Was = entity.FilePath,
                    Now = template.FilePath
                });
            }
            if (entity.AddresseeRequired != template.AddresseeRequired)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Template amended",
                    EventDate = DateTime.Now,
                    RecordChanged = template.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "AddresseeRequired",
                    Was = entity.AddresseeRequired.ToString(),
                    Now = template.AddresseeRequired.ToString()
                });
            }
            entity.Discriminator = template.Discriminator;
            entity.TemplateName = template.TemplateName;
            entity.FilePath = template.FilePath;
            entity.AddresseeRequired = template.AddresseeRequired;
            entity.Active = template.Active;
            entity.Deactivated = template.Deactivated;
            entity.DeactivatedBy = template.DeactivatedBy;

            _dynamoAPI.Save(entity);
        }
    }
}
