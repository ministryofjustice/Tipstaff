using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("Tipstaff_AuditEvents")]
    public class AuditEvent : DynamoTable
    {
        public DateTime EventDate { get; set; }

        public string UserId { get; set; }

        public string AuditEventDescription { get; set; }

        public string RecordChanged { get; set; }

        public string RecordAddedTo { get; set; }

        public string DeletedReason { get; set; }

        public string ColumnName { get; set; }

        public string Was { get; set; }

        public string Now { get; set; }
    }
}
