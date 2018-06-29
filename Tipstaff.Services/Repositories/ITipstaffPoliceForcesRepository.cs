using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface ITipstaffPoliceForcesRepository
    {
        void Add(Tipstaff_PoliceForces tpf);

        Tipstaff_PoliceForces GetTipstaffPoliceForces(string id);

        void Delete(Tipstaff_PoliceForces tpf);

        void Update(Tipstaff_PoliceForces tpf);

        IEnumerable<Tipstaff_PoliceForces> GetTipstaffPoliceForcesByTipstaffRecordID(string id);
    }
}
