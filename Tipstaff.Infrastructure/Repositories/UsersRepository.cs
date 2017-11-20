using System.Collections.Generic;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using System.Linq;

namespace Tipstaff.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IDynamoAPI<User> _dynamoAPI;

        public UsersRepository(IDynamoAPI<User> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public void Add(User user)
        {
            _dynamoAPI.Save(user);
        }

        public IEnumerable<User> GetAll()
        {
            return _dynamoAPI.GetAll();
        }

        public User GetUserByID(string id)
        {
            return _dynamoAPI.GetEntityByHashKey(id);
        }

        public User GetUserByName(string name)
        {
            var users =  _dynamoAPI.GetResultsByCondition("Name",Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal,name);
            return users.FirstOrDefault();
        }

        public void Update(User user)
        {
            var entity = _dynamoAPI.GetEntityByHashKey(user.Id);
            entity.DisplayName = user.DisplayName;
            entity.Id = user.Id;
            entity.LastActive = user.LastActive;
            entity.Name = user.Name;
            entity.Role = user.Role;
            entity.RoleStrength = user.RoleStrength;
            
            _dynamoAPI.Save(entity);
        }
    }
}
