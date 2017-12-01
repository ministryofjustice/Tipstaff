using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("Tipstaff_TipstaffRecords")]
    public class TipstaffRecord : DynamoTable
    {
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? ProtectiveMarkingId { get; set; }

        public string Discriminator { get; set; }

        public int? ResultId { get; set; }

        public DateTime? NextReviewDate { get; set; }

        public DateTime? ResultDate { get; set; }

        public DateTime? DateExecuted { get; set; }

        public int? ArrestCount { get; set; }

        public int? PrisonCount { get; set; }

        public string ResultEnteredBy { get; set; }

        public string NPO { get; set; }

        public int? CaseStatusId { get; set; }

        public int? DivisionId { get; set; }
        
        //Child Abduction
        public DateTime? SentSCD26 { get; set; }

        public DateTime? OrderDated { get; set; }

        public DateTime? OrderReceived { get; set; }

        public string OfficerDealing { get; set; }

        public string EldestChild { get; set; }

        public int? CAOrderTypeId { get; set; }

        public string CaseNumber { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string RespondentName { get; set; }

        public DateTime? DateCirculated { get; set; }

        //End of ChildAbduction
    }
}
