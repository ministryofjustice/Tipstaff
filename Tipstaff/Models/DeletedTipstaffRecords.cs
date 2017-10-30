using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using Tipstaff.MemoryCollections;

namespace Tipstaff.Models
{
    public class DeleteChildAbductionViewModel : DeleteTRViewModel
    {
        public ChildAbduction ChildAbduction { get; set; }
        public DeleteChildAbductionViewModel()
        {
            deletedTipstaffRecord.discriminator = "ChildAbduction";
        }
    }
    public class DeleteWarrantViewModel : DeleteTRViewModel
    {
        public Warrant Warrant{ get; set; }
        public DeleteWarrantViewModel()
        {
            deletedTipstaffRecord.discriminator = "Warrant";
        }
    }


    public abstract class DeleteTRViewModel
    {
        public DeletedTipstaffRecord deletedTipstaffRecord { get; set; }
        public SelectList DeletedReasonList { get; set; }
        public DeleteTRViewModel()
        {
            //DeletedReasonList = new SelectList(myDBContextHelper.CurrentContext.DeletedReasons.Where(x => x.active == true).ToList(), "deletedReasonID", "Detail");
            DeletedReasonList = new SelectList(MemoryCollections.DeletedReasonList.GetDeletedReasonList().Where(x => x.Active == 1).ToList(), "DeletedReasonID", "Detail");
            deletedTipstaffRecord = new DeletedTipstaffRecord();
        }
    }

    public class DeletedTipstaffRecord
    {
        [Key, Column(Order = 0)]
        public string TipstaffRecordID { get; set; }
        [Key, Column(Order = 1)]
        [Display(Name="Reason for Deletion")]
        public int deletedReasonID { get; set; }
        [Required, MaxLength(50)]
        public string discriminator { get; set; }
        [Required, MaxLength(10)]
        public string UniqueRecordID { get; set; }
        public DeletedReason deletedReason { get; set; }
    }

    //public class DeletedReason
    //{
    //    public int deletedReasonID { get; set; }
    //    [Required, MaxLength(50), Display(Name = "Deleted Reason")]
    //    public string Detail { get; set; }
    //    [Display(Name = "Active")]
    //    public bool active { get; set; }
    //    public DateTime? deactivated { get; set; }
    //    [MaxLength(50)]
    //    public string deactivatedBy { get; set; }
    //}
}