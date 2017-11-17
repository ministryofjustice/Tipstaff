using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.DynamoTables
{
    [DynamoDBTable("Tipstaff_User")]
    public class User : DynamoTable
    {
        public string Name { get; set; }
        
        public string DisplayName { get; set; }
        
        public DateTime? LastActive { get; set; }
        
        public int RoleStrength { get; set; }
       
        public string Role { get; set; }
    }
}
