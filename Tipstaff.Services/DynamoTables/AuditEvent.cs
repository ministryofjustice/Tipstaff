﻿using Amazon.DynamoDBv2.DataModel;
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

        public int UserId { get; set; }

        public int AuditEventDescriptionId { get; set; }

        public string RecordChanged { get; set; }

        public int RecordAddedTo { get; set; }

        public int DeletedReasonId { get; set; }
    }
}
