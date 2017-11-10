using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class WarrantPresenter : IWarrantPresenter, IMapper<Warrant, Services.DynamoTables.TipstaffRecord>
    {
        private readonly ITipstaffRecordRepository _tipstaffRecordRepository;

        public WarrantPresenter(ITipstaffRecordRepository _tipstaffRecordRepository)
        {

        }

        public void AddDeletedTipstaffRecord(Models.DeletedTipstaffRecord record)
        {
            throw new NotImplementedException();
        }

        public void AddWarrant(Warrant warrant)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Warrant> GetAllWarrants()
        {
            throw new NotImplementedException();
        }

        public Services.DynamoTables.TipstaffRecord GetDynamoTable(Warrant table)
        {
            throw new NotImplementedException();
        }

        public Warrant GetModel(Services.DynamoTables.TipstaffRecord model)
        {
            throw new NotImplementedException();
        }

        public Models.TipstaffRecord GetTipstaffRecord(string id)
        {
            throw new NotImplementedException();
        }

        public Warrant GetWarrant(string id)
        {
            throw new NotImplementedException();
        }

        public void RemoveWarrant(Warrant warrant)
        {
            throw new NotImplementedException();
        }

        public void UpdateTipstaffRecord(Models.TipstaffRecord tipstaffRecord)
        {
            throw new NotImplementedException();
        }
    }
}