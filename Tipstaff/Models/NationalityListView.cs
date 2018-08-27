using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tipstaff.Models
{
    public class NationalityListView : AdminListView
    {
        public IPagedList<Nationality> Nationalities { get; set; }
    }

    public class Nationality
    {
        [Key]
        public int nationalityID { get; set; }
        [Required, MaxLength(50), Display(Name = "Nationality")]
        public string Detail { get; set; }
        public bool active { get; set; }
        public DateTime? deactivated { get; set; }
        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }
}