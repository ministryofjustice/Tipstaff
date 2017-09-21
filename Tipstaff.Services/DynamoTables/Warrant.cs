using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("Warrant")]
    public class Warrant
    {
        [DynamoDBHashKey]
        public int TipstaffRecordID { get; set; }

        [DynamoDBRangeKey]
        public string UniqueRecordID { get; set; }

        public string NPO { get; set; }

        public string CaseNumber { get; set; }
      
        public DateTime? ExpiryDate { get; set; }
        
        public string RespondentName { get; set; }
     
        public int DivisionID { get; set; }
      
        public DateTime? DateCirculated { get; set; }
    }
}
