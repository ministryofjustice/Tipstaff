using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tipstaff.MemoryCollections
{
    public class CaseStatus
    {
        public int CaseStatusId { get; set; }

        public string Detail { get; set; }

        public int Active { get; set; }

        public int Sequence { get; set; }
    }

    public class CaseStatusList
    {
        public static List<CaseStatus> GetCaseStatusList()
        {
            return new List<CaseStatus>()
            {
                new CaseStatus() {  CaseStatusId=1 ,  Detail = "Awaiting Information",  Active = 1,  Sequence = 1  },
                new CaseStatus() {  CaseStatusId=2 ,  Detail = "Active",                Active = 1,  Sequence = 2  },
                new CaseStatus() {  CaseStatusId=3 ,  Detail = "File Closed",           Active = 1,  Sequence = 4  },
                new CaseStatus() {  CaseStatusId=4 ,  Detail = "File Archived",         Active = 1,  Sequence = 5  },
                new CaseStatus() {  CaseStatusId=5 ,  Detail = "Stayed",                Active = 1,  Sequence = 3  }
            };
        }
    }
}