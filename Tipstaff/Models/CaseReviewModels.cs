﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Tipstaff.Models
{
    public class ListCaseReviewsByTipstaffRecord : IListByTipstaffRecord
    {
        public int tipstaffRecordID { get; set; }
        public Tipstaff.xPagedList<CaseReview> CaseReviews { get; set; }
        public bool TipstaffRecordClosed { get; set; }
    }
    public class OutstandingCaseReviewViewModel
    {
        public ICollection<TipstaffRecord> OverdueCaseReviews { get; set; }
        public ICollection<TipstaffRecord> DueTodayCaseReviews { get; set; }
        public ICollection<TipstaffRecord> DueWithinWeekCaseReviews { get; set; }
    }
    public class CaseReview
    {
        [Key]
        public int caseReviewID { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Reviewed")]
        public DateTime? reviewDate { get; set; }
        [MaxLength(800), DataType(DataType.MultilineText), Display(Name = "Action To Be Taken"), UIHint("TextAreaWithCountdown")]
        [AdditionalMetadata("maxLength", 800)]
        //Note: Multiline text for textarea on page
        public string actionTaken { get; set; }
        [Display(Name = "Case Review Status")]
        public int caseReviewStatusID { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Next Review Date")]
        public DateTime nextReviewDate { get; set; }
        public int tipstaffRecordID { get; set; }

        public virtual TipstaffRecord tipstaffRecord { get; set; }
        public virtual CaseReviewStatus caseReviewStatus { get; set; }
    }
    public class CaseReviewStatus
    {
        [Key]
        public int caseReviewStatusID { get; set; }
        [Required, MaxLength(20), Display(Name = "Case Review Status")]
        public string Detail { get; set; }
        public bool active { get; set; }
        public DateTime? deactivated { get; set; }
        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }
    public class CaseReviewCreation
    {
        public CaseReview CaseReview { get; set; }
        public SelectList CaseStatusList { get; set; }
        public SelectList CaseReviewStatusList { get; set; }
        [Required, Display(Name = "Case status")]
        public int CaseStatusID { get; set; }
        public CaseReviewCreation()
        {
            CaseReview = new CaseReview();
            CaseStatusList = new SelectList(myDBContextHelper.CurrentContext.CaseStatuses.Where(x => x.active == true && x.sequence <= 3).OrderBy(x => x.sequence).ToList(), "caseStatusID", "Detail");
            CaseReviewStatusList = new SelectList(myDBContextHelper.CurrentContext.CaseReviewStatuses.Where(c => c.active == true), "caseReviewStatusID", "Detail");
        }
    }
}