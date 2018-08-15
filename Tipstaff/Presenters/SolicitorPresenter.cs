using System;
using System.Collections.Generic;
using System.Linq;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.Repositories;
using TPLibrary.GuidGenerator;

namespace Tipstaff.Presenters
{
    public class SolicitorPresenter : ISolicitorPresenter, IMapper<Models.Solicitor, Tipstaff.Services.DynamoTables.Solicitor>, IMapperCollections<Models.Solicitor,Tipstaff.Services.DynamoTables.Solicitor>
    {
        private readonly ISolicitorRepository _solicitorRepository;
        private readonly ISolicitorFirmRepository _solicitorFirmRepository;
        private readonly ITipstaffRecordSolicitorsRepository _tipstaffRecordSolicitorsRepository;
        
        public SolicitorPresenter(ISolicitorRepository solicitorRepository, ISolicitorFirmRepository solicitorFirmRepository, 
            ITipstaffRecordSolicitorsRepository tipstaffRecordSolicitorsRepository)
        {
            _solicitorRepository = solicitorRepository;
            _solicitorFirmRepository = solicitorFirmRepository;
            _tipstaffRecordSolicitorsRepository = tipstaffRecordSolicitorsRepository;
        }

        public void AddSolicitor(Models.Solicitor solicitor)
        {
            var entity = GetDynamoTable(solicitor);
            _solicitorRepository.AddSolicitor(entity);
        }

        public Solicitor GetSolicitor(string id)
        {
            var sol = new Models.Solicitor();

            var entity = _solicitorRepository.GetSolicitor(id);

            var model = entity != null ? GetModel(entity) : sol;

            return sol;
        }

        public IEnumerable<Models.Solicitor> GetSolicitors()
        {
            var entities = _solicitorRepository.GetSolicitors();

            return GetAll(entities);
        }
        
        public void Update(Models.Solicitor solicitor)
        {
            var entity = GetDynamoTable(solicitor);
            _solicitorRepository.Update(entity);
        }

        public Services.DynamoTables.Solicitor GetDynamoTable(Models.Solicitor model)
        {
            var entity = new Services.DynamoTables.Solicitor()
            {
               Active = model.active,
               Dectivated = model.deactivated,
               Email = model.email,
               DectivatedBy = model.deactivatedBy,
               FirstName = model.firstName,
               LastName = model.lastName,
               PhoneDayTime = model.phoneDayTime,
               PhoneOutOfHours = model.phoneOutofHours,
               Salutation = MemoryCollections.SalutationList.GetSalutationByID(model.salutation.SalutationId)?.Detail,
               SolicitorFirmID = model.solicitorFirmID,
               Id = model.solicitorID,
               SolicitorName = model.solicitorName
            };

            return entity;
        }

        public Models.Solicitor GetModel(Services.DynamoTables.Solicitor table)
        {
            var firm = _solicitorFirmRepository.GetSolicitorFirm(table.SolicitorFirmID);

            var solicitorFirmMdl = new SolicitorFirm();

            if (firm != null)
            {
                solicitorFirmMdl.active = firm.Active;
                solicitorFirmMdl.addressLine1 = firm.AddressLine1;
                solicitorFirmMdl.addressLine2 = firm.AddressLine2;
                solicitorFirmMdl.addressLine3 = firm.AddressLine3;
                solicitorFirmMdl.county = firm.County;
                solicitorFirmMdl.DX = firm.DX;
                solicitorFirmMdl.email = firm.Email;
                solicitorFirmMdl.firmName = firm.FirmName;
                solicitorFirmMdl.phoneDayTime = firm.PhoneDayTime;
                solicitorFirmMdl.phoneOutofHours = firm.PhoneOutofHours;
                solicitorFirmMdl.postcode = firm.Postcode;
                solicitorFirmMdl.town = firm.Town;
            }

            var model = new Models.Solicitor()
            {
                active = table.Active,
                deactivated = table.Dectivated,
                deactivatedBy = table.DectivatedBy,
                email = table.Email,
                firstName = table.FirstName,
                lastName = table.LastName,
                phoneDayTime = table.PhoneDayTime,
                phoneOutofHours = table.PhoneOutOfHours,
                solicitorID = table.Id,
                solicitorFirmID = table.SolicitorFirmID,
                salutation = MemoryCollections.SalutationList.GetSalutationByDetail(table.Salutation),
                solicitorFirmName = table.FirstName,
                SolicitorFirm = solicitorFirmMdl
            };

            return model;
        }
        
        public IEnumerable<Models.Solicitor> GetAll(IEnumerable<Services.DynamoTables.Solicitor> entities)
        {
            return entities.Select(x => GetModel(x));
        }

        public IEnumerable<Services.DynamoTables.Solicitor> GetAll(IEnumerable<Models.Solicitor> entities)
        {
            return entities.Select(x => GetDynamoTable(x));
        }

        public void AddRecord(string solicitorId, string tipstaffRecordId, string key)
        {
            var dataTable = new Services.DynamoTables.Tipstaff_Solicitors();
            dataTable.SolicitorID = solicitorId;
            dataTable.TipstaffRecordID = tipstaffRecordId;
            dataTable.Id = key;
            _tipstaffRecordSolicitorsRepository.AddRecord(dataTable);
        }

        public IEnumerable<TipstaffRecordSolicitor> GetTipstaffRecordSolicitors(string tipstaffRecordId)
        {
            var tipstaffRecordSolicitors = new List<TipstaffRecordSolicitor>();

            IEnumerable<Services.DynamoTables.Tipstaff_Solicitors> solicitorIds = new List<Services.DynamoTables.Tipstaff_Solicitors>();
            solicitorIds = _tipstaffRecordSolicitorsRepository.GetAllByCondition("TipstaffRecordID", tipstaffRecordId);

            if (solicitorIds.Any())
            {
                foreach (var item in solicitorIds)
                {
                    var solicitor = GetSolicitor(item.SolicitorID);
                    tipstaffRecordSolicitors.Add(new TipstaffRecordSolicitor() { solicitor = solicitor, solicitorID = item.SolicitorID, tipstaffRecordID = item.TipstaffRecordID });
                }
            }

            return tipstaffRecordSolicitors;
        }
    }
}