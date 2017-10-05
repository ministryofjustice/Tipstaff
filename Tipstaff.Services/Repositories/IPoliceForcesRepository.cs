using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface IPoliceForcesRepository
    {
        void AddPoliceForces(PoliceForces contact);

        PoliceForces GetPoliceForces(string id);

        IEnumerable<PoliceForces> GetAllPoliceForces();

        void Update(PoliceForces policeforces);

        void Delete(PoliceForces policeforces);
    }
}
