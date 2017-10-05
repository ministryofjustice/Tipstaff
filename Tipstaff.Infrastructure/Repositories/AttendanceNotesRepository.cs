using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

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
            return _dynamoAPI.GetEntityByHashKey(id);
        }

        public AttendanceNote GetEntityByObjectKey(object hashKey, object rangeKey)
        {
            return _dynamoAPI.GetEntity(hashKey, rangeKey);
        }

        public AttendanceNote GetEntityByObjectKey(object key)
        {
            return _dynamoAPI.GetEntityByHashKey(key);
        }
    }
}
