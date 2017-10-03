using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Tests.Integration
{
    internal class userRepository : IUserRepository
    {
        private IDynamoAPI<User> _dynamoAPI;

        public userRepository(IDynamoAPI<User> _dynamoAPI)
        {
            this._dynamoAPI = _dynamoAPI;
        }
    }
}