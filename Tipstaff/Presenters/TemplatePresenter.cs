using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Models;
using Tipstaff.Services.Services;

namespace Tipstaff.Presenters
{
    public class TemplatePresenter : ITemplatePresenter
    {
        private readonly ITemplateServices _templateServices;

        public TemplatePresenter(ITemplateServices templateServices)
        {
            _templateServices = templateServices;
        }

        public Template GetTemplate(string id)
        {
            Services.dto.Template t = _templateServices.GetTemplate(id);
            Template template = new Template()
            {
                templateID = t.TemplateID,
                Discriminator = t.Discriminator,
                templateName = t.TemplateName,
                filePath = t.FilePath,
                addresseeRequired = t.AddresseeRequired,
                active = t.Active,
                deactivated = t.Deactivated,
                deactivatedBy = t.DeactivatedBy
            };

            return template;
        }

        public IEnumerable<Template> GetAllTemplates()
        {
            IEnumerable<Services.dto.Template> ts = _templateServices.GetAllTemplates();
            List<Template> templates = new List<Template>();
            foreach (Services.dto.Template t in ts)
            {
                Template template = new Template()
                {
                    templateID = t.TemplateID,
                    Discriminator = t.Discriminator,
                    templateName = t.TemplateName,
                    filePath = t.FilePath,
                    addresseeRequired = t.AddresseeRequired,
                    active = t.Active,
                    deactivated = t.Deactivated,
                    deactivatedBy = t.DeactivatedBy
                };
                templates.Add(template);
            }
            return templates;
        }

        public void AddTemplate(TemplateEdit model)
        {
            _templateServices.AddTemplate(new Services.dto.Template() {
                TemplateID = model.Template.templateID,
                Discriminator = model.Template.Discriminator,
                TemplateName = model.Template.templateName,
                FilePath = model.Template.filePath,
                AddresseeRequired = model.Template.addresseeRequired,
                Active = model.Template.active,
                Deactivated = model.Template.deactivated,
                DeactivatedBy = model.Template.deactivatedBy
            });
        }

        public void UpdateTemplate(TemplateEdit model)
        {
            _templateServices.UpdateTemplate(new Services.dto.Template()
            {
                TemplateID = model.Template.templateID,
                Discriminator = model.Template.Discriminator,
                TemplateName = model.Template.templateName,
                FilePath = model.Template.filePath,
                AddresseeRequired = model.Template.addresseeRequired,
                Active = model.Template.active,
                Deactivated = model.Template.deactivated,
                DeactivatedBy = model.Template.deactivatedBy
            });
        }

        public TipstaffRecord GetTipstaffRecord(string id)
        {
            Services.dto.Tipstaff t = _templateServices.GetTipstaffRecord(id);
            TipstaffRecord tipstaff = new TipstaffRecord()
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

        public Applicant GetApplicant(string id)
        {
            Services.dto.Applicant a = _templateServices.GetApplicant(id);
            Applicant applicant = new Applicant() {
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

        public Solicitor GetSolicitor(string id)
        {
            throw new NotImplementedException();
        }
        
        

        
    }
}