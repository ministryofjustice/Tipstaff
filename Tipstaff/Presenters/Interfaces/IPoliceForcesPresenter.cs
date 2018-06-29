using System.Collections.Generic;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public interface IPoliceForcesPresenter
    {
        void AddPoliceForces(PoliceForces contact);

        PoliceForces GetPoliceForces(string id);

        IEnumerable<PoliceForces> GetAllPoliceForces();

        void Update(PoliceForces policeforces);

        void Delete(PoliceForces policeforces);
    }
}
