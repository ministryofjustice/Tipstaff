using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("Tipstaff_TipstaffRecord")]
    public class TipstaffRecord : DynamoTable
    {
        [DynamoDBRangeKey]
        public string TrackItem { get; set; }

        public string CreatedBy { get; set; }
     
        public DateTime? CreatedOn { get; set; }
       
        public string ProtectiveMarking { get; set; }
      
        public string Discriminator { get; set; }

        public string Result { get; set; }

        public DateTime? NextReviewDate { get; set; }
    
        public DateTime? ResultDate { get; set; }
        
        public DateTime? DateExecuted { get; set; }

        public int? ArrestCount { get; set; }

        public int? PrisonCount { get; set; }
 
        public string ResultEnteredBy { get; set; }

        public string NPO { get; set; }
        
        public string CaseStatus { get; set; }

        public string Division { get; set; }




        //Child Abduction
        public DateTime? SentSCD26 { get; set; }
        //[Required, Display(Name = "Date Order made"), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? OrderDated { get; set; }
        //[Required, Display(Name = "Date Order received"), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? OrderReceived { get; set; }
        //[Required, MaxLength(50), Display(Name = "Officer dealing")]
        public string OfficerDealing { get; set; }
        //[MaxLength(50), Display(Name = "Eldest Child")]
        public string EldestChild { get; set; }
        //[Required,Display(Name = "Current case status")]
        //public int childAbductionCaseStatusID { get; set; }

        //[Required, Display(Name = "Order Type")]
        public string CAOrderType { get; set; }

        

        //End of ChildAbduction
    }
}
