using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("Tipstaff_Tipstaff_Solicitors")]
    public class Tipstaff_Solicitors:DynamoTable
    {
        [DynamoDBRangeKey]
        public string TipstaffRecordID { get; set; }

        public string SolicitorID { get; set; }
    }
}
