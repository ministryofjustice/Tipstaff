using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Web.UI;
using Tipstaff.MemoryCollections;
using Tipstaff.Presenters;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class AttendanceNoteController : Controller
    {
        //private TipstaffDB db;//////= myDBContextHelper.CurrentContext;
        
        private readonly IAttendanceNotePresenter _attendanceNotePresenter;

        public AttendanceNoteController(IAttendanceNotePresenter attendanceNotePresenter)
        {
            _attendanceNotePresenter = attendanceNotePresenter;
        }
        
        [HttpGet]
        public ActionResult Create(string id)
        {
            AttendanceNote AttendanceNote = new AttendanceNote(DateTime.Now);
            ViewBag.AttendanceNoteCodes = AttendanceNoteCodeList.GetAttendanceNoteCodeList().Where(x => x.Active == 1);
            ////////ViewBag.AttendanceNoteCodes = db.AttendanceNoteCodes.Where(x => x.active == true).ToList();


            ////AttendanceNote.tipstaffRecord = db.TipstaffRecord.Find(id);
            AttendanceNote.tipstaffRecord = _attendanceNotePresenter.GetTipStaffRecord(id);
            ////////var tipstaffRecord = _tipstaffRecordRepository.GetEntityByHashKey(id);
            ////////AttendanceNote.tipstaffRecord = new TipstaffRecord() { resultID = tipstaffRecord.res}


            AttendanceNote.tipstaffRecordID = id;

            if (AttendanceNote.tipstaffRecord.caseStatus.Sequence > 3)
            {
                TempData["UID"] = AttendanceNote.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }
            return View(AttendanceNote);
        }

        [HttpPost]
        public ActionResult Create(AttendanceNote AttendanceNote)
        {
            AttendanceNote.callEnded = DateTime.Now;
            if (ModelState.IsValid)
            {
                ////db.AttendanceNotes.Add(AttendanceNote);
                ////db.SaveChanges();
                _attendanceNotePresenter.AddAttendanceNote(AttendanceNote);

               
               var area = _attendanceNotePresenter.GetTipStaffRecord(AttendanceNote.tipstaffRecordID);

               //// return RedirectToAction("Details", genericFunctions.TypeOfTipstaffRecord(AttendanceNote.tipstaffRecordID), new { id = AttendanceNote.tipstaffRecordID });
                return RedirectToAction("Details", area.Descriminator, new { id = AttendanceNote.tipstaffRecordID });

            }

            
            //////ViewBag.AttendanceNoteCodes = db.AttendanceNoteCodes.Where(x => x.active == true).ToList();
            ViewBag.AttendanceNoteCodes = AttendanceNoteCodeList.GetAttendanceNoteCodeList().Where(x => x.Active == 1);
            return View(AttendanceNote);
        }

        [OutputCache(Location = OutputCacheLocation.Server, Duration = 180)]
        public PartialViewResult ListAttendanceNotesByRecord(string id, int? page)
        {
            //////TipstaffRecord w = db.TipstaffRecord.Find(id);
            TipstaffRecord w = _attendanceNotePresenter.GetTipStaffRecord(id);
            ListAttendanceNotesByTipstaffRecord model = new ListAttendanceNotesByTipstaffRecord();
            model.tipstaffRecordID = w.tipstaffRecordID;
            model.TipstaffRecordClosed = w.caseStatusID > 2;
            model.AttendanceNotes = w.AttendanceNotes.OrderByDescending(p => p.callDated).ToXPagedList<AttendanceNote>(page ?? 1, 8);
            return PartialView("_ListAttendanceNotesByRecord", model);
        }

        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            DeleteAttendanceNote model = new DeleteAttendanceNote(id);
            if (model.AttendanceNote == null)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("Attendance Note {0} has been deleted, please raise a help desk call if you think this has been deleted in error.", id);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            } 
            if (model.AttendanceNote.tipstaffRecord.caseStatus.Sequence > 3)
            {
                TempData["UID"] = model.AttendanceNote.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }

            return View(model);
        }

        //
        // POST: /AttendanceNote/Delete/5

        [HttpPost, ActionName("Delete"), AuthorizeRedirect(Roles = "Admin")]
        public ActionResult DeleteConfirmed(DeleteAttendanceNote model)
        {
            //////model.AttendanceNote = db.AttendanceNotes.Find(model.DeleteModelID);
            
            model.AttendanceNote = _attendanceNotePresenter.GetAttendanceNote(model.DeleteModelID);

            var tipstaffRecordID = model.AttendanceNote.tipstaffRecordID;
            string controller = string.Empty;/////genericFunctions.TypeOfTipstaffRecord(tipstaffRecordID);
            //////db.AttendanceNotes.Remove(model.AttendanceNote);
            //////db.SaveChanges();
            // _attendanceNotesRepository.
            _attendanceNotePresenter.DeleteAttendanceNote(model.AttendanceNote);
            
            //REVISIT AUDIT EVENTS!!!!
            //get the Audit Event we just created 
            //////////////string recDeleted = model.DeleteModelID.ToString();
            //////////////AuditEvent AE = db.AuditEvents.Where(a => a.auditEventDescription.AuditDescription == "AttendanceNote deleted" && a.RecordChanged == recDeleted).OrderByDescending(a => a.EventDate).Take(1).Single();
            ////////////////add a deleted reason
            //////////////AE.DeletedReasonID = model.DeletedReasonID;
            ////////////////and save again
            //////////////db.SaveChanges();
            return RedirectToAction("Details", controller, new { id = tipstaffRecordID });
        }
        
    }
}