using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("Tipstaff_FAQs")]
    public class FAQ : DynamoTable
    {
        public bool LoggedInUser { get; set; }
        
        public string Question { get; set; }
        
        public string Answer { get; set; }
    }
}
