using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class TipstaffPoliceForcesRepository : ITipstaffPoliceForcesRepository
    {
        private readonly IDynamoAPI<Tipstaff_PoliceForces> _dynamoAPI;

        public TipstaffPoliceForcesRepository(IDynamoAPI<Tipstaff_PoliceForces> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }
        public void Add(Tipstaff_PoliceForces tpf)
        {
            _dynamoAPI.Save(tpf);
        }

        public void Delete(Tipstaff_PoliceForces tpf)
        {
            _dynamoAPI.Delete(tpf);
        }

        public Tipstaff_PoliceForces GetTipstaffPoliceForces(string id)
        {
            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition("Id", ScanOperator.Equal, id)
                }).FirstOrDefault();
        }

        public IEnumerable<Tipstaff_PoliceForces> GetTipstaffPoliceForcesByTipstaffRecordID(string id)
        {
            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition("TipstaffRecordID", ScanOperator.Equal, id)
                });
        }

        public void Update(Tipstaff_PoliceForces tpf)
        {
            var entity = _dynamoAPI.GetEntityByKeys(tpf.Id, tpf.TipstaffRecordID);

            entity.PoliceForceID = tpf.PoliceForceID;

            _dynamoAPI.Save(entity);
        }
    }
}
