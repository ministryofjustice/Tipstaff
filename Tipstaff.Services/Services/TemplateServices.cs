using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.dto;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Services.Services
{
    public class TemplateServices : ITemplateServices
    {
        public readonly ITemplateRepository _templateRepo;
        public readonly ITipstaffRecordRepository _tipstaffRepo;
        public readonly IApplicantRepository _appRepo;

        public TemplateServices(ITemplateRepository tr, ITipstaffRecordRepository tipstaffRepo, IApplicantRepository appRepo)
        {
            _templateRepo = tr;
            _tipstaffRepo = tipstaffRepo;
            _appRepo = appRepo;
        }
        public Template GetTemplate(string id)
        {
            DynamoTables.Template t = _templateRepo.GetTemplate(id);
            Template template = new Template() {
                TemplateID = t.templateID,
                Discriminator = t.Discriminator,
                TemplateName = t.templateName,
                FilePath = t.filePath,
                AddresseeRequired = t.addresseeRequired,
                Active = t.active,
                Deactivated = t.deactivated,
                DeactivatedBy = t.deactivatedBy
            };

            return template;
        }

        public List<Template> GetAllTemplates()
        {
            IEnumerable<DynamoTables.Template> ts = _templateRepo.GetAllTemplates();
            List<Template> templates = new List<Template>();
            foreach (DynamoTables.Template t in ts)
            {
                Template template = new Template()
                {
                    TemplateID = t.templateID,
                    Discriminator = t.Discriminator,
                    TemplateName = t.templateName,
                    FilePath = t.filePath,
                    AddresseeRequired = t.addresseeRequired,
                    Active = t.active,
                    Deactivated = t.deactivated,
                    DeactivatedBy = t.deactivatedBy
                };
                templates.Add(template);
            }
            return templates;
        }

        public void AddTemplate(Template t)
        {
            _templateRepo.AddTemplate(new DynamoTables.Template()
            {
                templateID = t.TemplateID,
                Discriminator = t.Discriminator,
                templateName = t.TemplateName,
                filePath = t.FilePath,
                addresseeRequired = t.AddresseeRequired,
                active = t.Active,
                deactivated = t.Deactivated,
                deactivatedBy = t.DeactivatedBy
            });
        }

        public void UpdateTemplate(Template t)
        {
            _templateRepo.Update(new DynamoTables.Template()
            {
                templateID = t.TemplateID,
                Discriminator = t.Discriminator,
                templateName = t.TemplateName,
                filePath = t.FilePath,
                addresseeRequired = t.AddresseeRequired,
                active = t.Active,
                deactivated = t.Deactivated,
                deactivatedBy = t.DeactivatedBy
            });
        }

        public dto.Tipstaff GetTipstaffRecord(string id)
        {
            DynamoTables.TipstaffRecord tr = _tipstaffRepo.GetEntityByHashKey(id);
            dto.Tipstaff t = new dto.Tipstaff()
            {
                TipstaffRecordID = tr.TipstaffRecordID,
                CreatedBy = tr.CreatedBy,
                CreatedOn = tr.CreatedOn,
                NextReviewDate = tr.NextReviewDate,
                ResultDate = tr.ResultDate,
                DateExecuted = tr.DateExecuted,
                ArrestCount = tr.ArrestCount,
                PrisonCount = tr.PrisonCount,
                ResultEnteredBy = tr.ResultEnteredBy,
                NPO = tr.NPO,
                ProtectiveMarking = tr.ProtectiveMarking,
                Result = tr.Result,
                CaseStatus = tr.CaseStatus
            };

            return t;
        }

        public Applicant GetApplicant(string id)
        {
            DynamoTables.Applicant a = _appRepo.GetApplicant(id);
            Applicant applicant = new Applicant() {
                ApplicantID = a.ApplicantID,
                Salutation = a.Salutation,
                NameLast = a.NameLast,
                NameFirst = a.NameFirst,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                AddressLine3 = a.AddressLine3,
                Town = a.Town,
                County =a.County,
                Postcode = a.Postcode,
                Phone = a.Phone,
                TipstaffRecordID = a.TipstaffRecordID
            };
            return applicant;
        }
    }
}
