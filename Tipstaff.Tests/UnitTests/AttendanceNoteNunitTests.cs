using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Tipstaff.Services.Repositories;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Infrastructure.Services;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Tests.UnitTests
{
    [TestFixture]
    public class AttendanceNoteNunitTests
    {
        private IAttendanceNotesRepository _attendanceNoteRepository;
        private ITipstaffRecordRepository _tipstaffRepository;
        private IDynamoAPI<AttendanceNote> _dynamoAPI;
        private IDynamoAPI<TipstaffRecord> _tipstaffDynamoAPI;
        string attendanceNoteIndex = string.Empty;
        string tipstaffIndex = string.Empty;
        AttendanceNote attendanceNote;
        TipstaffRecord tr;

        [SetUp]
        public void SetUp()
        {

            _dynamoAPI = new DynamoAPI<AttendanceNote>();
            _tipstaffDynamoAPI = new DynamoAPI<TipstaffRecord>();
            _attendanceNoteRepository = new AttendanceNotesRepository(_dynamoAPI);
            _tipstaffRepository = new TipstaffRecordRepository(_tipstaffDynamoAPI);
            attendanceNoteIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
            tipstaffIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
        }

        [Test]
        public void Create_Should_Add_New_AttendanceNote()
        {
            _tipstaffRepository.Add(new TipstaffRecord()
            {
                Id = tipstaffIndex
            });

            _attendanceNoteRepository.AddAttendanceNote(new AttendanceNote() {
                Id = attendanceNoteIndex,
                TipstaffRecordID = tipstaffIndex,
                AttendanceNoteCode = "Attendance Note Code",
                CallDated = DateTime.Now,
                CallDetails = "Call details",
                CallEnded = DateTime.Now,
                CallStarted = DateTime.Now

            });

            attendanceNote = _attendanceNoteRepository.GetAttendanceNote(attendanceNoteIndex);

            Assert.AreEqual(tipstaffIndex, attendanceNote.TipstaffRecordID);
            Assert.AreEqual("Attendance Note Code", attendanceNote.AttendanceNoteCode);
            Assert.AreEqual("Call details", attendanceNote.CallDetails);


        }

        [TearDown]
        public void TearDown()
        {
            _attendanceNoteRepository.DeleteAttendanceNote(attendanceNote);
            _tipstaffRepository.Delete(tr);
        }
    }
}
