using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tipstaff.Models
{
    public class SalutationListView : AdminListView
    {
        public IPagedList<Salutation> Salutations { get; set; }
    }

    public class Salutation
    {
        [Key]
        public int salutationID { get; set; }
        [Required, MaxLength(10), Display(Name = "Title")]
        public string Detail { get; set; }
        [Display(Name = "Active")]
        public bool active { get; set; }
        public DateTime? deactivated { get; set; }
        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }
}