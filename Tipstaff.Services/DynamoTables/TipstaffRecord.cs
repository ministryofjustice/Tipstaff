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
       
        public DateTime? OrderDated { get; set; }
        
        public DateTime? OrderReceived { get; set; }
        
        public string OfficerDealing { get; set; }
        
        public string EldestChild { get; set; }
        
        public string CAOrderType { get; set; }

        

        //End of ChildAbduction
    }
}
