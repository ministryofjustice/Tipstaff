using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface IUserRepository
    {
        void AddUser(User contact);

        User GetUser(string id);

        IEnumerable<User> GetAllUsers();

        void Update(User user);

        void Delete(User user);
    }
}
