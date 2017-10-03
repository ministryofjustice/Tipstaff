using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("Templates")]
    public class Template
    {
        [DynamoDBHashKey]
        public string templateID { get; set; }

        public string Discriminator { get; set; }
        
        public string templateName { get; set; }
        
        public string filePath { get; set; }

        public bool addresseeRequired { get; set; }

        public bool active { get; set; }

        public DateTime? deactivated { get; set; }
        
        public string deactivatedBy { get; set; }
    }
}
