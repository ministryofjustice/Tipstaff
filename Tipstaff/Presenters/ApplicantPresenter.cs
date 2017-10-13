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
    public class ApplicantPresenter : IApplicantPresenter, IMapper<Models.Applicant, Tipstaff.Services.DynamoTables.Applicant>, IMapperCollections<Models.Applicant, Tipstaff.Services.DynamoTables.Applicant>
    {
        private readonly IApplicantRepository _appRepository;
        private readonly ITipstaffRecordPresenter _tipstaffPresenter;

        public ApplicantPresenter(IApplicantRepository appRepo, ITipstaffRecordPresenter tipstaffPresenter)
        {
            _appRepository = appRepo;
            _tipstaffPresenter = tipstaffPresenter;
        }

        public Models.Applicant GetApplicant(string id)
        {
            var applicant = _appRepository.GetApplicant(id);
            return GetModel(applicant);
        }

        public IEnumerable<Models.Applicant> GetAllApplicantsByTipstaffRecordID(string id)
        {
            var applicants = _appRepository.GetAllApplicantsByTipstaffRecordID(id);
            return GetAll(applicants);
        }

        public void AddApplicant(ApplicantCreationModel model)
        {
            var entity = GetDynamoTable(model.applicant);
            _appRepository.AddApplicant(entity);
        }

        public void UpdateApplicant(ApplicantEditModel model)
        {
            var entity = GetDynamoTable(model.applicant);
            _appRepository.Update(entity);
        }

        public void DeleteApplicant(DeleteApplicant model)
        {
            var entity = GetDynamoTable(model.Applicant);
            _appRepository.Delete(entity);
        }

        public Models.TipstaffRecord GetTipstaffRecord(string id)
        {
            var tipstaff = _tipstaffPresenter.GetTipStaffRecord(id);

            return tipstaff;
        }

        public Models.Applicant GetModel(Services.DynamoTables.Applicant table)
        {
            var model = new Models.Applicant()
            {
                addressLine1 = table.AddressLine1,
                addressLine2 = table.AddressLine2,
                addressLine3 = table.AddressLine3,
                ApplicantID = table.Id,
                childAbduction = (ChildAbduction) _tipstaffPresenter.GetTipStaffRecord(table.TipstaffRecordID),
                county = table.County,
                nameFirst = table.NameFirst,
                nameLast = table.NameLast,
                phone = table.Phone,
                postcode = table.Postcode,
                salutation = MemoryCollections.SalutationList.GetSalutationByDetail(table.Salutation),
                tipstaffRecordID = table.TipstaffRecordID,
                town = table.Town

            };

            return model;
        }

        public Services.DynamoTables.Applicant GetDynamoTable(Models.Applicant model)
        {
            var entity = new Services.DynamoTables.Applicant() {
                Town = model.town,
                AddressLine1 = model.addressLine1,
                AddressLine2 = model.addressLine2,
                AddressLine3 = model.addressLine3,
                County = model.county,
                Id = model.ApplicantID,
                NameFirst = model.nameFirst,
                NameLast = model.nameLast,
                Phone = model.phone,
                Postcode = model.postcode,
                Salutation = model.salutation.Detail,
                TipstaffRecordID = model.tipstaffRecordID
            };

            return entity;
        }

        public IEnumerable<Models.Applicant> GetAll(IEnumerable<Services.DynamoTables.Applicant> entities)
        {
            return entities.Select(x => GetModel(x));
        }

        public IEnumerable<Services.DynamoTables.Applicant> GetAll(IEnumerable<Models.Applicant> entities)
        {
            return entities.Select(x => GetDynamoTable(x));
        }

    }
}