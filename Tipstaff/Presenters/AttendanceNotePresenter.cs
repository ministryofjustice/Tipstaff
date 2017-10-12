using System;
using System.Collections.Generic;
using System.Linq;
using Tipstaff.Infrastructure.Services;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class AttendanceNotePresenter : IMapper<Models.AttendanceNote, Services.DynamoTables.AttendanceNote>, IAttendanceNotePresenter
    {
        private readonly IAttendanceNotesRepository _attendanceNotesRepository;
        private readonly ITipstaffRecordPresenter _tipstaffRecordPresenter;
        private readonly IGuidGenerator _guidGenerator;

        public AttendanceNotePresenter(IAttendanceNotesRepository attendanceNotesRepository, IGuidGenerator guidGenerator, ITipstaffRecordPresenter tipstaffRecordPresenter)
        {
            _attendanceNotesRepository = attendanceNotesRepository;
            _guidGenerator = guidGenerator;
            _tipstaffRecordPresenter = tipstaffRecordPresenter;
        }

        public void AddAttendanceNote(Models.AttendanceNote note)
        {
            var dynamoEntity = GetDynamoTable(note);

            _attendanceNotesRepository.AddAttendanceNote(dynamoEntity);
        }

        public void DeleteAttendanceNote(Models.AttendanceNote note)
        {
            var record = GetDynamoTable(note);

            _attendanceNotesRepository.DeleteAttendanceNote(record);
        }

        public Models.AttendanceNote GetAttendanceNote(string id)
        {
            var note = _attendanceNotesRepository.GetAttendanceNote(id);

            var model = GetModel(note);

            return model;
        }
       
        public Models.TipstaffRecord GetTipStaffRecord(string id)
        {
            var record =  _tipstaffRecordPresenter.GetTipStaffRecord(id);

            return record;
        }
        
        public Models.AttendanceNote GetModel(Services.DynamoTables.AttendanceNote table)
        {
            var model = new Models.AttendanceNote()
            {
                callDated = table.CallDated,
                callDetails = table.CallDetails,
                callEnded = table.CallEnded,
                callStarted = table.CallStarted.Value,
                AttendanceNoteCode = MemoryCollections.AttendanceNoteCodeList.GetAttendanceNoteCodeList().FirstOrDefault(x => x.Detail == table.AttendanceNoteCode),
                AttendanceNoteID = table.Id,
                tipstaffRecordID = table.TipstaffRecordID,
                tipstaffRecord = GetTipStaffRecord(table.TipstaffRecordID)
            };

            return model;
        }
        
        public Services.DynamoTables.AttendanceNote GetDynamoTable(Models.AttendanceNote model)
        {
            var table = new Tipstaff.Services.DynamoTables.AttendanceNote()
            {
                TipstaffRecordID = model.tipstaffRecordID,
                AttendanceNoteCode = model.AttendanceNoteCode.Detail,
                CallDated = model.callDated,
                CallDetails = model.callDetails,
                CallEnded = model.callEnded,
                CallStarted = model.callStarted,
                Id = model.AttendanceNoteID
            };

            return table;
        }

        public IEnumerable<TipstaffRecord> GettAllRecords()
        {
            throw new NotImplementedException();
        }
    }
}