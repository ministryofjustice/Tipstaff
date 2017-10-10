using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("Tipstaff_PoliceForces")]
    public class PoliceForces
    {
        [DynamoDBHashKey]
        public string PoliceForceID { get; set; }

        public bool LoggedInUser { get; set; }

        public string PoliceForceName { get; set; }
        
        public string PoliceForceEMail { get; set; }
        
        public bool Active { get; set; }

        public DateTime Deactivated { get; set; }

        public string DeactivatedBy { get; set; }

    }
}
