using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("Tipstaff_Cache")]
    public class Cache : DynamoTable
    {
        public string Context { get; set; }

        public DateTime DateTime { get; set; }
    }
}
