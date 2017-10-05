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
    public class PoliceForcesRepository : IPoliceForcesRepository
    {
        private readonly IDynamoAPI<PoliceForces> _dynamoAPI;

        public PoliceForcesRepository(IDynamoAPI<PoliceForces> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public void AddPoliceForces(PoliceForces contact)
        {
            throw new NotImplementedException();
        }


        public void Delete(PoliceForces policeforces)
        {
            _dynamoAPI.Delete(policeforces);
        }

        public IEnumerable<PoliceForces> GetAllPoliceForces()
        {
            return _dynamoAPI.GetAll();
        }

        public PoliceForces GetPoliceForces(string id)
        {
            return _dynamoAPI.GetEntityByHashKey(id);
        }

        public void Update(PoliceForces policeforces)
        {
            var entity = _dynamoAPI.GetEntityByHashKey(policeforces.PoliceForceId);
            entity.Active= policeforces.Active;
            entity.Deactivated = policeforces.Deactivated;
            entity.DeactivatedBy = policeforces.DeactivatedBy;
            entity.PoliceForceEMail = policeforces.PoliceForceEMail;
            entity.PoliceForceName = policeforces.PoliceForceName;
            entity.LoggedInUser = policeforces.LoggedInUser;
            _dynamoAPI.Save(entity);
        }
    }
}
