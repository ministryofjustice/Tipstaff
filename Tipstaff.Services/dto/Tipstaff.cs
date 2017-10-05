using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.dto
{
    public class Tipstaff
    {
        
        public int tipstaffRecordID { get; set; }
        
        public string createdBy { get; set; }
        
        public DateTime createdOn { get; set; }
        
        public int protectiveMarkingID { get; set; }
        
        public int? resultID { get; set; }
        
        public DateTime nextReviewDate { get; set; }
        
        public DateTime? resultDate { get; set; }
        
        public DateTime? DateExecuted { get; set; }
        public int? arrestCount { get; set; }
        public int? prisonCount { get; set; }      
        public string resultEnteredBy { get; set; }
        public string NPO { get; set; }
    
        public int caseStatusID { get; set; }

        public string protectiveMarking { get; set; }

        public string result { get; set; }

        public IEnumerable<Address> addresses { get; set; }
    }
}
