using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Tipstaff.Models
{
    public abstract class DeleteModel
    {
        public string DeleteModelID { get; set; }
        [Display(Name="Reason for deletion")]
        public MemoryCollections.DeletedReason DeletedReason { get; set; }
        public SelectList DeletedReasons { get; set; }
        public DeleteModel()
        {
            //DeletedReasons = new SelectList(myDBContextHelper.CurrentContext.DeletedReasons.Where(x => x.active == true).ToList(), "deletedReasonID", "Detail");
            DeletedReasons = new SelectList(MemoryCollections.DeletedReasonList.GetDeletedReasonList().Where(x => x.Active == 1).ToList(), "DeletedReasonID", "Detail");
        }
    }
    public class DeleteApplicant : DeleteModel
    {
        public Applicant Applicant { get; set; }
        public DeleteApplicant() {
            DeletedReasons = new SelectList(MemoryCollections.DeletedReasonList.GetDeletedReasonList().Where(x => x.Active == 1).ToList(), "DeletedReasonID", "Detail");
        }
        public DeleteApplicant(string id)
        {
            Applicant = myDBContextHelper.CurrentContext.Applicants.Find(id);
            DeleteModelID = id;
        }
    }
    public class DeleteChild : DeleteModel
    {
        public Child Child { get; set; }
        public DeleteChild()
        {
            DeletedReasons = new SelectList(MemoryCollections.DeletedReasonList.GetDeletedReasonList().Where(x => x.Active == 1).ToList(), "DeletedReasonID", "Detail");
        }
    }
	public class DeleteRespondent : DeleteModel
    {
        public Respondent Respondent { get; set; }
        public DeleteRespondent() { }
        public DeleteRespondent(string id)
        {
            Respondent = myDBContextHelper.CurrentContext.Respondents.Find(id);
            DeleteModelID = id;
        }
    }
	public class DeleteAddress : DeleteModel
    {
        public Address Address { get; set; }
        public DeleteAddress() {
            DeletedReasons = new SelectList(MemoryCollections.DeletedReasonList.GetDeletedReasonList().Where(x => x.Active == 1).ToList(), "DeletedReasonID", "Detail");
        }
        public DeleteAddress(string id)
        {
            Address = myDBContextHelper.CurrentContext.Addresses.Find(id);
            DeleteModelID = id;
        }
    }
	public class DeleteAttendanceNote : DeleteModel
    {
        public AttendanceNote AttendanceNote { get; set; }
        public DeleteAttendanceNote() { }
        public DeleteAttendanceNote(string id)
        {
            AttendanceNote = myDBContextHelper.CurrentContext.AttendanceNotes.Find(id);
            DeleteModelID = id;
        }
    }
	public class DeleteDocument : DeleteModel
    {
        public Document Document { get; set; }
        public DeleteDocument() {
            DeletedReasons = new SelectList(MemoryCollections.DeletedReasonList.GetDeletedReasonList().Where(x => x.Active == 1).ToList(), "DeletedReasonID", "Detail");
        }
        public DeleteDocument(string id)
        {
            //Document = myDBContextHelper.CurrentContext.Documents.Find(id);
            DeleteModelID = id;
        }

    }
    public class DeleteTipstaffRecordSolicitor : DeleteModel
    {
        public TipstaffRecordSolicitor TipstaffRecordSolicitor { get; set; }
        public DeleteTipstaffRecordSolicitor() { }
        public DeleteTipstaffRecordSolicitor(string id)
        {
            TipstaffRecordSolicitor = myDBContextHelper.CurrentContext.TipstaffRecordSolicitors.Find(id);
            DeleteModelID = id;
        }
    }
}