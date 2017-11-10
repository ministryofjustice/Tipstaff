using System;
using System.Collections.Generic;
using System.Linq;
using Tipstaff.Mappers;
using Tipstaff.Presenters.Interfaces;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class DeletedTipstaffRecordPresenter : IDeletedTipstaffRecordPresenter, IMapper<Models.DeletedTipstaffRecord, Tipstaff.Services.DynamoTables.DeletedTipstaffRecord>
    {
        private readonly IDeletedTipstaffRecordRepository _deletedTipstaffRecordRepository;

        public DeletedTipstaffRecordPresenter(IDeletedTipstaffRecordRepository deletedTipstaffRecordRepository)
        {
            _deletedTipstaffRecordRepository = deletedTipstaffRecordRepository;
        }
        
        public IEnumerable<Models.DeletedTipstaffRecord> GetAll()
        {
            return _deletedTipstaffRecordRepository.GetAll().Select(x => GetModel(x));
        }

        public Services.DynamoTables.DeletedTipstaffRecord GetDynamoTable(Models.DeletedTipstaffRecord model)
        {
            throw new NotImplementedException();
        }

        public Models.DeletedTipstaffRecord GetModel(Services.DynamoTables.DeletedTipstaffRecord table)
        {
            var model = new Models.DeletedTipstaffRecord()
            {
                discriminator = table.Discriminator,
                TipstaffRecordID = table.Id,
                UniqueRecordID = table.UniqueRecordID,
                deletedReason = MemoryCollections.DeletedReasonList.GetDeletedReasonList().FirstOrDefault(x=>x.Detail == table.DeletedReason)
            };

            return model;
        }
    }
}