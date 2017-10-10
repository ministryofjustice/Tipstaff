using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
