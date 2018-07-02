using System;
using System.Collections.Generic;
using System.Linq;
using Tipstaff.Mappers;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class RespondentPresenter : IRespondentPresenter , IMapper<Models.Respondent, Tipstaff.Services.DynamoTables.Respondent>
    {
        //private readonly ITipstaffRecordPresenter _tipstaffRecordPresenter;
        private readonly IRespondentRepository _respondentRepository;

        public RespondentPresenter(IRespondentRepository respondentRepository)
        {
            
            _respondentRepository = respondentRepository;
        }

        public void Add(Models.Respondent respondent)
        {
            try
            {
                var entity = GetDynamoTable(respondent);
                _respondentRepository.Add(entity);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public void Delete(Models.Respondent respondent)
        {
            var entity = GetDynamoTable(respondent);
            _respondentRepository.Delete(entity);
        }
        
        public IEnumerable<Models.Respondent> GetAllById(string id)
        {
            var respondents = _respondentRepository.GetAllRespondentsByTipstaffRecordID(id);
            return respondents.Select(x => GetModel(x));
        }

        public Services.DynamoTables.Respondent GetDynamoTable(Models.Respondent model)
        {
            var table = new Services.DynamoTables.Respondent()
            {
                Build = model.build,
                ChildRelationship = MemoryCollections.ChildRelationshipList.GetChildRelationshipList().FirstOrDefault(c => c.ChildRelationshipID == model.childRelationship.ChildRelationshipID).Detail,
                Country = MemoryCollections.CountryList.GetCountryByID(model.country.CountryID).Detail,
                DateOfBirth = model.dateOfBirth,
                EyeColour = model.eyeColour,
                HairColour = model.hairColour,
                NameFirst = model.nameFirst,
                NameLast = model.nameLast,
                NameMiddle = model.nameMiddle,
                Height = model.height,
                PNCID = model.PNCID,
                Nationality = MemoryCollections.NationalityList.GetNationalityByID(model.nationality.NationalityID).Detail,
                Specialfeatures = model.specialfeatures,
                Gender = MemoryCollections.GenderList.GetGenderById(model.gender.GenderId).Detail,
                RiskOfDrugs = model.riskOfDrugs,
                RiskOfViolence = model.riskOfViolence,
                SkinColour = MemoryCollections.SkinColourList.GetSkinColourById(model.skinColour.SkinColourId).Detail,
                Id = model.respondentID, //Guid.NewGuid().ToString(),
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
                
                //tipstaffRecord = _tipstaffRecordPresenter.GetTipStaffRecord(table.TipstaffRecordID)
             };

            return model;
        }

        public Models.Respondent GetRespondent(string id)
        {
            var respondent = _respondentRepository.GetRespondent(id);

            var model = GetModel(respondent);

            return model;
        }

        public Models.Respondent GetRespondentByKeys(string id, string rangeKey)
        {
            var respondent = _respondentRepository.GetRespondentByKeys(id, rangeKey);

            var model = GetModel(respondent);

            return model;
        }

        public void Update(Models.Respondent respondent)
        {
            var resp = GetDynamoTable(respondent);
            _respondentRepository.Update(resp);
        }
    }
}