using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using Tipstaff.Infrastructure.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class TipstaffRecordRepository : ITipstaffRecordRepository
    {
        private readonly IDynamoAPI<TipstaffRecord> _dynamoAPI;

        public TipstaffRecordRepository(IDynamoAPI<TipstaffRecord> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
           
        }

        public void Add(TipstaffRecord record)
        {
            _dynamoAPI.Save(record);
        }
        
        public TipstaffRecord GetEntityByObjectKey(object hashKey, object rangeKey)
        {
            return _dynamoAPI.GetEntity(hashKey, rangeKey);
        }

        public TipstaffRecord GetEntityByHashKey(object hashKey)
        {
            return _dynamoAPI.GetEntityByHashKey(hashKey);
        }

        public void Update(TipstaffRecord record)
        {
            var entity = _dynamoAPI.GetEntityByHashKey(record.TipstaffRecordID);
            entity.EldestChild = record.EldestChild;
            entity.ArrestCount = record.ArrestCount;
            entity.ProtectiveMarking = record.ProtectiveMarking;
            entity.Discriminator = record.Discriminator;
            entity.Result = record.Result;
            entity.NextReviewDate = record.NextReviewDate;
            entity.ResultDate = record.ResultDate;
            entity.DateExecuted = record.DateExecuted;
            entity.PrisonCount = record.PrisonCount;
            entity.ResultEnteredBy = record.ResultEnteredBy;
            entity.NPO = record.NPO;
            entity.Division = record.Division;
            entity.CaseStatus = record.CaseStatus;
            entity.SentSCD26 = record.SentSCD26;
            entity.OrderDated = record.OrderDated;
            entity.OrderReceived = record.OrderReceived;
            entity.OfficerDealing = record.OfficerDealing;
            entity.EldestChild = record.EldestChild;
            entity.CAOrderType = record.CAOrderType;
            _dynamoAPI.Save(entity);
        }

        public IEnumerable<TipstaffRecord> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
