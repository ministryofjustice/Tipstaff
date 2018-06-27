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
    public class TipstaffRecord
    {
        [Key]
        public string tipstaffRecordID { get; set; }
        [Required, MaxLength(50), Display(Name = "Created by")]
        public string createdBy { get; set; }
       // [Required, Display(Name = "Created on")]
        public DateTime? createdOn { get; set; }

        [Required, Display(Name = "Protective Marking")]
        public int protectiveMarkingID { get; set; }

        [Display(Name="Result")]
        public int? resultID { get; set; }


        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Required, Display(Name = "Next review date"),FutureDate(ErrorMessage="The review date must be in the future")]
        public DateTime? nextReviewDate { get; set; }
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

        //[Display(Name = "Protective Marking")]
        //public virtual ProtectiveMarking protectiveMarking { get; set; }
        [Display(Name = "Protective Marking")]
        public MemoryCollections.ProtectiveMarkings protectiveMarking { get; set; }

        //public virtual Result result { get; set; }
        public MemoryCollections.Result result { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
        public IEnumerable<AttendanceNote> AttendanceNotes { get; set; }
        public virtual IEnumerable<CaseReview> caseReviews { get; set; }
        public virtual ICollection<TipstaffRecordSolicitor> LinkedSolicitors { get; set; }
        public IEnumerable<Respondent> Respondents { get; set; }

        public IEnumerable<Address> addresses { get; set; }
        //public virtual CaseStatus caseStatus { get; set; }
        public MemoryCollections.CaseStatus caseStatus { get; set; }
        public virtual ICollection<TipstaffPoliceForce> policeForces { get; set; }

        public string Discriminator { get; set; }
        

        public string UniqueRecordID
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
                    if((this) is Warrant) //division.Prefix; 
                         prefix = ((Warrant)this).Division.Prefix;
                }
                //return string.Format("{0}{1}",prefix, tipstaffRecordID.ToString("D6"));
                return string.Format("{0}{1}", prefix, tipstaffRecordID);
            }
        }

        //////////public virtual string GetLastStatusChangeDetails
        //////////{
        //////////    get
        //////////    {
        //////////        string recID = tipstaffRecordID.ToString();
        //////////        try
        //////////        {
        //////////            var status = myDBContextHelper.CurrentContext.AuditEventRows.Where(x => x.ColumnName == "CaseStatusID" && x.Now == "3" && x.auditEvent.RecordChanged == recID)
        //////////                                            .OrderByDescending(a => a.auditEvent.EventDate).Take(1).SingleOrDefault();
        //////////            if (status != null)
        //////////            {
        //////////                return string.Format("was closed by {0} on {1}", status.auditEvent.UserID, status.auditEvent.EventDate.ToShortDateString());
        //////////            }
        //////////            return "";
        //////////        }
        //////////        catch
        //////////        {
        //////////            return "";
        //////////        }
        //////////    }
        //////////}
    }

    public class ChildAbduction : TipstaffRecord
    {
        [Required,DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date to SCD26")]
        public DateTime? sentSCD26 { get; set; }
        [Required,Display(Name = "Date Order made"),DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? orderDated { get; set; }
        [Required,Display(Name = "Date Order received"),DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? orderReceived { get; set; }
        [Required,MaxLength(50), Display(Name="Officer dealing")] 
        public string officerDealing { get; set; }
        [MaxLength(50), Display(Name="Eldest Child")]
        public string EldestChild { get; set; }
        //[Required,Display(Name = "Current case status")]
        //public int childAbductionCaseStatusID { get; set; }

        ////[Required,Display(Name = "Order Type")]
        ////public int caOrderTypeID { get; set; }
        //[Display(Name = "Order Type")]
        //public virtual CAOrderType caOrderType { get; set; }
        [Display(Name = "Order Type")]
        public MemoryCollections.CaOrderType caOrderType { get; set;}

        //public virtual ChildAbductionCaseStatus childAbductionCaseStatus { get; set; }
        [Display(Name="Linked Children")]
        public IEnumerable<Child> children { get; set; }
        //[Display(Name="Linked Respondents")]
        //public virtual ICollection<Respondent> Respondents { get; set; }
        [Display(Name = "Linked Applicants")]
        public IEnumerable<Applicant> Applicants { get; set; }


        public string ListOfChildNames
        {
            get
            {
                string result=string.Empty;

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
        public  string MultiChildDescriptor
        {
            get { return this.children.Count() > 1 ? "children" : "child"; }
        }
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

    public class Warrant: TipstaffRecord
    {
        [Required,MaxLength(50), Display(Name = "Case number")]
        public string caseNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Required,Display(Name = "Expiry Date")]
        public DateTime? expiryDate { get; set; }
        [MaxLength(153), Display(Name = "Full name of respondent")]
        public string RespondentName { get; set; }
        [Required, Display(Name = "Division")]
        public MemoryCollections.Division Division { get; set; }
        //[Display(Name = "Division")]
        //public virtual Division division { get; set; }
        ////[Display(Name = "Division")]
        ////public MemoryCollections.Division division { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Circulated")]
        public DateTime? DateCirculated { get; set; }

        public int RespondentsCount
        {
            get { return Respondents.Count(); }
            set { }
        }
    }
    #endregion

    public class TipstaffNPO
    {
        public string tipstaffRecordID { get; set; }
        public string UniqueRecordID { get; set; }
        public string NPO { get; set; }
    }

    public class TipstaffRecordResolutionModel
    {
        public TipstaffRecord tipstaffRecord { get; set; }
        public string tipstaffRecordID { get; set; }
        public MemoryCollections.Result result { get; set; }

        [Display(Name="Result Type")]
        public int resultID { get; set; }
        public SelectList resultList { get; set; }
        public Dictionary<int, string> prisonDict { get; set; }
        public Dictionary<int, string> arrestDict { get; set; }
        [Required, Display(Name="Date of execution"),DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? DateExecuted { get; set; }
        [Display(Name="People committed to prison")]
        public int? pCount { get; set; }
        [Display(Name = "People arrested")]
        public int? aCount { get; set; }

        public TipstaffRecordResolutionModel()
        {
            resultList = new SelectList(MemoryCollections.ResultsList.GetResultList().Where(r => r.Active == 1), "ResultID", "Detail");
        }
        
    }

    public class TipstaffCaseClosedDataModel
    {
        public IEnumerable<TipstaffRecord> TipstaffRecords { get; set; }
       
    }

    public abstract class ListViewModel
    {
        public int page { get; set; }
        public string sortOrder { get; set; }
        [Display(Name="Show resolved cases?")]
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
        public SelectList  StatusList { get; set; }
        public string respondentNameContains { get; set; }
        public string caseNumberContains { get; set; }
        public int divisionID { get; set; }
        public SelectList DivisionList { get; set; }

        public WarrantListViewModel()
        {
            caseStatusID = -1;
            divisionID = -1;
            //StatusList = new SelectList(myDBContextHelper.CurrentContext.CaseStatuses.Where(c => c.active == true), "CaseStatusID", "Detail");
            //DivisionList = new SelectList(myDBContextHelper.CurrentContext.Divisions.Where(c => c.active == true), "DivisionID", "Detail");
            StatusList = new SelectList(MemoryCollections.CaseStatusList.GetCaseStatusList().Where(c => c.Active == 1), "CaseStatusID", "Detail");
            DivisionList = new SelectList(MemoryCollections.DivisionsList.GetResultList().Where(c => c.Active == 1), "DivisionID", "Detail");
        }
    }

    public class ChildAbductionListViewModel : ListViewModel
    {
        public IPagedList<ChildAbduction> ChildAbductions{ get; set; }
        public SelectList StatusList { get; set; }
        public SelectList OrderTypeList { get; set; }
        public int caOrderTypeID { get; set; }
        public string childNameContains { get; set; }
        public ChildAbductionListViewModel()
        {
            caseStatusID = -1;
            caOrderTypeID = -1;
            //StatusList = new SelectList(myDBContextHelper.CurrentContext.CaseStatuses.Where(c => c.active == true), "CaseStatusID", "Detail");
            //OrderTypeList = new SelectList(myDBContextHelper.CurrentContext.CAOrderTypes.Where(c => c.active == true), "caOrderTypeID", "Detail");
            OrderTypeList = new SelectList(MemoryCollections.CaOrderTypeList.GetOrderTypeList().Where(c => c.Active == 1), "CAOrderTypeID", "Detail");
            StatusList = new SelectList(MemoryCollections.CaseStatusList.GetCaseStatusList().Where(c => c.Active == 1), "CaseStatusID", "Detail");
        }

    }

    public class ListPNCIDsNPO
    {
        public TipstaffNPO npo { get; set; }
        public ICollection<Child> children { get; set; }
        public ICollection<Respondent> Respondents { get; set; }
    }

    #region Case Status Models
    //public class CaseStatus
    //{
    //    [Key]
    //    public int caseStatusID { get; set; }
    //    [Required, MaxLength(30), Display(Name = "Case Status")]
    //    public string Detail { get; set; }
    //    public bool active { get; set; }
    //    public int sequence { get; set; }
    //    public DateTime? deactivated { get; set; }
    //    [MaxLength(50)]
    //    public string deactivatedBy { get; set; }
    //}
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
    #endregion
    #region Lookup models
    //public class Gender
    //{
    //    [Key]
    //    public int genderID { get; set; }
    //    [Required,MaxLength(50)]
    //    public string Detail { get; set; }
    //    public bool active { get; set; }
    //    public DateTime? deactivated { get; set; }
    //    [MaxLength(50)]
    //    public string deactivatedBy { get; set; }
    //}
    //public class FaxCode
    //{
    //    [Key]
    //    public int faxCodeID { get; set; }
    //    [Required, MaxLength(50)]
    //    public string Detail { get; set; }
    //    public bool active { get; set; }
    //    public DateTime? deactivated { get; set; }
    //    [MaxLength(50)]
    //    public string deactivatedBy { get; set; }
    //}
    //public class ProtectiveMarking
    //{
    //    [Key]
    //    public int protectiveMarkingID { get; set; }
    //    [Required,MaxLength(15),Display(Name="Protective Marking")]
    //    public string Detail { get; set; }
    //    public bool active { get; set; }
    //    public DateTime? deactivated { get; set; }
    //    [MaxLength(50)]
    //    public string deactivatedBy { get; set; }
    //}
    //public class Result
    //{
    //    [Key]
    //    public int resultID { get; set; }
    //    [Required,MaxLength(20),Display(Name="Result")]
    //    public string Detail { get; set; }
    //    public bool active { get; set; }
    //    public DateTime? deactivated { get; set; }
    //    [MaxLength(50)]
    //    public string deactivatedBy { get; set; }
    //}
    //public class DocumentType
    //{
    //    [Key]
    //    public int documentTypeID { get; set; }
    //    [Required, MaxLength(100), Display(Name = "Document Type")]
    //    public string Detail { get; set; }
    //    public bool active { get; set; }
    //    public DateTime? deactivated { get; set; }
    //    [MaxLength(50)]
    //    public string deactivatedBy { get; set; }
    //}
    //public class DocumentStatus
    //{
    //    [Key]
    //    public int DocumentStatusID { get; set; }
    //    [Required, MaxLength(40), Display(Name = "Document Status")]
    //    public string Detail { get; set; }
    //    public bool active { get; set; }
    //    public DateTime? deactivated { get; set; }
    //    [MaxLength(50)]
    //    public string deactivatedBy { get; set; }
    //}
    //public class ChildRelationship
    //{
    //    [Key]
    //    public int childRelationshipID { get; set; }
    //    [Required, MaxLength(40), Display(Name = "Child Relationship")]
    //    public string Detail { get; set; }
    //    public bool active { get; set; }
    //    public DateTime? deactivated { get; set; }
    //    [MaxLength(50)]
    //    public string deactivatedBy { get; set; }
    //}
    //public class Division
    //{
    //    [Key]
    //    public int divisionID { get; set; }
    //    [Required, MaxLength(50), Display(Name = "Division")]
    //    public string Detail { get; set; }
    //    [Required]
    //    public string Prefix { get; set; }
    //    [Display(Name="Active")]
    //    public bool active { get; set; }
    //    public DateTime? deactivated { get; set; }
    //    [MaxLength(50)]
    //    public string deactivatedBy { get; set; }
    //}
    //public class CAOrderType
    //{
    //    [Key]
    //    public int caOrderTypeID { get; set; }
    //    [Required, MaxLength(50), Display(Name = "Order Type")]
    //    public string Detail { get; set; }
    //    [Display(Name="Active")]
    //    public bool active { get; set; }
    //    public DateTime? deactivated { get; set; }
    //    [MaxLength(50)]
    //    public string deactivatedBy { get; set; }
    //}
    //public class Salutation
    //{
    //    [Key]
    //    public int salutationID { get; set; }
    //    [Required, MaxLength(10), Display(Name = "Title")]
    //    public string Detail { get; set; }
    //    [Display(Name="Active")]
    //    public bool active { get; set; }
    //    public DateTime? deactivated { get; set; }
    //    [MaxLength(50)]
    //    public string deactivatedBy { get; set; }
    //}
    #endregion

}