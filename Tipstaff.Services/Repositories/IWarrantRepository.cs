using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface IWarrantRepository
    {
        void AddWarrant(Warrant warrant);

        Warrant GetWarrant(int id);

        IEnumerable<Warrant> GetWarrants(int id, string name);

        void UpdateWarrant(Warrant contact);
    }
}
