using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("Contact")]
    public class Contact
    {
        [DynamoDBHashKey]
        public string ContactID { get; set; }

        public string salutation{ get; set; }
       
        public string firstName { get; set; }
        
        public string lastName { get; set; }
   
        public string addressLine1 { get; set; }
       
        public string addressLine2 { get; set; }
      
        public string addressLine3 { get; set; }
      
        public string town { get; set; }
       
        public string county { get; set; }
       
        public string postcode { get; set; }
      
        public string DX { get; set; }
       
        public string phoneHome { get; set; }
       
        public string phoneMobile { get; set; }
      
        public string email { get; set; }
       
        public string notes { get; set; }
     
        public string ContactType { get; set; }
    }
}
