using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.MemoryCollections;
using Tipstaff.Models;
using Tipstaff.Presenters.Interfaces;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class UsersPresenter : IUsersPresenter, Mappers.IMapper<Models.User, Services.DynamoTables.User>
    {
        private readonly IUsersRepository _userRepository;

        public UsersPresenter(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Add(Models.User user)
        {
            var entity = GetDynamoTable(user);

            _userRepository.Add(entity);
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return RolesList.GetRolesList();
        }

        public IEnumerable<Models.User> GetAllUsers()
        {
            var users = _userRepository.GetAll();

            return users.Select(x => GetModel(x));
        }

        public Services.DynamoTables.User GetDynamoTable(Models.User model)
        {
            var user = new Services.DynamoTables.User()
            {
                Id = model.UserID.ToString(),
                DisplayName = model.DisplayName,
                LastActive = model.LastActive,
                Name = model.Name,
                Role = MemoryCollections.RolesList.GetRoleByDetail(model.Role.Detail).Detail,
                RoleStrength = model.RoleStrength
            };

            return user;
        }

        public Models.User GetModel(Services.DynamoTables.User table)
        {
            var model = new Models.User()
            {
                DisplayName = table.DisplayName,
                UserID = table.Id,
                LastActive = table.LastActive,
                Name = table.Name,
                RoleStrength = table.RoleStrength,
                Role = MemoryCollections.RolesList.GetRoleByDetail(table.Role),
            };

            return model;
        }

        public Models.User GetUserByID(string id)
        {
            var user = _userRepository.GetUserByID(id);

            var mdl = GetModel(user);

            return mdl;
        }

        public void Update(Models.User user)
        {
            var entity = GetDynamoTable(user);

            _userRepository.Update(entity);
        }
    }
}