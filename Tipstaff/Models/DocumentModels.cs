using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace Tipstaff.Models
{
    public class Document:IModel
    {
        [Key]
        public string documentID { get; set; }
        [Required(ErrorMessage = "Document Reference must be completed"), MaxLength(60)]
        [Display(Name = "Document Reference number")]
        public string documentReference { get; set; }
        [Display(Name = "Country of Origin")]
        public MemoryCollections.Country country { get; set; }
        [Required, Display(Name = "Nationality")]
        public MemoryCollections.Nationality nationality { get; set; }
        [Required, Display(Name = "Document Status")]
        public MemoryCollections.DocumentStatus documentStatus { get; set; }
        [Required, Display(Name = "Document Type")]
        public MemoryCollections.DocumentType documentType { get; set; }
        //public int? templateID { get; set; }
        public string templateID { get; set; }
        [Display(Name="Created on")]
        public DateTime createdOn { get; set; }
        [ Display(Name="Created by")]
        public string createdBy { get; set; }
        public string tipstaffRecordID { get; set; }
        
        public string fileName { get; set; }
        public string filePath { get; set; }
        [ScaffoldColumn(false), MaxLength(60)]
        public string mimeType { get; set; }
        //public virtual Country country { get; set; }
        //public virtual Nationality nationality { get; set; }
        //public virtual DocumentType documentType { get; set; }
        //public virtual DocumentStatus documentStatus { get; set; }
        
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
        public string tipstaffRecordID { get; set; }
        public Tipstaff.xPagedList<Document> Documents { get; set; }
        public bool TipstaffRecordClosed { get; set; }
    }

    public class DocumentUploadModel
    {
        //public int tipstaffRecordID { get; set; }
        public string tipstaffRecordID { get; set; }
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
            //CountryList = new SelectList(myDBContextHelper.CurrentContext.IssuingCountries.Where(x => x.active == true).ToList(), "countryID", "Detail");
            //NationalityList = new SelectList(myDBContextHelper.CurrentContext.Nationalities.Where(x => x.active == true).ToList(), "nationalityID", "Detail");
            //TypeList = new SelectList(myDBContextHelper.CurrentContext.DocumentTypes.Where(x => x.active == true).Where(t => t.Detail != "Generated").ToList(), "documentTypeID", "Detail");
            //StatusList = new SelectList(myDBContextHelper.CurrentContext.DocumentStatuses.Where(x => x.active == true).Where(s => s.Detail != "Generated").ToList(), "DocumentStatusID", "Detail");
            CountryList = new SelectList(MemoryCollections.CountryList.GetCountryList().Where(x => x.Active == 1).ToList(), "CountryID", "Detail");
            NationalityList = new SelectList(MemoryCollections.NationalityList.GetNationalityList().Where(x => x.Active == 1).ToList(), "NationalityID", "Detail");
            StatusList = new SelectList(MemoryCollections.DocumentStatusList.GetDocumentStatusList().Where(x => x.Active == 1).Where(s => s.Detail != "Generated").ToList(), "DocumentStatusID", "Detail");
            TypeList = new SelectList(MemoryCollections.DocumentTypeList.GetDocumentTypeList().Where(x => x.Active == 1).Where(t => t.Detail != "Generated").ToList(), "DocumentTypeID", "Detail");
        }
    }
    public class ChooseAddresseeModel
    {
        public TipstaffRecord tipstaffRecord { get; set; }
        public IEnumerable<TipstaffRecordSolicitor> SolicitorsOnRecord { get; set; }
        public IEnumerable<Applicant> Applicants { get; set; }
        public Template template { get; set; }
        public int solicitorID { get; set; }
        public string applicantID { get; set; }
    }
}