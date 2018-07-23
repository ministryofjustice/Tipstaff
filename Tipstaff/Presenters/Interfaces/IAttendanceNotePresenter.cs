using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public interface IAttendanceNotePresenter 
    {
        void AddAttendanceNote(Tipstaff.Models.AttendanceNoteCreation note);

        void DeleteAttendanceNote(Tipstaff.Models.AttendanceNoteCreation note);

        Tipstaff.Models.AttendanceNoteCreation GetAttendanceNote(string id);

        IEnumerable<AttendanceNoteCreation> GetAllById(string id);
    }
}
