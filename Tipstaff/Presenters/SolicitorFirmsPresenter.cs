using System.Collections.Generic;
using System.Linq;
using Tipstaff.Mappers;
using Tipstaff.Services.Repositories;
using TPLibrary.GuidGenerator;

namespace Tipstaff.Presenters
{
    public class SolicitorFirmsPresenter : ISolicitorFirmsPresenter, IMapper<Models.SolicitorFirm, Tipstaff.Services.DynamoTables.SolicitorFirm> , IMapperCollections<Models.SolicitorFirm, Tipstaff.Services.DynamoTables.SolicitorFirm>
    {
        private readonly ISolicitorFirmRepository _solicitiorFirmRepository;
        private readonly ISolicitorPresenter _solicitorPresenter;
        private readonly IGuidGenerator _guidGenerator;
        
        public SolicitorFirmsPresenter(ISolicitorFirmRepository solicitiorFirmRepository, ISolicitorPresenter solicitorPresenter, IGuidGenerator guidGenerator)
        {
            _solicitiorFirmRepository = solicitiorFirmRepository;
            _solicitorPresenter = solicitorPresenter;
            _guidGenerator = guidGenerator;
        }

        public void AddSolicitorFirm(Models.SolicitorFirm solicitorFirm)
        {
            solicitorFirm.solicitorFirmID = _guidGenerator.GenerateTimeBasedGuid().ToString();

            var entity = GetDynamoTable(solicitorFirm);

            _solicitiorFirmRepository.AddSolicitorFirm(entity);
        }

        public void Delete(Models.SolicitorFirm solicitorFirm)
        {
            var entity = GetDynamoTable(solicitorFirm);

            _solicitiorFirmRepository.Delete(entity);
        }

        public IEnumerable<Models.SolicitorFirm> GetAll(IEnumerable<Services.DynamoTables.SolicitorFirm> entities)
        {
            return entities.Select(x => GetModel(x));
        }

        public IEnumerable<Services.DynamoTables.SolicitorFirm> GetAll(IEnumerable<Models.SolicitorFirm> entities)
        {
            return entities.Select(x => GetDynamoTable(x));
        }

        public IEnumerable<Models.SolicitorFirm> GetAllSolicitorFirms()
        {
            var firms = _solicitiorFirmRepository.GetAllSolicitorFirms();

            return GetAll(firms);
        }

        public Services.DynamoTables.SolicitorFirm GetDynamoTable(Models.SolicitorFirm model)
        {
            var entity = new Services.DynamoTables.SolicitorFirm()
            {
                DeactivatedBy = model.deactivatedBy,
                Active = model.active,
                AddressLine1 = model.addressLine1,
                AddressLine2 = model.addressLine2,
                AddressLine3 = model.addressLine3,
                County = model.county,
                Deactivated = model.deactivated,
                Id = model.solicitorFirmID,
                DX = model.DX,
                Email = model.email,
                FirmName = model.firmName,
                PhoneDayTime = model.phoneDayTime,
                PhoneOutofHours = model.phoneOutofHours,
                Postcode = model.postcode,
                Town = model.town
            };

            return entity;
        }

        public Models.SolicitorFirm GetModel(Services.DynamoTables.SolicitorFirm entity)
        {
            var model = new Models.SolicitorFirm()
            {
                active = entity.Active,
                addressLine1 = entity.AddressLine1,
                addressLine2 = entity.AddressLine2,
                addressLine3 = entity.AddressLine3,
                county = entity.County,
                deactivated = entity.Deactivated,
                deactivatedBy = entity.DeactivatedBy,
                DX = entity.DX,
                email = entity.Email,
                firmName = entity.FirmName,
                phoneDayTime = entity.PhoneDayTime,
                phoneOutofHours = entity.PhoneOutofHours,
                postcode = entity.Postcode,
                solicitorFirmID = entity.Id,
                town = entity.Town,
                Solicitors = _solicitorPresenter.GetSolicitors().Where(x=>x.solicitorFirmID == entity.Id)
            };

            return model;
        }

        public Models.SolicitorFirm GetSolicitorFirm(string id)
        {
            var entity = _solicitiorFirmRepository.GetSolicitorFirm(id);

            var model = GetModel(entity);

            return model;
        }

        public void Update(Models.SolicitorFirm solicitorFirm)
        {
            var entity = GetDynamoTable(solicitorFirm);

            _solicitiorFirmRepository.Update(entity);
        }
    }
}