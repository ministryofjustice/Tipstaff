using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Infrastructure.Repositories
{
    public class RespondentRepository : IRespondentRepository
    {
        private readonly IDynamoAPI<Respondent> _dynamoAPI;
        
        public RespondentRepository(IDynamoAPI<Respondent> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }
        
        public void Add(Respondent respondent)
        {
            _dynamoAPI.Save(respondent);
        }

        public void Delete(Respondent respondent)
        {
            _dynamoAPI.Delete(respondent);
        }

        public IEnumerable<Respondent> GetAll()
        {
            return _dynamoAPI.GetAll();
        }

        public Respondent GetRespondent(string id)
        {
            return _dynamoAPI.GetEntityByHashKey(id);
        }

        public void Update(Respondent respondent)
        {
            var entity = _dynamoAPI.GetEntityByHashKey(respondent.Id);

            entity.Build = respondent.Build;
            entity.ChildRelationship = respondent.ChildRelationship;
            entity.Country = respondent.Country;
            entity.DateOfBirth = respondent.DateOfBirth;
            entity.EyeColour = respondent.EyeColour;
            entity.Gender = respondent.Gender;
            entity.HairColour = respondent.HairColour;
            entity.Height = respondent.Height;
            entity.Id = respondent.Id;
            entity.NameFirst = respondent.NameFirst;
            entity.NameLast = respondent.NameLast;
            entity.NameMiddle = respondent.NameMiddle;
            entity.Nationality = respondent.Nationality;
            entity.PNCID = respondent.PNCID;
            entity.RiskOfDrugs = respondent.RiskOfDrugs;
            entity.RiskOfViolence = respondent.RiskOfViolence;
            entity.SkinColour = respondent.SkinColour;
            entity.Specialfeatures = respondent.Specialfeatures;
            entity.TipstaffRecordID = respondent.TipstaffRecordID;
            
            _dynamoAPI.Save(entity);
        }
    }
}
