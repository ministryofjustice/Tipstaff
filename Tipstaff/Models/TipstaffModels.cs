using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Web.Mvc;
using System.Configuration;
using PagedList;
using System.Linq.Expressions;

namespace Tipstaff.Models
{
    #region Tipstaff Record Models

    public abstract class TipstaffRecord
    {
        [Key]
        public int tipstaffRecordID { get; set; }

        [Required, MaxLength(50), Display(Name = "Created by")]
        public string createdBy { get; set; }

        [Required, Display(Name = "Created on")]
        public DateTime createdOn { get; set; }

        [Required, Display(Name = "Protective Marking")]
        public int protectiveMarkingID { get; set; }

        [Display(Name = "Result")]
        public int? resultID { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Required, Display(Name = "Next review date"), FutureDate(ErrorMessage = "The review date must be in the future")]
        public DateTime nextReviewDate { get; set; }

        [Display(Name = "Date result entered")]
        public DateTime? resultDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}, ApplyFormatInEditMode = true"), Display(Name = "Date of Execution")]
        public DateTime? DateExecuted { get; set; }

        public int? arrestCount { get; set; }
        public int? prisonCount { get; set; }

        [MaxLength(50), Display(Name = "Result entered by")]
        public string resultEnteredBy { get; set; }

        public string NPO { get; set; }

        [Required, Display(Name = "Current case status")]
        public int caseStatusID { get; set; }

        public bool Retention { get; set; }

        [Display(Name = "Protective Marking")]
        public virtual ProtectiveMarking protectiveMarking { get; set; }

        public virtual Result result { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Passport> Passports { get; set; }
        public virtual ICollection<AttendanceNote> AttendanceNotes { get; set; }
        public virtual ICollection<CaseReview> caseReviews { get; set; }
        public virtual ICollection<TipstaffRecordSolicitor> LinkedSolicitors { get; set; }
        public virtual ICollection<Respondent> Respondents { get; set; }
        public virtual ICollection<Address> addresses { get; set; }
        public virtual CaseStatus caseStatus { get; set; }
        public virtual ICollection<TipstaffPoliceForce> policeForces { get; set; }

        public virtual string UniqueRecordID
        {
            get
            {
                string prefix = "TR";
                if (genericFunctions.isTipstaffRecordChildAbduction(this))
                {
                    prefix = "CA";
                }
                else
                {
                    prefix = ((Warrant)this).division.Prefix;
                }
                return string.Format("{0}{1}", prefix, tipstaffRecordID.ToString("D6"));
            }
        }

        public virtual string GetLastStatusChangeDetails
        {
            get
            {
                string recID = tipstaffRecordID.ToString();
                try
                {
                    var status = myDBContextHelper.CurrentContext.AuditEventRows.Where(x => x.ColumnName == "caseStatusID" && x.Now == "3" && x.auditEvent.RecordChanged == recID)
                                                    .OrderByDescending(a => a.auditEvent.EventDate).Take(1).SingleOrDefault();
                    if (status != null)
                    {
                        return string.Format("was closed by {0} on {1}", status.auditEvent.UserID, status.auditEvent.EventDate.ToShortDateString());
                    }
                    return "";
                }
                catch
                {
                    return "";
                }
            }
        }
    }

    public class ChildAbduction : TipstaffRecord
    {
        [Required, DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date to SCD26")]
        public DateTime? sentSCD26 { get; set; }

        [Required, Display(Name = "Date Order made"), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? orderDated { get; set; }

        [Required, Display(Name = "Date Order received"), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? orderReceived { get; set; }

        [Required, MaxLength(50), Display(Name = "Officer dealing")]
        public string officerDealing { get; set; }

        [MaxLength(50), Display(Name = "Eldest Child")]
        public string EldestChild { get; set; }

        //[Required,Display(Name = "Current case status")]
        //public int childAbductionCaseStatusID { get; set; }

        [Required, Display(Name = "Order Type")]
        public int caOrderTypeID { get; set; }

        [Display(Name = "Order Type")]
        public virtual CAOrderType caOrderType { get; set; }

        //public virtual ChildAbductionCaseStatus childAbductionCaseStatus { get; set; }
        [Display(Name = "Linked Children")]
        public virtual ICollection<Child> children { get; set; }

        //[Display(Name="Linked Respondents")]
        //public virtual ICollection<Respondent> Respondents { get; set; }
        [Display(Name = "Linked Applicants")]
        public virtual ICollection<Applicant> Applicants { get; set; }

        public virtual string ListOfChildNames
        {
            get
            {
                string result = string.Empty;

                if (this.children.Count() > 0)
                {
                    foreach (Child child in this.children)
                    {
                        result += " " + child.fullname;
                    }
                }
                else
                {
                    result = "No child records found";
                }

                return result;
            }
        }

        public virtual string MultiChildDescriptor
        {
            get { return this.children.Count() > 1 ? "children" : "child"; }
        }

        [MaxLength(50), Display(Name = "Court File Number")]
        public string CourtFileNumber { get; set; }

        //[NotMapped]
        //public virtual string EldestChild
        //{
        //    get
        //    {
        //        Tipstaff.Models.Child eldest = this.children.OrderByDescending(c => c.dateOfBirth).ThenBy(c => c.childID).Take(1).Single();
        //        return eldest.nameLast.ToUpper();
        //    }
        //    set { EldestChild = value; }
        //}
    }

    public class Warrant : TipstaffRecord
    {
        [Required, MaxLength(50), Display(Name = "Case number")]
        public string caseNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Required, Display(Name = "Expiry Date")]
        public DateTime? expiryDate { get; set; }

        [MaxLength(153), Display(Name = "Full name of respondent")]
        public string RespondentName { get; set; }

        [Required, Display(Name = "Division")]
        public int divisionID { get; set; }

        [Display(Name = "Division")]
        public virtual Division division { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Required, Display(Name = "DateCirculated")]
        public DateTime? DateCirculated { get; set; }
    }

    #endregion Tipstaff Record Models

    public class TipstaffNPO
    {
        public int tipstaffRecordID { get; set; }
        public string UniqueRecordID { get; set; }

        [Display(Name = "NBTC")]
        public string NPO { get; set; }
    }

    public class TipstaffRecordResolutionModel
    {
        public TipstaffRecord tipstaffRecord { get; set; }
        public int tipstaffRecordID { get; set; }
        public Result result { get; set; }

        [Display(Name = "Result Type")]
        public int resultID { get; set; }

        public SelectList resultList { get; set; }
        public Dictionary<int, string> prisonDict { get; set; }
        public Dictionary<int, string> arrestDict { get; set; }

        [Required, Display(Name = "Date of execution"), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? DateExecuted { get; set; }

        [Display(Name = "People committed to prison")]
        public int? pCount { get; set; }

        [Display(Name = "People arrested")]
        public int? aCount { get; set; }

        public TipstaffRecordResolutionModel(int TipstaffRecordID)
        {
            tipstaffRecord = myDBContextHelper.CurrentContext.TipstaffRecord.Find(TipstaffRecordID);
            tipstaffRecordID = TipstaffRecordID;
            resultList = new SelectList(myDBContextHelper.CurrentContext.Results.Where(r => r.active == true).ToList(), "resultID", "Detail");
            int respCount = tipstaffRecord.Respondents.Count();

            Dictionary<int, string> resp = new Dictionary<int, string>();
            for (int i = 0; i <= respCount; i++)
            {
                resp.Add(i, i.ToString());
            }
            this.prisonDict = resp;
            this.arrestDict = resp;
        }

        public TipstaffRecordResolutionModel()
        {
            resultList = new SelectList(myDBContextHelper.CurrentContext.Results.Where(r => r.active == true).ToList(), "resultID", "Detail");
        }
    }

    public class TipstaffCaseClosedDataModel
    {
        public ICollection<TipstaffRecord> TipstaffRecords { get; set; }

        public TipstaffCaseClosedDataModel()
        {
            TipstaffRecords = myDBContextHelper.CurrentContext.TipstaffRecord.Where(x => x.caseStatusID > 2 && x.resultID == null).ToList();
        }
    }

    public abstract class ListViewModel
    {
        public int page { get; set; }
        public string sortOrder { get; set; }

        [Display(Name = "Show resolved cases?")]
        public bool includeFinal { get; set; }

        public int caseStatusID { get; set; }
        public int TotalRecordCount { get; set; }
        public int FilteredRecordCount { get; set; }

        public ListViewModel()
        {
            page = 1;
            includeFinal = true;
        }
    }

    public class WarrantListViewModel : ListViewModel
    {
        public IPagedList<Warrant> Warrants { get; set; }
        public SelectList StatusList { get; set; }
        public string respondentNameContains { get; set; }
        public string caseNumberContains { get; set; }
        public int divisionID { get; set; }
        public SelectList DivisionList { get; set; }

        public WarrantListViewModel()
        {
            caseStatusID = -1;
            divisionID = -1;
            StatusList = new SelectList(myDBContextHelper.CurrentContext.CaseStatuses.Where(c => c.active == true), "CaseStatusID", "Detail");
            DivisionList = new SelectList(myDBContextHelper.CurrentContext.Divisions.Where(c => c.active == true), "DivisionID", "Detail");
        }
    }

    public class ChildAbductionListViewModel : ListViewModel
    {
        public IPagedList<ChildAbduction> ChildAbductions { get; set; }
        public SelectList StatusList { get; set; }
        public SelectList OrderTypeList { get; set; }
        public int caOrderTypeID { get; set; }
        public string childNameContains { get; set; }

        public ChildAbductionListViewModel()
        {
            caseStatusID = -1;
            caOrderTypeID = -1;
            StatusList = new SelectList(myDBContextHelper.CurrentContext.CaseStatuses.Where(c => c.active == true), "CaseStatusID", "Detail");
            OrderTypeList = new SelectList(myDBContextHelper.CurrentContext.CAOrderTypes.Where(c => c.active == true), "caOrderTypeID", "Detail");
        }
    }

    public class ListPNCIDsNPO
    {
        public TipstaffNPO npo { get; set; }
        public ICollection<Child> children { get; set; }
        public ICollection<Respondent> Respondents { get; set; }
    }

    #region Case Status Models

    public class CaseStatus
    {
        [Key]
        public int caseStatusID { get; set; }

        [Required, MaxLength(30), Display(Name = "Case Status")]
        public string Detail { get; set; }

        public bool active { get; set; }
        public int sequence { get; set; }
        public DateTime? deactivated { get; set; }

        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }

    //public class ChildAbductionCaseStatus
    //{
    //    [Key]
    //    public int childAbductionCaseStatusID { get; set; }
    //    [Required, MaxLength(30), Display(Name = "Case Status")]
    //    public string Detail { get; set; }
    //    public bool active { get; set; }
    //    public DateTime? deactivated { get; set; }
    //    [MaxLength(50)]
    //    public string deactivatedBy { get; set; }
    //}
    //public class WarrantCaseStatus
    //{
    //    [Key]
    //    public int warrantCaseStatusID { get; set; }
    //    [Required, MaxLength(30), Display(Name = "Case Status")]
    //    public string Detail { get; set; }
    //    public bool active { get; set; }
    //    public DateTime? deactivated { get; set; }
    //    [MaxLength(50)]
    //    public string deactivatedBy { get; set; }
    //}

    #endregion Case Status Models

    #region Lookup models

    public class Gender
    {
        [Key]
        public int genderID { get; set; }

        [Required, MaxLength(50)]
        public string detail { get; set; }

        public bool active { get; set; }
        public DateTime? deactivated { get; set; }

        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }

    public class FaxCode
    {
        [Key]
        public int faxCodeID { get; set; }

        [Required, MaxLength(50)]
        public string Detail { get; set; }

        public bool active { get; set; }
        public DateTime? deactivated { get; set; }

        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }

    public class ProtectiveMarking
    {
        [Key]
        public int protectiveMarkingID { get; set; }

        [Required, MaxLength(15), Display(Name = "Protective Marking")]
        public string Detail { get; set; }

        public bool active { get; set; }
        public DateTime? deactivated { get; set; }

        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }

    public class Result
    {
        [Key]
        public int resultID { get; set; }

        [Required, MaxLength(20), Display(Name = "Result")]
        public string Detail { get; set; }

        public bool active { get; set; }
        public DateTime? deactivated { get; set; }

        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }

    public class DocumentType
    {
        [Key]
        public int documentTypeID { get; set; }

        [Required, MaxLength(100), Display(Name = "Document Type")]
        public string Detail { get; set; }

        public bool active { get; set; }
        public DateTime? deactivated { get; set; }

        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }

    public class DocumentStatus
    {
        [Key]
        public int DocumentStatusID { get; set; }

        [Required, MaxLength(40), Display(Name = "Document Status")]
        public string Detail { get; set; }

        public bool active { get; set; }
        public DateTime? deactivated { get; set; }

        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }

    public class ChildRelationship
    {
        [Key]
        public int childRelationshipID { get; set; }

        [Required, MaxLength(40), Display(Name = "Child Relationship")]
        public string Detail { get; set; }

        public bool active { get; set; }
        public DateTime? deactivated { get; set; }

        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }

    public class Country
    {
        [Key]
        public int countryID { get; set; }

        [Required, MaxLength(50), Display(Name = "Issuing Country")]
        public string Detail { get; set; }

        public bool active { get; set; }
        public DateTime? deactivated { get; set; }

        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }

    public class Division
    {
        [Key]
        public int divisionID { get; set; }

        [Required, MaxLength(50), Display(Name = "Division")]
        public string Detail { get; set; }

        [Required]
        public string Prefix { get; set; }

        [Display(Name = "Active")]
        public bool active { get; set; }

        public DateTime? deactivated { get; set; }

        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }

    public class CAOrderType
    {
        [Key]
        public int caOrderTypeID { get; set; }

        [Required, MaxLength(50), Display(Name = "Order Type")]
        public string Detail { get; set; }

        [Display(Name = "Active")]
        public bool active { get; set; }

        public DateTime? deactivated { get; set; }

        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }

    public class Salutation
    {
        [Key]
        public int salutationID { get; set; }

        [Required, MaxLength(10), Display(Name = "Title")]
        public string Detail { get; set; }

        [Display(Name = "Active")]
        public bool active { get; set; }

        public DateTime? deactivated { get; set; }

        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }

    #endregion Lookup models
}