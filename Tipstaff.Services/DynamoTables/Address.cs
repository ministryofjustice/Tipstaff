using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("Tipstaff_Address")]
    public class Address
    {
        [DynamoDBHashKey]
        public string AddressID { get; set; }
        [DynamoDBRangeKey]
        public string TipstaffRecordID { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string Town { get; set; }

        public string County { get; set; }

        public string PostCode { get; set; }

        public string Phone { get; set; }

        public string AddresseeName { get; set; }
    }
}
