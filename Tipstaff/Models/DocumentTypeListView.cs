using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tipstaff.Models
{
    public class DocumentTypeListView : AdminListView
    {
        public IPagedList<DocumentType> DocumentTypes { get; set; }
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
}