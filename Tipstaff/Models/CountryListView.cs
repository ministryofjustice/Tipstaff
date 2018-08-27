using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tipstaff.Models
{
    public class CountryListView : AdminListView
    {
        public IPagedList<Country> Countries { get; set; }
    }

    public class Country
    {
        [Key]
        public int countryID { get; set; }
        [Required, MaxLength(50), Display(Name = "Issuing Country")]
        public string Detail { get; set; }
        public bool active { get; set; }
        public DateTime? deactivated { get; set; }
        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }
}