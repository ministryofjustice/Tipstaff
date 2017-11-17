﻿using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.DynamoTables
{
    [DynamoDBTable("Tipstaff_DeletedTipstaffRecord")]
    public class DeletedTipstaffRecord : DynamoTable
    {
        public string DeletedReason { get; set; }

        public string Discriminator { get; set; }

        public string UniqueRecordID { get; set; }
    }
}
