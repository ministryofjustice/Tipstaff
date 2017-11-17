using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.DynamoTables
{
    [DynamoDBTable("Tipstaff_Address")]
    public class Address : DynamoTable
    {
        [DynamoDBRangeKey]
        public string TipstaffRecordId { get; set; }

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
