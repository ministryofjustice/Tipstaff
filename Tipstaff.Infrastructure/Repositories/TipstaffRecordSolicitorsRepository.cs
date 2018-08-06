using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class TipstaffRecordSolicitorsRepository : ITipstaffRecordSolicitorsRepository
    {
        private readonly IDynamoAPI<Tipstaff_Solicitors> _dynamoAPI;

        public TipstaffRecordSolicitorsRepository(IDynamoAPI<Tipstaff_Solicitors> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public void AddRecord(Tipstaff_Solicitors record)
        {
            _dynamoAPI.Save(record);
        }

        public IEnumerable<Tipstaff_Solicitors> GetAllByCondition<T>(string name, T value)
        {
            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition(name, ScanOperator.Equal, value)
                });
        }
    }
}
