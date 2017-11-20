using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface IUsersRepository
    {
        IEnumerable<User> GetAll();

        void Update(User user);

        void Add(User user);

        User GetUserByID(string id);

        User GetUserByName(string name);
    }
}
