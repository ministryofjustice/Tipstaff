//using NUnit.Framework;
//using System;
//using Tipstaff.Infrastructure.DynamoAPI;
//using Tipstaff.Infrastructure.Repositories;
//using Tipstaff.Infrastructure.Services;
//using Tipstaff.Services.DynamoTables;
//using Tipstaff.Services.Repositories;

//namespace Tipstaff.Tests.Integration
//{
//    public class UserControllerTests
//    {
//        [TestFixture]
//        public class UsersControllerTests
//        {
//            private UserController _sub;
//            private IUserRepository _userRepository;
//            private IDynamoAPI<Tipstaff.Services.DynamoTables.User> _dynamoAPI;
//            string userIndex = string.Empty;
//            User user;


//            [SetUp]
//            public void SetUp()
//            {
//                _dynamoAPI = new DynamoAPI<Tipstaff.Services.DynamoTables.User>();
//                _userRepository = new UserRepository(_dynamoAPI);
//                userIndex = GuidGenerator.GenerateTimeBasedGuid().ToString();
//                _sub = new UserController(_userRepository);
//            }

//            [Test]
//            public void Create_Should_Add_New_User()
//            {
//                var response = _sub.Create(new Models.User()
//                {
//                   //create to be added
//        });

//                // to be added
//            }

//            [Test]
//            public void Update_Should_Update_PoliceForces()
//            {
//            }

//            [TearDown]
//            public void TearDown()
//            {
//                // to be added for user
//            }
//        }
//    }
//}
