using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
