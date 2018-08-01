using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tipstaff.MemoryCollections
{
    public class AttendanceNoteCode
    {
        [Display(Name = "Attendance type")]
        public int Id { get; set; }

        public string Detail { get; set; }

        public int Active { get; set; }
    }
    
    public class AttendanceNoteCodeList
    {
        public static List<AttendanceNoteCode> GetAttendanceNoteCodeList()
        {
            return new List<AttendanceNoteCode>()
            {
                new AttendanceNoteCode() { Id = 1, Detail = "Phone call",           Active=1},
                new AttendanceNoteCode() { Id = 2, Detail = "Personal Attendance",  Active=1},
                new AttendanceNoteCode() { Id = 3, Detail = "Note",                 Active=1}
            };
        }

        public static AttendanceNoteCode GetAttendanceNoteCodeByDetail(string c)
        {
            return GetAttendanceNoteCodeList().FirstOrDefault(x => x.Detail == c);
        }

        public static AttendanceNoteCode GetAttendanceNoteCodeByID(int id)
        {
            return GetAttendanceNoteCodeList().FirstOrDefault(x => x.Id == id);
        }
    }
}