using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Tipstaff.Models;

namespace Tipstaff.Models
{
    public class Passport
    {
        [Key]
        public int passportID { get; set; }
        [Required(ErrorMessage = "Passport Reference must be completed"), MaxLength(60)]
        [Display(Name = "Passport Reference number")]
        public string passportReference { get; set; }
        [Display(Name = "Country of Origin")]
        public int? countryID { get; set; }
        [Required, Display(Name = "Nationality")]
        public int? nationalityID { get; set; }
        [Required, Display(Name = "Passport Status")]
        public int documentStatusID { get; set; }
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
        [MaxLength(1000), Display(Name = "Comments")]
        public string comments { get; set; }
        public string CreationData
        {
            get
            {
                return string.Format("{0} by {1}", createdOn.ToString("d MMM yy @ HH:mm"), createdBy);
            }
        }
    }
    public class ListPassportsByTipstaffRecord :IListByTipstaffRecord
    {
        public int tipstaffRecordID { get; set; }
        public Tipstaff.xPagedList<Passport> Passports { get; set; }
        public bool TipstaffRecordClosed { get; set; }
    }

    public class PassportUploadModel
    {
        public int tipstaffRecordID { get; set; }
        public Passport passport { get; set; }
        public HttpPostedFileBase uploadFile { get; set; }
        public SelectList CountryList { get; set; }
        public SelectList NationalityList { get; set; }
        [Required]
        public SelectList StatusList { get; set; }
        public SelectList TypeList { get; set; }
        public virtual TipstaffRecord tipstaffRecord { get; set; }
        public bool initial { get; set; }
        public PassportUploadModel()
        {
            CountryList = new SelectList(myDBContextHelper.CurrentContext.IssuingCountries.Where(x => x.active == true).ToList(), "countryID", "Detail");
            StatusList = new SelectList(myDBContextHelper.CurrentContext.DocumentStatuses.Where(x => x.active == true).Where(s => s.Detail != "Generated").ToList(), "DocumentStatusID", "Detail");
            NationalityList = new SelectList(myDBContextHelper.CurrentContext.Nationalities.Where(x => x.active == true).ToList(), "nationalityID", "Detail");
        }
        public PassportUploadModel(int id)
        {
            tipstaffRecord = myDBContextHelper.CurrentContext.TipstaffRecord.Find(id);
            tipstaffRecordID = id;
            CountryList = new SelectList(myDBContextHelper.CurrentContext.IssuingCountries.Where(x => x.active == true).ToList(), "countryID", "Detail");
            StatusList = new SelectList(myDBContextHelper.CurrentContext.DocumentStatuses.Where(x => x.active == true).Where(s => s.Detail != "Generated").ToList(), "DocumentStatusID", "Detail");
            NationalityList = new SelectList(myDBContextHelper.CurrentContext.Nationalities.Where(x => x.active == true).ToList(), "nationalityID", "Detail");
        }
    }
}