﻿using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface ITipstaffRecordRepository
    {
        void Add(TipstaffRecord record);

        void Update(TipstaffRecord record);

        TipstaffRecord GetEntityByObjectKey(object hashKey, object rangeKey);

        TipstaffRecord GetEntityByHashKey(object hashKey);

        IEnumerable<TipstaffRecord> GetAll();

        void Delete(TipstaffRecord record);

        IEnumerable<TipstaffRecord> GetAllByCondition<T>(string name, T value);

        IEnumerable<TipstaffRecord> GetAllByConditions<T>(IDictionary<string, T> conditions);

        IEnumerable<TipstaffRecord> GetAllByConditionsOr<T>(IDictionary<string, T> conditions);

        void PartialUpdate(TipstaffRecord record, string discriminator);
    }
}
