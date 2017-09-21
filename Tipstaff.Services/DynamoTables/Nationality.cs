using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("Nationalities")]
    public class Nationality
    {
        [DynamoDBHashKey]
        public string NationalityId { get; set; }
        
        public string Detail { get; set; }

        public bool Active { get; set; }

        public DateTime? Deactivated { get; set; }

        public string DeactivatedBy { get; set; }
    }
}
