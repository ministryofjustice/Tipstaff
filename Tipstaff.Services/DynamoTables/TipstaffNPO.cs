using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("TipstaffRecord")]
    public class TipstaffNPO
    {
        [DynamoDBHashKey]
        public int TipstaffRecordID { get; set; }

        [DynamoDBRangeKey]
        public string UniqueRecordID { get; set; }

        public string NPO { get; set; }
    }
}
