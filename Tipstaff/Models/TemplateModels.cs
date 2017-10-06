using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Data.SqlClient;
using Tipstaff.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using Tipstaff.Infrastructure.Repositories;

namespace Tipstaff.Models
{
    public class CreateDocumentViewModel
    {
        public TipstaffRecord tipstaffRecord { get; set; }
        public IEnumerable<Template> TemplatesForRecordType { get; set; }
    }
    public class TemplateEdit
    {
        public Template Template { get; set; }
        public HttpPostedFileBase uploadFile { get; set; }
        public bool UploadSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public SelectList DiscriminatorType { get; set; }
        //public TemplateEdit(int id) : this()
        //{
        //    Template = myDBContextHelper.CurrentContext.Templates.Find(id);
        //}
        public TemplateEdit() 
        {
            DiscriminatorType = new SelectList(
                  new List<Object>{ 
                        new { value = "" , text = "Please select a value"}, 
                        new { value = "All" , text = "All"  },
                        new { value = "Warrant" , text = "Warrant"},
                        new { value = "ChildAbduction" , text = "Child Abduction"}
                  },
                  "value",
                  "text","");
            Template = new Template();
        }
    }

    public class Template
    {
        [Key]
        //public int templateID { get; set; }
        public string templateID { get; set; }
        [Required,MaxLength(128)]
        public string Discriminator { get; set; }
        [Required,MaxLength(80),Display(Name="Document Name")]
        public string templateName { get; set; }
        [Required]
        //public string templateXML { get; set; }
        public string filePath { get; set; }
        [Display(Name="Is addressee required?")]
        public bool addresseeRequired { get; set; }
        public bool active { get; set; }
        public DateTime? deactivated { get; set; }
        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }

    public class WordFile
    {
        public string fileName { get; set; }
        public string Path { get; set; }
        public string fullName { get; set; }
        public string tipstaffRecordID { get; set; }

        public WordFile() { } //empty constructor   

        public WordFile(Warrant warrant, string serverPath) //Constructor with variables
        {
            tipstaffRecordID = warrant.tipstaffRecordID;
            Path = string.Format(serverPath + "{0}", warrant.tipstaffRecordID);
            fileName = string.Format("SCD26Location-{0}.doc", warrant.UniqueRecordID);
            fullName = string.Format("{0}\\{1}", Path, fileName);
            //Ensure folder exists to create outoput
            //if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);
        }
        public WordFile(TipstaffRecord tipstaffRecord, string serverPath, Template template) //Constructor with variables
        {
            tipstaffRecordID = tipstaffRecord.tipstaffRecordID;
            Path = string.Format(serverPath + "{0}", tipstaffRecord.tipstaffRecordID);
            fileName = string.Format("{0}-{1}.doc", template.templateName, tipstaffRecord.UniqueRecordID);
            fullName = string.Format("{0}\\{1}", Path, fileName);
            //Ensure folder exists to create outoput
            //if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);
        }
        public WordFile(string tipstaffRecordID, string serverPath, string templateID, string templateName, string tipstaffURI)
        {
            Path = string.Format(serverPath + "{0}", tipstaffRecordID);
            fileName = string.Format("{0}-{1}.doc", templateName, tipstaffURI);
            fullName = string.Format("{0}\\{1}", Path, fileName);
        }

        public bool Exists
        {
            get
            {
                return System.IO.File.Exists(fullName);
            }
        }

        public string serverPath
        {
            get
            {
                return string.Format("~/Documents/{0}/{1}",tipstaffRecordID,fileName);
            }
        }
    }

}