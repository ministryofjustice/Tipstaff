using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Tipstaff.Models
{
    public class Nationality
    {
        [Key]
        //public int nationalityID { get; set; }
        public string nationalityID { get; set; }
        [Required, MaxLength(50), Display(Name = "Nationality")]
        public string Detail { get; set; }
        public bool active { get; set; }
        public DateTime? deactivated { get; set; }
        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }
}