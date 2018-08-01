using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tipstaff.MemoryCollections
{
    public class CaseReviewStatus
    {
        [Display(Name = "Case Review Status")]
        public int CaseReviewStatusId { get; set; }

        public string Detail { get; set; }

        public int Active { get; set; }
    }

    public class CaseReviewStatusList
    {
        public static List<CaseReviewStatus> GetCaseReviewStatusList()
        {
            return new List<CaseReviewStatus>()
            {
                new CaseReviewStatus() {  CaseReviewStatusId=1 ,  Detail = "to be reviewed",  Active = 1  },
                new CaseReviewStatus() {  CaseReviewStatusId=2 ,  Detail = "File Closed",  Active = 1  },
                new CaseReviewStatus() {  CaseReviewStatusId=3 ,  Detail = "File Archived",  Active = 1  }
            };
        }

        public static CaseReviewStatus GetCaseReviewStatusByDetail(string c)
        {
            return GetCaseReviewStatusList().FirstOrDefault(x => x.Detail == c);
        }

        public static CaseReviewStatus GetCaseReviewStatusByID(int id)
        {
            return GetCaseReviewStatusList().FirstOrDefault(x => x.CaseReviewStatusId == id);
        }
    }
}