using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tipstaff.Models
{
    public class DocumentStatusListView : AdminListView
    {
        public IPagedList<DocumentStatus> DocumentStatuses { get; set; }
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
}