using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("Tipstaff_CaseReview")]
    public class CaseReview : DynamoTable
    {
        [DynamoDBRangeKey]
        public string TipstaffRecordID { get; set; }

        public DateTime ReviewDate { get; set; }

        public string ActionTaken { get; set; }

        public string CaseReviewStatus { get; set; }

        public DateTime NextReviewDate { get; set; }
    }
}
