using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    public class DynamoTable
    { 
        [DynamoDBHashKey]
        public string Id { get; set; }
    }
}
