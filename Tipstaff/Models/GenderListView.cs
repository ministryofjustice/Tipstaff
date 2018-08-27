using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tipstaff.Models
{
    public class GenderListView : AdminListView
    {
        public IPagedList<Gender> Genders { get; set; }
    }

    public class Gender
    {
        [Key]
        public int genderID { get; set; }
        [Required, MaxLength(50)]
        public string Detail { get; set; }
        public bool active { get; set; }
        public DateTime? deactivated { get; set; }
        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }
}