using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tipstaff.Models
{
    public class DeletionReasonsListView : AdminListView
    {
        public IPagedList<DeletedReasonModel> DeletionReasons { get; set; }
    }

    public class DeletedReasonModel
    {
        public int deletedReasonID { get; set; }
        [Required, MaxLength(50), Display(Name = "Deleted Reason")]
        public string Detail { get; set; }
        [Display(Name = "Active")]
        public bool active { get; set; }
        public DateTime? deactivated { get; set; }
        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }
}