//using NUnit.Framework;
//using System;
//using Tipstaff.Controllers;
//using Tipstaff.Services.Repositories;
//using Tipstaff.Infrastructure.Repositories;
//using Tipstaff.Infrastructure.DynamoAPI;
//using Tipstaff.Models;
//using Tipstaff.Infrastructure.Services;
//using Moq;

//namespace Tipstaff.Tests.Integration
//{
//    [TestFixture]
//    public class AttendanceNoteControllerTests
//    {
//        private AttendanceNoteController _sub;
//        private IAttendanceNotesRepository _attendanceNotesRepository;
//        private IDynamoAPI<Tipstaff.Services.DynamoTables.AttendanceNote> _dynamoAPI;
//        private ITipstaffRecordRepository _tipstaffRecordRepository;
//        //string tipstaffRecordIndex = string.Empty;
//        //string attendanceNoteIndex = string.Empty;
//        //string tipstaffRecordRangeIndex = string.Empty;
//        private Mock<IGuidGenerator> _guidGeneratorMock;
//        Guid id;

//        [SetUp]
//        public void SetUp()
//        {
//            _dynamoAPI = new DynamoAPI<Tipstaff.Services.DynamoTables.AttendanceNote>();
//            _attendanceNotesRepository = new AttendanceNotesRepository(_dynamoAPI);
//            _tipstaffRecordRepository = new TipstaffRecordRepository(new DynamoAPI<Tipstaff.Services.DynamoTables.TipstaffRecord>());
//            id = Guid.NewGuid();
//            _guidGeneratorMock.Setup(x => x.GenerateTimeBasedGuid()).Returns(id);
//            //tipstaffRecordIndex = GuidGenerator.GenerateTimeBasedGuid().ToString();
//            //tipstaffRecordRangeIndex = GuidGenerator.GenerateTimeBasedGuid().ToString();
//            _sub = new AttendanceNoteController(_attendanceNotesRepository, _tipstaffRecordRepository, _guidGeneratorMock.Object);
//        }

//        [Test]
//        public void Create_Should_Add_New_AttendanceNote()
//        {
//            _tipstaffRecordRepository.Add(new Services.DynamoTables.TipstaffRecord()
//            {
//                ArrestCount = 1,
//                TrackItem = tipstaffRecordRangeIndex,
//                Discriminator = "ChildAbduction",
//                CreatedBy = "VZ",
//                CreatedOn = DateTime.Now,
//                OfficerDealing = "VZ",
//                EldestChild = "John Doe",
//                NPO = "NPO-Test",
//                PrisonCount = 1,
//                SentSCD26 = DateTime.Now,
//                OrderReceived = DateTime.Now.AddDays(-1),
//                Division = "Division-Test",
//                ResultDate = DateTime.Now.AddDays(10),
//                TipstaffRecordID = tipstaffRecordIndex
//            });

//            var itemRow = _tipstaffRecordRepository.GetEntityByObjectKey(tipstaffRecordIndex, tipstaffRecordRangeIndex);

//            //Use Id
//            var view = _sub.Create(new AttendanceNote()
//            {
//                callDated = DateTime.Now,
//                callEnded = DateTime.Now.AddMinutes(10),
//                callDetails = "Test",
//                tipstaffRecordID = itemRow.TipstaffRecordID,
//                callStarted = DateTime.Now,
//                AttendanceNoteID = attendanceNoteIndex
//            });

//            var note = _attendanceNotesRepository.GetEntityByObjectKey(attendanceNoteIndex);

//            Assert.AreEqual(attendanceNoteIndex, note.AttendanceNoteID);
//        }

//        [TearDown]
//        public void TearDown()
//        {

//        }
//    }
//}
