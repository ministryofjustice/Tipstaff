using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Web.UI;
using Tipstaff.Services.Repositories;
using Tipstaff.MemoryCollections;
using Tipstaff.Infrastructure.Services;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class AttendanceNoteController : Controller
    {
        //private TipstaffDB db;//////= myDBContextHelper.CurrentContext;
        private readonly IAttendanceNotesRepository _attendanceNotesRepository;
        private readonly ITipstaffRecordRepository _tipstaffRecordRepository;
        private readonly IGuidGenerator _guidGenerator;

        public AttendanceNoteController(IAttendanceNotesRepository attendanceNotesRepository, ITipstaffRecordRepository tipstaffRecordRepository, IGuidGenerator guidGenerator)
        {
            _attendanceNotesRepository = attendanceNotesRepository;
            _tipstaffRecordRepository = tipstaffRecordRepository;
            _guidGenerator = guidGenerator;
        }
        
        [HttpGet]
        public ActionResult Create(string id)
        {
            AttendanceNote AttendanceNote = new AttendanceNote(DateTime.Now);
            ViewBag.AttendanceNoteCodes = AttendanceNoteCodeList.GetAttendanceNoteCodeList().Where(x => x.Active == 1);
            ////////ViewBag.AttendanceNoteCodes = db.AttendanceNoteCodes.Where(x => x.active == true).ToList();


            ////AttendanceNote.tipstaffRecord = db.TipstaffRecord.Find(id);
            AttendanceNote.tipstaffRecord = GetTipstaffRecord(id);
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
                _attendanceNotesRepository.AddAttendanceNote(new Services.DynamoTables.AttendanceNote()
                {
                    CallDated = AttendanceNote.callDated,
                    CallDetails = AttendanceNote.callDetails,
                    CallEnded = AttendanceNote.callEnded,
                    CallStarted = AttendanceNote.callStarted,
                    TipstaffRecordID = AttendanceNote.tipstaffRecordID,
                    AttendanceNoteID = _guidGenerator.GenerateTimeBasedGuid().ToString(),
                    AttendanceNoteCode = AttendanceNoteCodeList.GetAttendanceNoteCodeList().FirstOrDefault(x => x.Id == AttendanceNote.AttendanceNoteCodeID).Detail
                });

               
               var area = _tipstaffRecordRepository.GetEntityByHashKey(AttendanceNote.tipstaffRecordID);

               //// return RedirectToAction("Details", genericFunctions.TypeOfTipstaffRecord(AttendanceNote.tipstaffRecordID), new { id = AttendanceNote.tipstaffRecordID });
                return RedirectToAction("Details", area.Discriminator, new { id = AttendanceNote.tipstaffRecordID });

            }

            
            //////ViewBag.AttendanceNoteCodes = db.AttendanceNoteCodes.Where(x => x.active == true).ToList();
            ViewBag.AttendanceNoteCodes = AttendanceNoteCodeList.GetAttendanceNoteCodeList().Where(x => x.Active == 1);
            return View(AttendanceNote);
        }

        [OutputCache(Location = OutputCacheLocation.Server, Duration = 180)]
        public PartialViewResult ListAttendanceNotesByRecord(string id, int? page)
        {
            //////TipstaffRecord w = db.TipstaffRecord.Find(id);
            TipstaffRecord w = GetTipstaffRecord(id);
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
            var dynamoEntity = _attendanceNotesRepository.GetAttendanceNote(model.DeleteModelID);
            model.AttendanceNote = new AttendanceNote()
            {
                 AttendanceNoteID = dynamoEntity.AttendanceNoteID,
                 tipstaffRecordID = dynamoEntity.TipstaffRecordID,
                 callDated = dynamoEntity.CallDated,
                 callDetails = dynamoEntity.CallDetails,
                 callEnded = dynamoEntity.CallEnded,
               //  callStarted = dynamoEntity.CallStarted,
            };

            var tipstaffRecordID = model.AttendanceNote.tipstaffRecordID;
            string controller = string.Empty;/////genericFunctions.TypeOfTipstaffRecord(tipstaffRecordID);
            //////db.AttendanceNotes.Remove(model.AttendanceNote);
            //////db.SaveChanges();
            // _attendanceNotesRepository.
            
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

        private TipstaffRecord GetTipstaffRecord(string id)
        {
            var record = _tipstaffRecordRepository.GetEntityByHashKey(id);

            var tipstaffRecord = new TipstaffRecord()
            {
                arrestCount = record.ArrestCount,
                NPO = record.NPO,
                createdBy = record.CreatedBy,  
                DateExecuted = record.DateExecuted,
                createdOn = record.CreatedOn,
                nextReviewDate = record.NextReviewDate,
                prisonCount = record.PrisonCount,
                tipstaffRecordID = int.Parse(record.TipstaffRecordID),
                resultEnteredBy = record.ResultEnteredBy,
                resultDate = record.ResultDate
            };

            return tipstaffRecord;
        }
    }
}