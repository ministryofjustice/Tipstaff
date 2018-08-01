using System;
using System.Collections.Generic;
using System.Linq;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class AttendanceNotePresenter : IMapper<Models.AttendanceNoteCreation, Services.DynamoTables.AttendanceNote>, IAttendanceNotePresenter
    {
        private readonly IAttendanceNotesRepository _attendanceNotesRepository;
        private readonly ITipstaffRecordPresenter _tipstaffRecordPresenter;
        
        public AttendanceNotePresenter(IAttendanceNotesRepository attendanceNotesRepository, ITipstaffRecordPresenter tipstaffRecordPresenter)
        {
            _attendanceNotesRepository = attendanceNotesRepository;
            _tipstaffRecordPresenter = tipstaffRecordPresenter;
        }

        public void AddAttendanceNote(Models.AttendanceNoteCreation note)
        {
            var dynamoEntity = GetDynamoTable(note);

            _attendanceNotesRepository.AddAttendanceNote(dynamoEntity);
        }

        public void DeleteAttendanceNote(Models.AttendanceNoteCreation note)
        {
            var record = GetDynamoTable(note);

            _attendanceNotesRepository.DeleteAttendanceNote(record);
        }

        public Models.AttendanceNoteCreation GetAttendanceNote(string id)
        {
            var note = _attendanceNotesRepository.GetAttendanceNote(id);

            var model = GetModel(note);

            return model;
        }
       
        
        public Models.AttendanceNoteCreation GetModel(Services.DynamoTables.AttendanceNote table)
        {
            var model = new Models.AttendanceNoteCreation()
            {
                callDated = table.CallDated,
                callDetails = table.CallDetails,
                callEnded = table.CallEnded,
                callStarted = table.CallStarted.Value,
                AttendanceNoteCode = MemoryCollections.AttendanceNoteCodeList.GetAttendanceNoteCodeList().FirstOrDefault(x => x.Detail == table.AttendanceNoteCode),
                AttendanceNoteID = table.Id,
                tipstaffRecordID = table.TipstaffRecordID,
                tipstaffRecord = _tipstaffRecordPresenter.GetTipStaffRecord(table.TipstaffRecordID)
            };

            return model;
        }
        
        public Services.DynamoTables.AttendanceNote GetDynamoTable(Models.AttendanceNoteCreation model)
        {
            var table = new Tipstaff.Services.DynamoTables.AttendanceNote()
            {
                TipstaffRecordID = model.tipstaffRecordID,
                AttendanceNoteCode = MemoryCollections.AttendanceNoteCodeList.GetAttendanceNoteCodeByID(model.AttendanceNoteCode.Id).Detail,
                CallDated = model.callDated,
                CallDetails = model.callDetails,
                CallEnded = model.callEnded,
                CallStarted = model.callStarted,
                Id = model.AttendanceNoteID
            };

            return table;
        }

        public IEnumerable<AttendanceNoteCreation> GetAllById(string id)
        {
            var entities = _attendanceNotesRepository.GetAllById(id);

            var caseRevies = entities.Select(x => GetModel(x));

            return caseRevies;
        }
    }
}