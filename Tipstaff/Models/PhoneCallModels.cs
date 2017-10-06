using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;
using System.Web.Mvc;

namespace Tipstaff.Models
{
    public class ListAttendanceNotesByTipstaffRecord : IListByTipstaffRecord
    {
        public string tipstaffRecordID { get; set; }
        //public IEnumerable<AttendanceNote> AttendanceNotes { get; set; }
        public Tipstaff.xPagedList<AttendanceNote> AttendanceNotes { get; set; }
        public bool TipstaffRecordClosed { get; set; }
    }

    public class AttendanceNote
    {
        [Key]
        public string AttendanceNoteID { get; set; }
        [Required, Display(Name = "Date/Time")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime callDated { get; set; }
        //https://localhost:44399/AttendanceNote/Create/4037 
       // [Display(Name="Call started"),DisplayFormat(DataFormatString = "{0:t}", ApplyFormatInEditMode = true), UIHint("Time")]
        public DateTime callStarted { get; set; }
        [DisplayFormat(DataFormatString = "{0:t}", ApplyFormatInEditMode = true), UIHint("Time")]
        public DateTime? callEnded { get; set; }
        [Required, MaxLength(1000, ErrorMessage = "Maximum 1000 characters"), Display(Name = "Call details"), UIHint("TextAreaWithCountdown")]
        [AdditionalMetadata("maxLength", 1000)]
        public string callDetails { get; set; }
        [Display(Name = "Call type")]
        public int AttendanceNoteCodeID { get; set; }
        [Required]
        public string tipstaffRecordID { get; set; }
        [Display(Name = "Call type")]
        public virtual AttendanceNoteCode AttendanceNoteCode { get; set; }
        public virtual TipstaffRecord tipstaffRecord { get; set; }

        public AttendanceNote() { }
        public AttendanceNote(DateTime started)
        {
            callDated = started;
            callStarted = started;
        }
        public string Duration
        {
            get
            {
                TimeSpan duration = new TimeSpan();
                if (callEnded != null)
                {
                    DateTime end = callEnded == null ? (DateTime)callStarted : (DateTime)callEnded;
                    DateTime start = callStarted == null ? (DateTime)callStarted : (DateTime)callStarted;
                    duration = end - start;
                    return string.Format("{0:D2}:{1:D2}:{2:D2}", duration.Minutes.ToString("D2"), duration.Hours.ToString("D2"), duration.Seconds.ToString("D2"));
                }
                else
                {
                    return "Duraction acnnot be calculated due to null end date";
                }
            }
        }
    }

    public class AttendanceNoteCode
    {
        [Key]
        public int AttendanceNoteCodeID { get; set; }
        [Required, MaxLength(50), Display(Name = "Call Type")]
        public string Detail { get; set; }
        public bool active { get; set; }
        public DateTime? deactivated { get; set; }
        [MaxLength(50)]
        public string deactivatedBy { get; set; }

        public virtual ICollection<AttendanceNote> AttendanceNotes { get; set; }
    }
}