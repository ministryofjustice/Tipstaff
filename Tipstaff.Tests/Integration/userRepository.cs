using System;
using System.Collections.Generic;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Models;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Tests.Integration
{
    internal class userRepository 
    {
        private IDynamoAPI<User> _dynamoAPI;

        public userRepository(IDynamoAPI<User> _dynamoAPI)
        {
            this._dynamoAPI = _dynamoAPI;
        }

        public void AddUser(User contact)
        {
            throw new NotImplementedException();
        }

        public void Delete(User user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public User GetUser(string id)
        {
            throw new NotImplementedException();
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}