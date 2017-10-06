using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.dto
{
    public class Tipstaff
    {
        
        public string TipstaffRecordID { get; set; }
        
        public string CreatedBy { get; set; }
        
        public DateTime CreatedOn { get; set; }
        
        public DateTime NextReviewDate { get; set; }
        
        public DateTime? ResultDate { get; set; }
        
        public DateTime? DateExecuted { get; set; }
        public int? ArrestCount { get; set; }
        public int? PrisonCount { get; set; }      
        public string ResultEnteredBy { get; set; }
        public string NPO { get; set; }

        public string ProtectiveMarking { get; set; }

        public string Result { get; set; }

        public string CaseStatus { get; set; }

        public IEnumerable<Address> addresses { get; set; }
    }
}
