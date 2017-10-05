using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("TipstaffSolicitorFirms")]
    public class SolicitorFirm
    {
        [DynamoDBHashKey]
        public string SolicitorFirmID { get; set; }
        
        public string FirmName { get; set; }
        
        public string AddressLine1 { get; set; }
        
        public string AddressLine2 { get; set; }
       
        public string AddressLine3 { get; set; }
       
        public string Town { get; set; }
        
        public string County { get; set; }
        
        public string Postcode { get; set; }
        
        public string DX { get; set; }
       
        public string PhoneDayTime { get; set; }
       
        public string PhoneOutofHours { get; set; }
       
        public string Email { get; set; }

        public bool Active { get; set; }

        public DateTime? Deactivated { get; set; }
        
        public string DeactivatedBy { get; set; }
    }
}
