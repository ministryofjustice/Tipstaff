//using Moq;
//using NUnit.Framework;
//using System;
//using Tipstaff.Areas.Admin.Controllers;
//using Tipstaff.Controllers;
//using Tipstaff.Infrastructure.DynamoAPI;
//using Tipstaff.Infrastructure.Repositories;
//using Tipstaff.Infrastructure.Services;
//using Tipstaff.Logger;
//using Tipstaff.Services.DynamoTables;
//using Tipstaff.Services.Repositories;

//namespace Tipstaff.Tests.Integration
//{
//    [TestFixture]
//    public class ChildControllerTests
//    {
//        private ChildController _sub;
//        private IChildRepository _childRepository;
//        private ITipstaffRecordRepository _tipstaffRepository;
//        private IDynamoAPI<Tipstaff.Services.DynamoTables.Child> _dynamoAPI;
//        private IDynamoAPI<Tipstaff.Services.DynamoTables.TipstaffRecord> _dynamoAPITipstaff;
//        Guid childIndex;
//        string tipstaffRecordIndex = string.Empty;
//        Child child;

//        private Mock<IGuidGenerator> _guidGeneratorMock;


//        [SetUp]
//        public void SetUp()
//        {

//            _dynamoAPI = new DynamoAPI<Tipstaff.Services.DynamoTables.Child>();
//            _childRepository = new ChildRepository(_dynamoAPI);
//            _tipstaffRepository = new TipstaffRecordRepository(_dynamoAPITipstaff);
//            _guidGeneratorMock = new Mock<IGuidGenerator>();



//            childIndex = Guid.NewGuid();
//            _guidGeneratorMock.Setup(x => x.GenerateTimeBasedGuid()).Returns(childIndex);
//            tipstaffRecordIndex = GuidGenerator.GenerateTimeBasedGuid().ToString();
//            _sub = new ChildController(_childRepository, _tipstaffRepository);
//        }

//        [Test]
//        public void Create_Should_Add_New_Child()
//        {
//            Models.Child c = new Models.Child()
//            {
//                childID = childIndex,
//                nameLast = "Surname",
//                nameFirst = "FirstName",
//                nameMiddle = "Middle Name",
//                dateOfBirth = DateTime.Today.AddYears(-10),
//                gender = MemoryCollections.GenderList.GetGenderByDetail("Male"),
//                height = "100",
//                build = "skinny",
//                hairColour = "black",
//                eyeColour = "black",
//                skinColour = MemoryCollections.SkinColourList.GetSkinColourByDetail("IC0 - Unknown"),
//                specialfeatures = "None",
//                country = MemoryCollections.CountryList.GetCountryByID(1),
//                nationality = MemoryCollections.NationalityList.GetNationalityByID(1),
//                tipstaffRecordID = tipstaffRecordIndex,
//                PNCID = "pncid test"
//            };
//            Models.ChildCreationModel acm = new Models.ChildCreationModel()
//            {
//                child = c,
//                tipstaffRecordID = tipstaffRecordIndex
//            };

//            var response = _sub.Create(acm, "");
//            child = _childRepository.GetChild(childIndex);

//            Assert.AreEqual(childIndex, child.ChildID);
//        }

//        [Test]
//        public void Update_Should_Update_Child()
//        {
//            var response = _sub.Create(new Models.ChildCreationModel()
//            {
//                child = new Models.Child()
//                {
//                    childID = childIndex,
//                    nameLast = "Surname",
//                    nameFirst = "FirstName",
//                    nameMiddle = "Middle Name",
//                    dateOfBirth = DateTime.Today.AddYears(-10),
//                    gender = MemoryCollections.GenderList.GetGenderByDetail("Male"),
//                    height = "100",
//                    build = "skinny",
//                    hairColour = "black",
//                    eyeColour = "black",
//                    skinColour = MemoryCollections.SkinColourList.GetSkinColourByDetail("IC0 - Unknown"),
//                    specialfeatures = "None",
//                    country = MemoryCollections.CountryList.GetCountryByID(1),
//                    nationality = MemoryCollections.NationalityList.GetNationalityByID(1),
//                    tipstaffRecordID = tipstaffRecordIndex,
//                    PNCID = "pncid test"
//                },
//                tipstaffRecordID = tipstaffRecordIndex
//            }, "");


//            response = _sub.Edit(new Models.ChildCreationModel()
//            {
//                child = new Models.Child()
//                {
//                    childID = childIndex,
//                    nameLast = "Surname modified",
//                    nameFirst = "FirstName modified",
//                    nameMiddle = "Middle Name",
//                    dateOfBirth = DateTime.Today.AddYears(-10),
//                    gender = MemoryCollections.GenderList.GetGenderByDetail("Male"),
//                    height = "100",
//                    build = "skinny fat",
//                    hairColour = "black",
//                    eyeColour = "black",
//                    skinColour = MemoryCollections.SkinColourList.GetSkinColourByDetail("IC0 - Unknown"),
//                    specialfeatures = "None",
//                    country = MemoryCollections.CountryList.GetCountryByID(1),
//                    nationality = MemoryCollections.NationalityList.GetNationalityByID(1),
//                    tipstaffRecordID = tipstaffRecordIndex,
//                    PNCID = "pncid test changed"
//                },
//                tipstaffRecordID = tipstaffRecordIndex
//            });

//            child = _childRepository.GetChild(childIndex);

//            Assert.AreEqual("Surname modified", child.NameLast);
//        }

//        [TearDown]
//        public void TearDown()
//        {
//            _childRepository.Delete(child);
//        }
//    }
//}
