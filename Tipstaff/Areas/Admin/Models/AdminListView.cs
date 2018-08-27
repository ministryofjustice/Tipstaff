using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tipstaff.Areas.Admin.Models
{
    public class AdminListView
    {
        public bool onlyActive { get; set; }
        public int page { get; set; }
        public string detailContains { get; set; }
        public string sortOrder { get; set; }

        public AdminListView()
        {
            onlyActive = true;
            page = 1;
            detailContains = "";
        }
    }

    public class ChildRelationshipListView : AdminListView
    {
        public IPagedList<ChildRelationship> ChildRelationships { get; set; }
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
}