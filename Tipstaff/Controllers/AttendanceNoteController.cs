using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using PagedList;

using Tipstaff.Models;
using System.Web.UI;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class AttendanceNoteController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;

        [HttpGet]
        public ActionResult Create(int id)
        {
            AttendanceNote AttendanceNote = new AttendanceNote(DateTime.Now);
            ViewBag.AttendanceNoteCodes = db.AttendanceNoteCodes.Where(x => x.active == true).ToList();
            AttendanceNote.tipstaffRecord = db.TipstaffRecord.Find(id);
            AttendanceNote.tipstaffRecordID = id;

            if (AttendanceNote.tipstaffRecord.caseStatus.sequence > 3)
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
                db.AttendanceNotes.Add(AttendanceNote);
                db.SaveChanges();
                return RedirectToAction("Details", genericFunctions.TypeOfTipstaffRecord(AttendanceNote.tipstaffRecordID), new { id = AttendanceNote.tipstaffRecordID });
            }
            ViewBag.AttendanceNoteCodes = db.AttendanceNoteCodes.Where(x => x.active == true).ToList();
            return View(AttendanceNote);
        }

        [OutputCache(Location = OutputCacheLocation.Server, Duration = 180)]
        public PartialViewResult ListAttendanceNotesByRecord(int id, int? page)
        {
            TipstaffRecord w = db.TipstaffRecord.Find(id);

            ListAttendanceNotesByTipstaffRecord model = new ListAttendanceNotesByTipstaffRecord();
            model.tipstaffRecordID = w.tipstaffRecordID;
            model.TipstaffRecordClosed = w.caseStatusID > 2;
            model.AttendanceNotes = w.AttendanceNotes.OrderByDescending(p => p.callDated).ToXPagedList<AttendanceNote>(page ?? 1, 8);
            return PartialView("_ListAttendanceNotesByRecord", model);
        }
        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            DeleteAttendanceNote model = new DeleteAttendanceNote(id);
            if (model.AttendanceNote == null)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("Attendance Note {0} has been deleted, please raise a help desk call if you think this has been deleted in error.", id);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            } 
            if (model.AttendanceNote.tipstaffRecord.caseStatus.sequence > 3)
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
            model.AttendanceNote = db.AttendanceNotes.Find(model.DeleteModelID);
            int tipstaffRecordID = model.AttendanceNote.tipstaffRecordID;
            string controller = genericFunctions.TypeOfTipstaffRecord(tipstaffRecordID);
            db.AttendanceNotes.Remove(model.AttendanceNote);
            db.SaveChanges();
            //get the Audit Event we just created 
            string recDeleted = model.DeleteModelID.ToString();
            AuditEvent AE = db.AuditEvents.Where(a => a.auditEventDescription.AuditDescription == "AttendanceNote deleted" && a.RecordChanged == recDeleted).OrderByDescending(a => a.EventDate).Take(1).Single();
            //add a deleted reason
            AE.DeletedReasonID = model.DeletedReasonID;
            //and save again
            db.SaveChanges();
            return RedirectToAction("Details", controller, new { id = tipstaffRecordID });
        }

    }
}