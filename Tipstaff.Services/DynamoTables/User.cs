using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("User")]
    public class User
    {
        [DynamoDBHashKey]
        public string UserId { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }
        
        public DateTime LastActive { get; set; }
        
        public string RoleStrength { get; set; }

        public string Role { get; set; }

    }
}
