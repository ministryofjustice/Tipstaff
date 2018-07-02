using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;
using System.Linq;

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
        
        public Respondent GetRespondent(string id)
        {

            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition("Id", ScanOperator.Equal, id)
                }).FirstOrDefault();

        }

        public IEnumerable<Respondent> GetAllRespondentsByTipstaffRecordID(string id)
        {
            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition("TipstaffRecordID", ScanOperator.Equal, id)
                });
        }

        public Respondent GetRespondentByKeys(string id, string rangeKey)
        {
            return _dynamoAPI.GetEntityByKeys(id,rangeKey);
            //return _dynamoAPI.GetResultsByConditions(
            //    new ScanCondition[]
            //    {
            //        new ScanCondition("Id", ScanOperator.Equal, id)
            //    }).FirstOrDefault();

        }


        public void Update(Respondent respondent)
        {
            //var entity = _dynamoAPI.GetEntityByKey(respondent.Id);
            var entity = _dynamoAPI.GetEntityByKeys(respondent.Id, respondent.TipstaffRecordID);

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
