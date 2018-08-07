using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface ITipstaffRecordSolicitorsRepository
    {
        void AddRecord(Tipstaff_Solicitors address);

        IEnumerable<Tipstaff_Solicitors> GetAllByCondition<T>(string name, T value);
    }
}
