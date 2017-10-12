using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Web.Mvc;
using System.Security;

namespace Tipstaff.Models
{
    public class Applicant : IModel
    {
        [Key]
        //public int ApplicantID { get; set; }
        public string ApplicantID { get; set; }
        [Required, Display(Name = "Title")]
        public int salutationID { get; set; }
        [Required, MaxLength(50), Display(Name = "Last name")]
        public string nameLast { get; set; }
        [MaxLength(50), Display(Name = "First name")]
        public string nameFirst { get; set; }
        [Required(ErrorMessage = "The first line of the address is mandatory"), MaxLength(100), Display(Name = "Address Line 1")]
        public string addressLine1 { get; set; }
        [MaxLength(100), Display(Name = "Address Line 2")]
        public string addressLine2 { get; set; }
        [MaxLength(100), Display(Name = "Address Line 3")]
        public string addressLine3 { get; set; }
        [MaxLength(100), Display(Name = "Town")]
        public string town { get; set; }
        [MaxLength(100), Display(Name = "County")]
        public string county { get; set; }
        [MaxLength(10), Display(Name = "Postcode")]
        [Required]
        public string postcode { get; set; }
        [MaxLength(20), Display(Name = "Phone")]
        public string phone { get; set; }

        [Required]
        //public int tipstaffRecordID { get; set; }
        public string tipstaffRecordID { get; set; }
        public virtual ChildAbduction childAbduction { get; set; }

        //public virtual TipstaffRecord tipstaffRecord { get; set; }
        //public virtual Salutation salutation { get; set; }

           public MemoryCollections.Salutation salutation { get; set; }

        [Display(Name = "Full name of Applicant")]
        public virtual string fullname
        {
            get
            {
                return string.Format("{0} {1} {2}", salutation.Detail ?? "", nameFirst, nameLast);
            }
        }

        [Display(Name = "Full name of Applicant")]
        public virtual string PoliceDisplayName
        {
            get
            {
                return string.Format("{0}, {1}", nameLast.ToUpper(), nameFirst).Replace("  ", " ");
            }
        }
        [Display(Name = "Address")]
        public virtual string printAddressMultiLine
        {
            get
            {
                List<string> popLines = new List<string>();
                foreach (var line in populatedLines)
                {
                    popLines.Add(SecurityElement.Escape(line));
                }
                return string.Join("<w:br/>", popLines.ToArray());
            }
        }
        [Display(Name = "Address")]
        public virtual string screenAddressMultiLine
        {
            get
            {
                List<string> popLines = populatedLines;
                return string.Join("<br />", popLines.ToArray());
            }
        }
        [Display(Name = "Address")]
        public virtual string screenAddressSingleLine
        {
            get
            {
                List<string> popLines = populatedLines;
                string result = string.Join(",", popLines.ToArray());
                return result;
            }
        }
        [Display(Name = "Address")]
        public virtual string PrintAddressSingleLine
        {
            get
            {
                List<string> popLines = new List<string>();
                foreach (var line in populatedLines)
                {
                    popLines.Add(SecurityElement.Escape(line));
                }
                return string.Join(", ", popLines.ToArray());
            }
        }

        private List<string> populatedLines
        {
            get
            {
                List<string> outputAddress = new List<string>();
                outputAddress.Add(addressLine1);
                if (addressLine2 != null) outputAddress.Add(addressLine2);
                if (addressLine3 != null) outputAddress.Add(addressLine3);
                if (town != null) outputAddress.Add(town);
                if (county != null) outputAddress.Add(county);
                if (postcode != null) outputAddress.Add(postcode);
                return outputAddress;
            }
        }
    }
    public class ListApplicantsByTipstaffRecord:IListByTipstaffRecord
    {
        //public int tipstaffRecordID { get; set; }
        public string tipstaffRecordID { get; set; }
        public Tipstaff.xPagedList<Applicant> Applicants { get; set; }
        public bool TipstaffRecordClosed { get; set; }
    }
    public class ApplicantCreationModel
    {
        //public int tipstaffRecordID { get; set; }
        public string tipstaffRecordID { get; set; }
        //public Applicant applicant { get; set; }
        public Applicant applicant { get; set; }
        public SelectList SalutationList { get; set; }
        public virtual TipstaffRecord tipstaffRecord { get; set; }

        public ApplicantCreationModel()
        {
            //SalutationList = new SelectList(myDBContextHelper.CurrentContext.Salutations.Where(x=>x.active==true).ToList(), "salutationID", "Detail");
            SalutationList = new SelectList(MemoryCollections.SalutationList.GetSalutationList().Where(x=>x.Active == 1));
        }
        public ApplicantCreationModel(string id)
        {
            tipstaffRecord = myDBContextHelper.CurrentContext.TipstaffRecord.Find(id);
            tipstaffRecordID = id;
            //SalutationList = new SelectList(myDBContextHelper.CurrentContext.Salutations.Where(x => x.active == true).ToList(), "salutationID", "Detail");
            SalutationList = new SelectList(MemoryCollections.SalutationList.GetSalutationList().Where(x => x.Active == 1), "SalutationID", "Detail");
        }
    }
    public class ApplicantEditModel
    {
        public Applicant applicant { get; set; }
        public SelectList SalutationList { get; set; }
        public ApplicantEditModel()
        {
            //SalutationList = new SelectList(myDBContextHelper.CurrentContext.Salutations.Where(x=>x.active==true).ToList(), "salutationID", "Detail");
            SalutationList = new SelectList(MemoryCollections.SalutationList.GetSalutationList().Where(x => x.Active == 1), "SalutationID", "Detail");
        }

    }
}