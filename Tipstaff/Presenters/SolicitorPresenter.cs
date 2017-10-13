using System;
using System.Collections.Generic;
using System.Linq;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class SolicitorPresenter : ISolicitorPresenter, IMapper<Models.Solicitor, Tipstaff.Services.DynamoTables.Solicitor>, IMapperCollections<Models.Solicitor,Tipstaff.Services.DynamoTables.Solicitor>
    {
        private readonly ISolicitorRepository _solicitorRepository;

        public SolicitorPresenter(ISolicitorRepository solicitorRepository)
        {
            _solicitorRepository = solicitorRepository;
        }

        public void AddSolicitor(Models.Solicitor solicitor)
        {
            var entity = GetDynamoTable(solicitor);
            _solicitorRepository.AddSolicitor(entity);
        }

        public Solicitor GetSolicitor(string id)
        {
            var entity = _solicitorRepository.GetSolicitor(id);
            return GetModel(entity);
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
               LastName = model.firstName,
               PhoneDayTime = model.phoneDayTime,
               PhoneOutOfHours = model.phoneOutofHours,
               Salutation = MemoryCollections.SalutationList.GetSalutationByDetail(model.salutation.Detail).Detail,
               SolicitorFirmID = model.solicitorFirmID,
               Id = model.solicitorID,
               SolicitorName = model.solicitorName
            };

            return entity;
        }

        public Models.Solicitor GetModel(Services.DynamoTables.Solicitor table)
        {
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
                salutation = MemoryCollections.SalutationList.GetSalutationByDetail(table.Salutation)
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
    }
}