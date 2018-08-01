using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Web.UI;
using Tipstaff.MemoryCollections;
using Tipstaff.Presenters;
using TPLibrary.GuidGenerator;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class AttendanceNoteController : Controller
    {
        //private TipstaffDB db;//////= myDBContextHelper.CurrentContext;
        
        private readonly IAttendanceNotePresenter _attendanceNotePresenter;
        private readonly ITipstaffRecordPresenter _tipstaffRecordPresenter;
        private readonly IGuidGenerator _guidGenerator;

        public AttendanceNoteController(IAttendanceNotePresenter attendanceNotePresenter, ITipstaffRecordPresenter tipstaffRecordPresenter, IGuidGenerator guidGenerator)
        {
            _attendanceNotePresenter = attendanceNotePresenter;
            _tipstaffRecordPresenter = tipstaffRecordPresenter;
            _guidGenerator = guidGenerator;
        }
        
        [HttpGet]
        public ActionResult Create(string id)
        {
            AttendanceNoteCreation note = new AttendanceNoteCreation(DateTime.Now);
            ViewBag.AttendanceNoteCodes = AttendanceNoteCodeList.GetAttendanceNoteCodeList().Where(x => x.Active == 1);
            ////////ViewBag.AttendanceNoteCodes = db.AttendanceNoteCodes.Where(x => x.active == true).ToList();
            //note.callDated = DateTime.Now;

            ////AttendanceNote.tipstaffRecord = db.TipstaffRecord.Find(id);
            note.tipstaffRecord = _tipstaffRecordPresenter.GetTipStaffRecord(id);
            ////////var tipstaffRecord = _tipstaffRecordRepository.GetEntityByHashKey(id);
            ////////AttendanceNote.tipstaffRecord = new TipstaffRecord() { resultID = tipstaffRecord.res}


            note.tipstaffRecordID = id;

            if (note.tipstaffRecord.caseStatus.Sequence > 3)
            {
                TempData["UID"] = note.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }
            return View(note);
        }

        [HttpPost]
        public ActionResult Create(AttendanceNoteCreation note)
        {
            note.callEnded = DateTime.Now;
            if (ModelState.IsValid)
            {
                ////db.AttendanceNotes.Add(AttendanceNote);
                ////db.SaveChanges();
                note.AttendanceNoteID = _guidGenerator.GenerateTimeBasedGuid().ToString();
                _attendanceNotePresenter.AddAttendanceNote(note);

               
               var area = _tipstaffRecordPresenter.GetTipStaffRecord(note.tipstaffRecordID);

               //// return RedirectToAction("Details", genericFunctions.TypeOfTipstaffRecord(AttendanceNote.tipstaffRecordID), new { id = AttendanceNote.tipstaffRecordID });
                return RedirectToAction("Details", area.Discriminator, new { id = note.tipstaffRecordID });

            }
            //////ViewBag.AttendanceNoteCodes = db.AttendanceNoteCodes.Where(x => x.active == true).ToList();
            ViewBag.AttendanceNoteCodes = AttendanceNoteCodeList.GetAttendanceNoteCodeList().Where(x => x.Active == 1);
            note.tipstaffRecord = _tipstaffRecordPresenter.GetTipStaffRecord(note.tipstaffRecordID);
            return View(note);
        }

        //[OutputCache(Location = OutputCacheLocation.Server, Duration = 180)]
        public PartialViewResult ListAttendanceNotesByRecord(string id, int? page)
        { 
            TipstaffRecord w = _tipstaffRecordPresenter.GetTipStaffRecord(id);
            ListAttendanceNotesByTipstaffRecord model = new ListAttendanceNotesByTipstaffRecord();
            model.tipstaffRecordID = w.tipstaffRecordID;
            model.TipstaffRecordClosed = w.caseStatusID > 2;
            model.AttendanceNotes = _attendanceNotePresenter.GetAllById(id).OrderByDescending(p => p.callDated).ToXPagedList<AttendanceNoteCreation>(page ?? 1, 8);
            //w.AttendanceNotes.OrderByDescending(p => p.callDated).ToXPagedList<AttendanceNote>(page ?? 1, 8);
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