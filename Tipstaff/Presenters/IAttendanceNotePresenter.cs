using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public interface IAttendanceNotePresenter : ITipstaffRecordPresenter
    {
        void AddAttendanceNote(Tipstaff.Models.AttendanceNote note);

        void DeleteAttendanceNote(Tipstaff.Models.AttendanceNote note);

        Tipstaff.Models.AttendanceNote GetAttendanceNote(string id);
    }
}
