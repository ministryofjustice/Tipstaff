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
        public int DeleteModelID { get; set; }
        [Display(Name="Reason for deletion")]
        public int DeletedReasonID { get; set; }
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
        public DeleteApplicant() { }
        public DeleteApplicant(int id)
        {
            Applicant = myDBContextHelper.CurrentContext.Applicants.Find(id);
            DeleteModelID = id;
        }
    }
    public class DeleteChild : DeleteModel
    {
        public Child Child { get; set; }
        public DeleteChild() { }
        public DeleteChild(int id)
        {
            Child = myDBContextHelper.CurrentContext.Children.Find(id);
            DeleteModelID = id;
        }
    }
	public class DeleteRespondent : DeleteModel
    {
        public Respondent Respondent { get; set; }
        public DeleteRespondent() { }
        public DeleteRespondent(int id)
        {
            Respondent = myDBContextHelper.CurrentContext.Respondents.Find(id);
            DeleteModelID = id;
        }
    }
	public class DeleteAddress : DeleteModel
    {
        public Address Address { get; set; }
        public DeleteAddress() { }
        public DeleteAddress(int id)
        {
            Address = myDBContextHelper.CurrentContext.Addresses.Find(id);
            DeleteModelID = id;
        }
    }
	public class DeleteAttendanceNote : DeleteModel
    {
        public AttendanceNote AttendanceNote { get; set; }
        public DeleteAttendanceNote() { }
        public DeleteAttendanceNote(int id)
        {
            AttendanceNote = myDBContextHelper.CurrentContext.AttendanceNotes.Find(id);
            DeleteModelID = id;
        }
    }
	public class DeleteDocument : DeleteModel
    {
        public Document Document { get; set; }
        public DeleteDocument() { }
        public DeleteDocument(int id)
        {
            Document = myDBContextHelper.CurrentContext.Documents.Find(id);
            DeleteModelID = id;
        }
    }
    public class DeleteTipstaffRecordSolicitor : DeleteModel
    {
        public TipstaffRecordSolicitor TipstaffRecordSolicitor { get; set; }
        public DeleteTipstaffRecordSolicitor() { }
        public DeleteTipstaffRecordSolicitor(int id)
        {
            TipstaffRecordSolicitor = myDBContextHelper.CurrentContext.TipstaffRecordSolicitors.Find(id);
            DeleteModelID = id;
        }
    }
}