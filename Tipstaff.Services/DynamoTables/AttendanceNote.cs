using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("Tipstaff_AttendanceNote")]
    public class AttendanceNote
    {
        [DynamoDBHashKey]
        public string AttendanceNoteID { get; set; }

        [DynamoDBRangeKey]
        public string TipstaffRecordID { get; set; }

        public DateTime CallDated { get; set; }

        public DateTime? CallStarted { get; set; }

        public DateTime? CallEnded { get; set; }

        public string CallDetails { get; set; }
     
        public string AttendanceNoteCode { get; set; }
    }
}
