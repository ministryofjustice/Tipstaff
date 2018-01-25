using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class AttendanceNotesRepository : IAttendanceNotesRepository  
    {
        private readonly IDynamoAPI<AttendanceNote> _dynamoAPI;

        public AttendanceNotesRepository(IDynamoAPI<AttendanceNote> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public void AddAttendanceNote(AttendanceNote note)
        {
            _dynamoAPI.Save(note);
         }

        public void DeleteAttendanceNote(AttendanceNote note)
        {
            _dynamoAPI.Delete(note);
        }

        public AttendanceNote GetAttendanceNote(string id)
        {
            return _dynamoAPI.GetEntityByKey(id);
        }

        public AttendanceNote GetAttendanceNoteByIdAndRange(string id, string range)
        {
            return _dynamoAPI.GetEntity(id, range);
        }

        public AttendanceNote GetEntityByObjectKey(object hashKey, object rangeKey)
        {
            return _dynamoAPI.GetEntityByKeys(hashKey, rangeKey);
        }

        public AttendanceNote GetEntityByObjectKey(object key)
        {
            return _dynamoAPI.GetEntityByKey(key);
        }
    }
}
