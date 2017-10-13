using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Infrastructure.Services;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class PoliceForcesPresenter : IPoliceForcesPresenter, IMapper<Models.PoliceForces, Tipstaff.Services.DynamoTables.PoliceForces>, IMapperCollections<Models.PoliceForces, Tipstaff.Services.DynamoTables.PoliceForces>
    {
        private readonly IPoliceForcesRepository _policeForcesRepository;

        public PoliceForcesPresenter(IPoliceForcesRepository policeForcesRepository)
        {
            _policeForcesRepository = policeForcesRepository;
        }

        public void AddPoliceForces(Models.PoliceForces policeForce)
        {
            var dt = GetDynamoTable(policeForce);
            _policeForcesRepository.AddPoliceForces(dt);
        }

        public void Delete(Models.PoliceForces policeforces)
        {
            var dt = GetDynamoTable(policeforces);
            _policeForcesRepository.Delete(dt);
        }

        public IEnumerable<Models.PoliceForces> GetAllPoliceForces()
        {
            var forces = _policeForcesRepository.GetAllPoliceForces();
            var mdl = GetAll(forces);
            return mdl;
        }

        public Models.PoliceForces GetPoliceForces(string id)
        {
            var dt = _policeForcesRepository.GetPoliceForces(id);
            var mdl = GetModel(dt);
            return mdl;
        }

        public void Update(Models.PoliceForces policeforces)
        {
            var dt = GetDynamoTable(policeforces);
            _policeForcesRepository.Update(dt);
        }

        public Models.PoliceForces GetModel(Services.DynamoTables.PoliceForces table)
        {
            var model = new Models.PoliceForces()
            {
                active = table.Active,
                policeForceName = table.PoliceForceName,
                deactivated = table.Deactivated,
                deactivatedBy = table.DeactivatedBy,
                loggedInUser = table.LoggedInUser,
                policeForceEmail = table.PoliceForceEMail,
                policeForceID = table.Id
            };

            return model;
        }

        public Services.DynamoTables.PoliceForces GetDynamoTable(Models.PoliceForces model)
        {
            var entity = new Tipstaff.Services.DynamoTables.PoliceForces()
            {
                Active = model.active,
                Deactivated = model.deactivated,
                DeactivatedBy = model.deactivatedBy,
                LoggedInUser = model.loggedInUser,
                PoliceForceEMail = model.policeForceEmail,
                Id = model.policeForceID,
                PoliceForceName = model.policeForceName
            };

            return entity;
        }

        public IEnumerable<Models.PoliceForces> GetAll(IEnumerable<Services.DynamoTables.PoliceForces> entities)
        {
            return entities.Select(x => GetModel(x));
        }

        public IEnumerable<Services.DynamoTables.PoliceForces> GetAll(IEnumerable<Models.PoliceForces> entities)
        {
            return entities.Select(x => GetDynamoTable(x));
        }
    }
}