using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Tipstaff.Models
{
    public class SkinColour
    {
        [Key]
        public int skinColourID { get; set; }
        [Required, MaxLength(50)]
        public string Detail { get; set; }
        public bool active { get; set; }
        public DateTime? deactivated { get; set; }
        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }
}