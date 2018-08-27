using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tipstaff.Models
{
    public class ContactTypeListView : AdminListView
    {
        public IPagedList<ContactType> ContactTypes { get; set; }
    }

    public class ContactType
    {
        [Key]
        public int contactTypeID { get; set; }
        [Required, MaxLength(50), Display(Name = "Contact Type")]
        public string Detail { get; set; }
        [ScaffoldColumn(false)]
        public bool active { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? deactivated { get; set; }
        [ScaffoldColumn(false), MaxLength(50)]
        public string deactivatedBy { get; set; }
    }
}