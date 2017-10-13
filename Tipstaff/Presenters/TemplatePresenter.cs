using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using Tipstaff.Services.Services;

namespace Tipstaff.Presenters
{
    public class TemplatePresenter : ITemplatePresenter, IMapper<Models.Template, Tipstaff.Services.DynamoTables.Template>, IMapperCollections<Models.Template, Tipstaff.Services.DynamoTables.Template>
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly ITipstaffRecordRepository _tipstaffRepo;

        public TemplatePresenter(ITemplateRepository templateRepo)
        {
            _templateRepository = templateRepo;
        }

        public Models.Template GetTemplate(string id)
        {
            var entity = _templateRepository.GetTemplate(id);
            return GetModel(entity);
        }

        public IEnumerable<Models.Template> GetAllTemplates()
        {
            var entities = _templateRepository.GetAllTemplates();
            return GetAll(entities);
        }

        public void AddTemplate(TemplateEdit model)
        {
            var entity = GetDynamoTable(model.Template);
            _templateRepository.AddTemplate(entity);
        }

        public void UpdateTemplate(TemplateEdit model)
        {
            var entity = GetDynamoTable(model.Template);
            _templateRepository.Update(entity);
        }

        public Models.TipstaffRecord GetTipstaffRecord(string id)
        {
            Services.dto.Tipstaff t = _templateServices.GetTipstaffRecord(id);
            Models.TipstaffRecord tipstaff = new Models.TipstaffRecord()
            {
                tipstaffRecordID = t.TipstaffRecordID,
                createdBy = t.CreatedBy,
                createdOn = t.CreatedOn,
                nextReviewDate = t.NextReviewDate,
                resultDate = t.ResultDate,
                DateExecuted = t.DateExecuted,
                arrestCount = t.ArrestCount,
                prisonCount = t.PrisonCount,
                resultEnteredBy = t.ResultEnteredBy,
                NPO = t.NPO,
                protectiveMarking = MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingByDetail(t.ProtectiveMarking),
                result = MemoryCollections.ResultsList.GetResultByDetail(t.Result),
                caseStatus = MemoryCollections.CaseStatusList.GetCaseStatusByDetail(t.CaseStatus)
            };

            return tipstaff;
        }

        public Models.Applicant GetApplicant(string id)
        {
            Services.dto.Applicant a = _templateServices.GetApplicant(id);
            Models.Applicant applicant = new Models.Applicant() {
                ApplicantID = a.ApplicantID,
                salutation = MemoryCollections.SalutationList.GetSalutationByDetail(a.Salutation),
                nameLast = a.NameLast,
                nameFirst = a.NameFirst,
                addressLine1 = a.AddressLine1,
                addressLine2 = a.AddressLine2,
                addressLine3 = a.AddressLine3,
                town = a.Town,
                county = a.County,
                postcode = a.Postcode,
                phone = a.Phone,
                tipstaffRecordID = a.TipstaffRecordID
            };

            return applicant;
        }

        public Models.Solicitor GetSolicitor(string id)
        {
            throw new NotImplementedException();
        }

        public Models.Template GetModel(Services.DynamoTables.Template table)
        {
            var model = new Models.Template() {
                active = table.Active,
                addresseeRequired = table.AddresseeRequired,
                deactivated = table.Deactivated,
                deactivatedBy = table.DeactivatedBy,
                Discriminator = table.Discriminator,
                filePath = table.FilePath,
                templateID = table.Id,
                templateName = table.TemplateName
            };

            return model;
        }

        public Services.DynamoTables.Template GetDynamoTable(Models.Template model)
        {
            var entity = new Services.DynamoTables.Template() {
                TemplateName=model.templateName,
                Active = model.active,
                AddresseeRequired = model.addresseeRequired,
                Deactivated = model.deactivated,
                DeactivatedBy = model.deactivatedBy,
                Discriminator = model.Discriminator,
                FilePath = model.filePath,
                Id = model.templateID
            };

            return entity;
        }

        public IEnumerable<Models.Template> GetAll(IEnumerable<Services.DynamoTables.Template> entities)
        {
            return entities.Select(x => GetModel(x));
        }

        public IEnumerable<Services.DynamoTables.Template> GetAll(IEnumerable<Models.Template> entities)
        {
            return entities.Select(x => GetDynamoTable(x));
        }
    }
}