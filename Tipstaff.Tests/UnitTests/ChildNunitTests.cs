﻿//using System;
//using NUnit.Framework;
//using Tipstaff.Services.Repositories;
//using Tipstaff.Infrastructure.Repositories;
//using Tipstaff.Services.DynamoTables;
//using TPLibrary.DynamoAPI;
//using TPLibrary.GuidGenerator;

//namespace Tipstaff.Tests.UnitTests
//{
//    [TestFixture]
//    public class ChildNunitTests
//    {
//        private IChildRepository _childRepository;
//        private IAuditEventRepository _auditRepo;
//        private IDynamoAPI<Child> _dynamoAPI;
//        string childIndex = string.Empty;
//        string tipstaffIndex = string.Empty;
//        Child child;

//        [SetUp]
//        public void SetUp()
//        {

//            _dynamoAPI = new DynamoAPI<Child>();
//            _auditRepo = new AuditEventRepository(new DynamoAPI<AuditEvent>(), new GuidGenerator());
//            _childRepository = new ChildRepository(_dynamoAPI, _auditRepo);
//            childIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
//            tipstaffIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
//        }

//        [Test]
//        public void Create_Should_Add_New_Child()
//        {
//            _childRepository.AddChild(new Child()
//            {
//                Id = childIndex,
//                TipstaffRecordID = tipstaffIndex,
//                Build = "average",
//                Country = "Spain",
//                DateOfBirth = DateTime.Now.AddYears(-10),
//                EyeColour = "Brown",
//                Gender = "Male",
//                HairColour = "Black",
//                Height = "150",
//                NameFirst = "Juanito",
//                NameLast = "Nadie",
//                NameMiddle = "",
//                Nationality = "Spanish",
//                PNCID = "12345",
//                SkinColour = "white",
//                Specialfeatures = "none"
//            });

//            child = _childRepository.GetChildByIdAndRange(childIndex, tipstaffIndex);

//            Assert.AreEqual(tipstaffIndex, child.TipstaffRecordID);
//            Assert.AreEqual("Nadie", child.NameLast);
//            Assert.AreEqual("12345", child.PNCID);
//            Assert.AreEqual("none", child.Specialfeatures);
//        }

//        [Test]
//        public void Update_Should_Update_Child()
//        {
//            _childRepository.AddChild(new Child()
//            {
//                Id = childIndex,
//                TipstaffRecordID = tipstaffIndex,
//                Build = "average",
//                Country = "Spain",
//                DateOfBirth = DateTime.Now.AddYears(-10),
//                EyeColour = "Brown",
//                Gender = "Male",
//                HairColour = "Black",
//                Height = "150",
//                NameFirst = "Juanito",
//                NameLast = "Nadie",
//                NameMiddle = "",
//                Nationality = "Spanish",
//                PNCID = "12345",
//                SkinColour = "white",
//                Specialfeatures = "none"
//            });

//            _childRepository.Update(new Child()
//            {
//                Id = childIndex,
//                TipstaffRecordID = tipstaffIndex,
//                Build = "average modified",
//                Country = "Spain",
//                DateOfBirth = DateTime.Now.AddYears(-10),
//                EyeColour = "Brown",
//                Gender = "Male",
//                HairColour = "Black",
//                Height = "150",
//                NameFirst = "Juanito",
//                NameLast = "Nadie modified",
//                NameMiddle = "",
//                Nationality = "Spanish",
//                PNCID = "12345 modified",
//                SkinColour = "white",
//                Specialfeatures = "none updated"
//            });

//            child = _childRepository.GetChildByIdAndRange(childIndex, tipstaffIndex);

//            Assert.AreEqual(tipstaffIndex, child.TipstaffRecordID);
//            Assert.AreEqual("Nadie modified", child.NameLast);
//            Assert.AreNotEqual("12345", child.PNCID);
//            Assert.AreNotEqual("none", child.Specialfeatures);

//        }

//        [TearDown]
//        public void TearDown()
//        {
//            _childRepository.Delete(child);
//        }
//    }
//}
