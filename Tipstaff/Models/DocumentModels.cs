using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Tipstaff.Models;

namespace Tipstaff.Models
{
    public class Document
    {
        [Key]
        public int documentID { get; set; }
        [Required(ErrorMessage = "Document Reference must be completed"), MaxLength(60)]
        [Display(Name = "Document Reference number")]
        public string documentReference { get; set; }
        [Display(Name = "Country of Origin")]
        public int? countryID { get; set; }
        [Required, Display(Name = "Nationality")]
        public int? nationalityID { get; set; }
        [Required, Display(Name = "Document Status")]
        public int documentStatusID { get; set; }
        [Required, Display(Name = "Document Type")]
        public int documentTypeID { get; set; }
        public int? templateID { get; set; }
        [ScaffoldColumn(false),Display(Name="Created on")]
        public DateTime createdOn { get; set; }
        [ScaffoldColumn(false), MaxLength(50), Display(Name="Created by")]
        public string createdBy { get; set; }
        public int tipstaffRecordID { get; set; }
        [ScaffoldColumn(false)]
        public byte[] binaryFile { get; set; }
        [ScaffoldColumn(false), MaxLength(256)]
        public string fileName { get; set; }
        [ScaffoldColumn(false), MaxLength(300)]
        public string mimeType { get; set; }
        public virtual Country country { get; set; }
        public virtual Nationality nationality { get; set; }
        public virtual DocumentStatus documentStatus { get; set; }
        public virtual DocumentType documentType { get; set; }
        public virtual Template template { get; set; }
        public virtual TipstaffRecord tipstaffRecord { get; set; }
        // public virtual TipstaffRecord tipstaffRecord { get; set; }
        public string CreationData
        {
            get
            {
                return string.Format("{0} by {1}", createdOn.ToString("d MMM yy @ HH:mm"), createdBy);
            }
        }
    }
    public class ListDocumentsByTipstaffRecord :IListByTipstaffRecord
    {
        public int tipstaffRecordID { get; set; }
        public Tipstaff.xPagedList<Document> Documents { get; set; }
        public bool TipstaffRecordClosed { get; set; }
    }

    public class ListPassportsByTipstaffRecord : IListByTipstaffRecord
    {
        public int tipstaffRecordID { get; set; }
        public Tipstaff.xPagedList<Passport> Passports { get; set; }
        public bool TipstaffRecordClosed { get; set; }
    }

    public class DocumentUploadModel
    {
        public int tipstaffRecordID { get; set; }
        public TipstaffRecord tipstaffRecord { get; set; }
        public HttpPostedFileBase uploadFile { get; set; }
        public Document document { get; set; }
        public SelectList CountryList { get; set; }
        public SelectList NationalityList { get; set; }
        [Required]
        public SelectList StatusList { get; set; }
        public SelectList TypeList { get; set; }
        //public List<DocumentStatus> Statuses { get; set; }
        //public List<DocumentType> Types { get; set; }
        public DocumentUploadModel()
        {
            CountryList = new SelectList(myDBContextHelper.CurrentContext.IssuingCountries.Where(x => x.active == true).ToList(), "countryID", "Detail");
            StatusList = new SelectList(myDBContextHelper.CurrentContext.DocumentStatuses.Where(x => x.active == true).Where(s => s.Detail != "Generated").ToList(), "DocumentStatusID", "Detail");
            TypeList = new SelectList(myDBContextHelper.CurrentContext.DocumentTypes.Where(x => x.active == true).Where(t => t.Detail != "Generated").ToList(), "documentTypeID", "Detail");
            NationalityList = new SelectList(myDBContextHelper.CurrentContext.Nationalities.Where(x => x.active == true).ToList(), "nationalityID", "Detail");
        }
    }
    public class ChooseAddresseeModel
    {
        public TipstaffRecord tipstaffRecord { get; set; }
        public IEnumerable<TipstaffRecordSolicitor> SolicitorsOnRecord { get; set; }
        public IEnumerable<Applicant> Applicants { get; set; }
        public Template template { get; set; }
        public int solicitorID { get; set; }
        public int applicantID { get; set; }
    }

    public class Passport : Document
    {
        [MaxLength(250), Display(Name ="Comments")]
        public string comments { get; set; }
        [Display(Name = "Passport Reference number")]
        public string passportReference { get; set; }
    }

    public class PassportUploadModel : DocumentUploadModel
    {
        public Passport passport { get; set; }
    }

    public class PassportEditModel
    {
        public Passport passport { get; set; }
        public PassportEditModel()
        {
            SalutationList = new SelectList(myDBContextHelper.CurrentContext.Salutations.Where(x => x.active == true).ToList(), "salutationID", "Detail");
        }

    }
}