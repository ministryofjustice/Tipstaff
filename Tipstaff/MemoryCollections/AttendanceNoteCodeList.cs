using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tipstaff.MemoryCollections
{
    public class AttendanceNoteCode
    {
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
    }
}