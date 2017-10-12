using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Infrastructure.Services;
using Tipstaff.Models;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class PoliceForcesPresenter : IPoliceForcesPresenter
    {
        private readonly IPoliceForcesRepository _policeForcesRepository;
        private readonly IGuidGenerator _guidGenerator;

        public PoliceForcesPresenter(IPoliceForcesRepository policeForcesRepository, IGuidGenerator guidGenerator)
        {
            _policeForcesRepository = policeForcesRepository;
            _guidGenerator = guidGenerator;
        }

        public void AddPoliceForces(PoliceForces policeForce)
        {
            var dt = GetDTPoliceForce(policeForce);
            _policeForcesRepository.AddPoliceForces(dt);
        }

        public void Delete(PoliceForces policeforces)
        {
            var dt = GetDTPoliceForce(policeforces);
            _policeForcesRepository.Delete(dt);
        }

        public IEnumerable<PoliceForces> GetAllPoliceForces()
        {
            var forces = _policeForcesRepository.GetAllPoliceForces();
            var mdl = GetAllMdlPoliceForces(forces);
            return mdl;
        }

        public PoliceForces GetPoliceForces(string id)
        {
            var dt = _policeForcesRepository.GetPoliceForces(id);
            var mdl = GetMDPoliceForces(dt);
            return mdl;
        }

        public void Update(PoliceForces policeforces)
        {
            var dt = GetDTPoliceForce(policeforces);
            _policeForcesRepository.Update(dt);
        }

        private Tipstaff.Services.DynamoTables.PoliceForces GetDTPoliceForce(PoliceForces mdl)
        {
            var force = new Tipstaff.Services.DynamoTables.PoliceForces()
            {
                Active = mdl.active,
                Deactivated = mdl.deactivated,
                DeactivatedBy = mdl.deactivatedBy,
                LoggedInUser = mdl.loggedInUser,
                PoliceForceEMail = mdl.policeForceEmail,
                Id = _guidGenerator.GenerateTimeBasedGuid().ToString(),
                PoliceForceName = mdl.policeForceName
            };

            return force;
        }

        private PoliceForces GetMDPoliceForces(Tipstaff.Services.DynamoTables.PoliceForces dt)
        {
            var forces = new PoliceForces()
            {
                active = dt.Active,
                policeForceName = dt.PoliceForceName,
                deactivated = dt.Deactivated,
                deactivatedBy = dt.DeactivatedBy,
                loggedInUser = dt.LoggedInUser,
                policeForceEmail = dt.PoliceForceEMail,
                policeForceID = dt.Id
            };

            return forces;
        }

        private IEnumerable<PoliceForces> GetAllMdlPoliceForces(IEnumerable<Tipstaff.Services.DynamoTables.PoliceForces> forces)
        {
            return forces.Select(x => new PoliceForces()
            {
                active = x.Active,
                deactivated = x.Deactivated,
                deactivatedBy = x.DeactivatedBy,
                loggedInUser = x.LoggedInUser,
                policeForceEmail = x.PoliceForceEMail,
                policeForceID = x.Id,
                policeForceName = x.PoliceForceName
            });
        }
    }
}