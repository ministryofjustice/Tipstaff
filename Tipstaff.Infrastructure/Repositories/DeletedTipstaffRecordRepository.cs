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
    public class DeletedTipstaffRecordRepository : IDeletedTipstaffRecordRepository
    {
        private readonly IDynamoAPI<DeletedTipstaffRecord> _dynamoAPI;

        public DeletedTipstaffRecordRepository(IDynamoAPI<DeletedTipstaffRecord> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public void Add(DeletedTipstaffRecord record)
        {
            _dynamoAPI.Save(record);
        }

        public void Remove(DeletedTipstaffRecord record)
        {
            _dynamoAPI.Delete(record);
        }
    }
}
