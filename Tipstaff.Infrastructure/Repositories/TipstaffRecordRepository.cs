using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

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
            return _dynamoAPI.GetEntityByKeys(hashKey, rangeKey);
        }

        public TipstaffRecord GetEntityByHashKey(object hashKey)
        {
            return _dynamoAPI.GetEntityByKey(hashKey);
        }

        public void Update(TipstaffRecord record)
        {
            var entity = _dynamoAPI.GetEntityByKey(record.Id);
            entity.EldestChild = record.EldestChild;
            entity.ArrestCount = record.ArrestCount;
            entity.ProtectiveMarkingId = record.ProtectiveMarkingId;
            entity.Discriminator = record.Discriminator;
            entity.ResultId = record.ResultId;
            entity.NextReviewDate = record.NextReviewDate;
            entity.ResultDate = record.ResultDate;
            entity.DateExecuted = record.DateExecuted;
            entity.PrisonCount = record.PrisonCount;
            entity.ResultEnteredBy = record.ResultEnteredBy;
            entity.NPO = record.NPO;
            entity.DivisionId = record.DivisionId;
            entity.CaseStatusId = record.CaseStatusId;
            entity.SentSCD26 = record.SentSCD26;
            entity.OrderDated = record.OrderDated;
            entity.OrderReceived = record.OrderReceived;
            entity.OfficerDealing = record.OfficerDealing;
            entity.ExpiryDate = record.ExpiryDate;
            entity.CAOrderTypeId = record.CAOrderTypeId;
            entity.CreatedOn = record.CreatedOn;
            entity.CreatedBy = record.CreatedBy;
            entity.DateCirculated = record.DateCirculated;
            entity.RespondentName = entity.RespondentName;
            entity.CaseNumber = entity.CaseNumber;
            entity.DivisionId = entity.DivisionId;
            _dynamoAPI.Save(entity);
        }

        public IEnumerable<TipstaffRecord> GetAll()
        {
            return _dynamoAPI.GetAll();
        }

        public IEnumerable<TipstaffRecord> GetAllByCondition<T>(string name, T value)
        {
            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition(name, ScanOperator.Equal, value)
                }
            );
        }

        public void Delete(TipstaffRecord record)
        {
            _dynamoAPI.Delete(record);
        }
    }
}
