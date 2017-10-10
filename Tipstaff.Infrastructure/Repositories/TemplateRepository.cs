﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Infrastructure.Repositories
{
    public class TemplateRepository : ITemplateRepository
    {
        private readonly IDynamoAPI<Template> _dynamoAPI;

        public TemplateRepository(IDynamoAPI<Template> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public void AddTemplate(Template template)
        {
            _dynamoAPI.Save(template);
        }

        public void Delete(Template template)
        {
            _dynamoAPI.Delete(template);
        }

        public IEnumerable<Template> GetAllTemplates()
        {
            return _dynamoAPI.GetAll();
        }

        public Template GetTemplate(string id)
        {
            return _dynamoAPI.GetEntityByHashKey(id);
        }

        public void Update(Template template)
        {
            var entity = _dynamoAPI.GetEntityByHashKey(template.TemplateID);
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
