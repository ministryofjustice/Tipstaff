using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("AttendanceNote")]
    public class AttendanceNote
    {
        public int AttendanceNoteID { get; set; }
       
        public DateTime CallDated { get; set; }

        public DateTime? CallStarted { get; set; }

        public DateTime? CallEnded { get; set; }

        public string CallDetails { get; set; }
       
        public int AttendanceNoteCode{ get; set; }

        public int TipstaffRecordID { get; set; }
     
        public AttendanceNote() { }

        public AttendanceNote(DateTime started)
        {
            CallDated = started;
            CallStarted = started;
        }
    }
}
