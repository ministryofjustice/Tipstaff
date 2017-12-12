using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

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

        public IEnumerable<DeletedTipstaffRecord> GetAll()
        {
            return _dynamoAPI.GetAll();
        }

        public void Remove(DeletedTipstaffRecord record)
        {
            _dynamoAPI.Delete(record);
        }
    }
}
