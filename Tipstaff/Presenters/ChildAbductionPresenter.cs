using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class ChildAbductionPresenter : IChildAbductionPresenter, IMapper<ChildAbduction, Services.DynamoTables.TipstaffRecord>
    {
        private readonly ITipstaffRecordRepository _tipstaffRecordRepository;
        private readonly IDeletedTipstaffRecordRepository _deletedTipstaffRecordRepository;
        private Object _lock = new Object();

        public ChildAbductionPresenter(ITipstaffRecordRepository tipstaffRecordRepository, IDeletedTipstaffRecordRepository deletedTipstaffRecordRepository)
        {
            _tipstaffRecordRepository = tipstaffRecordRepository;
            _deletedTipstaffRecordRepository = deletedTipstaffRecordRepository;
        }

        public void AddDeletedTipstaffRecord(Models.DeletedTipstaffRecord record)
        {
            var entity = new Services.DynamoTables.DeletedTipstaffRecord()
            {
                Id = record.TipstaffRecordID,
                DeletedReason = MemoryCollections.DeletedReasonList.GetDeletedReasonList().FirstOrDefault(x=>x.Detail == record.deletedReason.Detail).Detail,
                UniqueRecordID = record.UniqueRecordID,
                Discriminator = record.discriminator
            };

            _deletedTipstaffRecordRepository.Add(entity);
        }

        public void AddTipstaffRecord(ChildAbduction childabduction)
        {
            lock (_lock)
            {
                var entity = GetDynamoTable(childabduction);

                var count = _tipstaffRecordRepository.GetAll().Count();

                entity.Id = $"{count++}";
                
                _tipstaffRecordRepository.Add(entity);
            }
        }

        public void DeletedTipstaffRecords(Models.DeletedTipstaffRecord record)
        {
            var entity = new Services.DynamoTables.DeletedTipstaffRecord()
            {
                Id = record.TipstaffRecordID,
                DeletedReason = MemoryCollections.DeletedReasonList.GetDeletedReasonList().FirstOrDefault(x => x.Detail == record.deletedReason.Detail).Detail,
                UniqueRecordID = record.UniqueRecordID,
                Discriminator = record.discriminator
            };

            _deletedTipstaffRecordRepository.Remove(entity);
        }

        public IEnumerable<ChildAbduction> GetAllChildAbductions()
        {
            var records = _tipstaffRecordRepository.GetAll();

            var childAbductions = records.Where(x => x.Discriminator == "ChildAbduction").Select(x=> GetModel(x));

            return childAbductions;
        }

        public ChildAbduction GetChildAbduction(string id)
        {
            var record = _tipstaffRecordRepository.GetEntityByHashKey(id);

            var childAbduction = GetModel(record);

            return childAbduction;
        }

        public Services.DynamoTables.TipstaffRecord GetDynamoTable(ChildAbduction model)
        {
            var record = new Services.DynamoTables.TipstaffRecord()
            {
                Id = model.tipstaffRecordID,
                SentSCD26 = model.sentSCD26,
                OfficerDealing = model.officerDealing,
                OrderDated = model.orderDated,
                OrderReceived = model.orderReceived,
                EldestChild = model.EldestChild,
                CAOrderType = model.caOrderType.Detail
            };

            return record;
        }
        
        public Models.ChildAbduction GetModel(Services.DynamoTables.TipstaffRecord table)
        {
            var entity = _tipstaffRecordRepository.GetEntityByHashKey(table.Id);

            var model = new Models.ChildAbduction()
            {
                sentSCD26 = table.SentSCD26,
                Descriminator = table.Discriminator,
                orderDated = table.OrderDated,
                orderReceived = table.OrderReceived,
                officerDealing = table.OfficerDealing,
                EldestChild = table.EldestChild,
                caOrderType = MemoryCollections.CaOrderTypeList.GetOrderTypeList().FirstOrDefault(x => x.Detail == table.CAOrderType),
                tipstaffRecordID = table.Id
            };

            return model;
        }
        
        public void RemoveChildAbduction(ChildAbduction childAbduction)
        {
            var entity = GetDynamoTable(childAbduction);

            _tipstaffRecordRepository.Delete(entity);
        }

        public void UpdateChildAbduction(ChildAbduction childAbduction)
        {
            var entity = GetDynamoTable(childAbduction);

            _tipstaffRecordRepository.Update(entity);
        }
    }
}