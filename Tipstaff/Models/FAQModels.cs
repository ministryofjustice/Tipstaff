using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Tipstaff.Models
{
    public class FAQ
    {
        [Key]
        public string faqID { get; set; }
        [Required]
        public bool loggedInUser { get; set; }
        [Required, MaxLength(150)]
        public string question { get; set; }
        [Required, MaxLength(4000)]
        [DataType(DataType.MultilineText)]
        public string answer { get; set; }
    }
}