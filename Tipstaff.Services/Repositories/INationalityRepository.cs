using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface INationalityRepository
    {
        void AddNationality(Nationality nationality);

        Nationality GetNationality(string id);

        IEnumerable<Nationality> GetAllNationalities();

        void Update(Nationality nationality);

        void Delete(Nationality nationality);

    }
}
