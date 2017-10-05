using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface IAttendanceNotesRepository
    {
        void AddAttendanceNote(AttendanceNote note);

        void DeleteAttendanceNote(AttendanceNote note);
    
        AttendanceNote GetAttendanceNote(string id);

        AttendanceNote GetEntityByObjectKey(object key);
    }
}
