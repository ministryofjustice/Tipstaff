using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Models;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class RespondentPresenter : IRespondentPresenter , IMapper<Models.Respondent, Tipstaff.Services.DynamoTables.Respondent>
    {
        private readonly ITipstaffRecordPresenter _tipstaffRecordPresenter;
        private readonly IRespondentRepository _respondentRepository;

        public RespondentPresenter(ITipstaffRecordPresenter tipstaffRecordPresenter, IRespondentRepository respondentRepository)
        {
            _tipstaffRecordPresenter = tipstaffRecordPresenter;
            _respondentRepository = respondentRepository;
        }

        public void Add(Models.Respondent respondent)
        {
            var entity = GetDynamoTable(respondent);
            _respondentRepository.Add(entity);
        }

        public void Delete(Models.Respondent respondent)
        {
            var entity = GetDynamoTable(respondent);
            _respondentRepository.Delete(entity);
        }

        public IEnumerable<Models.Respondent> GetAllById(string id)
        {
            throw new NotImplementedException();
        }

        public Services.DynamoTables.Respondent GetDynamoTable(Models.Respondent model)
        {
            var table = new Services.DynamoTables.Respondent()
            {
                Build = model.build,
                ChildRelationship = model.childRelationship.Detail,
                Country = model.country.Detail,
                DateOfBirth = model.dateOfBirth,
                EyeColour = model.eyeColour,
                HairColour = model.hairColour,
                NameFirst = model.nameFirst,
                NameLast = model.nameLast,
                NameMiddle = model.nameMiddle,
                Height = model.height,
                PNCID = model.PNCID,
                Nationality = model.nationality.Detail,
                Specialfeatures = model.specialfeatures,
                Gender = model.gender.Detail,
                RiskOfDrugs = model.riskOfDrugs,
                RiskOfViolence = model.riskOfViolence,
                SkinColour = model.skinColour.Detail,
                Id = model.respondentID,
                TipstaffRecordID = model.tipstaffRecordID,
             };

            return table;
        }

        public Models.Respondent GetModel(Services.DynamoTables.Respondent table)
        {
            var model = new Models.Respondent()
            {
                build = table.Build,
                childRelationship = MemoryCollections.ChildRelationshipList.GetChildRelationshipList().FirstOrDefault(x => x.Detail == table.ChildRelationship),
                country = MemoryCollections.CountryList.GetCountryByDetail(table.Country),
                dateOfBirth = table.DateOfBirth,
                eyeColour = table.EyeColour,
                gender = MemoryCollections.GenderList.GetGenderByDetail(table.Gender),
                hairColour = table.HairColour,
                height = table.Height,
                nameFirst = table.NameFirst,
                nameLast = table.NameLast,
                nameMiddle = table.NameMiddle,
                nationality = MemoryCollections.NationalityList.GetNationalityByDetail(table.Nationality),
                PNCID = table.PNCID,
                riskOfDrugs = table.RiskOfDrugs,
                riskOfViolence = table.RiskOfViolence,
                specialfeatures = table.Specialfeatures,
                respondentID = table.Id,
                skinColour = MemoryCollections.SkinColourList.GetSkinColourByDetail(table.SkinColour),
                tipstaffRecordID = table.TipstaffRecordID,
                tipstaffRecord = _tipstaffRecordPresenter.GetTipStaffRecord(table.TipstaffRecordID)
             };

            return model;
        }

        public Models.Respondent GetRespondent(string id)
        {
            var respondent = _respondentRepository.GetRespondent(id);

            var model = GetModel(respondent);

            return model;
        }

        public void Update(Models.Respondent respondent)
        {
           
        }
    }
}