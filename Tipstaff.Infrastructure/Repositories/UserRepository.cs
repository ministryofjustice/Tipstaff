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
    public class UserRepository : IUserRepository
    {
        private readonly IDynamoAPI<User> _dynamoAPI;

        public UserRepository(IDynamoAPI<User> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public void AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(User user)
        {
            _dynamoAPI.Delete(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _dynamoAPI.GetAll();
        }

        public User GetUser(string id)
        {
            throw new NotImplementedException();
        }

        public User GetUsers(string id)
        {
            return _dynamoAPI.GetEntityByHashKey(id);
        }

        public void Update(User user)
        {
            var entity = _dynamoAPI.GetEntityByHashKey(user.UserId);
            entity.Name = user.Name;
            entity.DisplayName = user.DisplayName;
            entity.LastActive = user.LastActive;
            entity.RoleStrength = user.RoleStrength;
            entity.Role = user.Role;
            _dynamoAPI.Save(entity);
        }
    }
}
