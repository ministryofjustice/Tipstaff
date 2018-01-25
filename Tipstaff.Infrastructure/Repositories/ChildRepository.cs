using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class ChildRepository : IChildRepository
    {
        private readonly IDynamoAPI<Child> _dynamoAPI;

        public ChildRepository(IDynamoAPI<Child> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public void AddChild(Child child)
        {
            _dynamoAPI.Save(child);
        }

        public void Delete(Child child)
        {
            _dynamoAPI.Delete(child);
        }

        public IEnumerable<Child> GetAllChildren()
        {
            return _dynamoAPI.GetAll();
        }

        public IEnumerable<Child> GetAllChildrenByTipstaffRecordID(string id)
        {
            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition("TipstaffRecordID", ScanOperator.Equal, id)
                });
        }

        public Child GetChild(string id)
        {
            return _dynamoAPI.GetEntityByKey(id);
        }

        public Child GetChildByIdAndRange(string id, string range)
        {
            return _dynamoAPI.GetEntityByKeys(id, range);
        }

        public void Update(Child child)
        {
            var entity = _dynamoAPI.GetEntityByKeys(child.Id, child.TipstaffRecordID);
            entity.NameFirst = child.NameFirst;
            entity.NameLast = child.NameLast;
            entity.NameMiddle = child.NameMiddle;
            entity.DateOfBirth = child.DateOfBirth;
            entity.Gender = child.Gender;
            entity.Height = child.Height;
            entity.Build = child.Build;
            entity.HairColour = child.HairColour;
            entity.EyeColour = child.EyeColour;
            entity.SkinColour = child.SkinColour;
            entity.Specialfeatures = child.Specialfeatures;
            entity.Country = child.Country;
            entity.Nationality = child.Nationality;
            entity.PNCID = child.PNCID;
            //entity.TipstaffRecordID = child.TipstaffRecordID;
            _dynamoAPI.Save(entity);
        }
    }
}
