using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Web.UI;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Data.Entity;
using Tipstaff.Logger;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class ApplicantController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;
        private readonly ITelemetryLogger _logger;
        
        public ApplicantController(ITelemetryLogger telemetryLogger)
        {
            _logger = telemetryLogger;
        }


        [OutputCache(Location = OutputCacheLocation.Server, Duration = 180)]
        public PartialViewResult ListApplicantsByRecord(int id, int? page)
        {
            ListApplicantsByTipstaffRecord model = new ListApplicantsByTipstaffRecord();
            try
            {
                ChildAbduction ca = db.ChildAbductions.Find(id);
                model.tipstaffRecordID = ca.tipstaffRecordID;
                model.Applicants = ca.Applicants.ToXPagedList<Applicant>(page ?? 1, 8);
                model.TipstaffRecordClosed = ca.caseStatus.sequence > 3;
            }
            catch
            {
                //do nothing!  Return empty model
            }
            return PartialView("_ListApplicantsByRecord", model);
        }
        public ActionResult Details(int id)
        {
            Applicant model = db.Applicants.Find(id);
            return View(model);
        }

        public ActionResult Create(int id)
        {
            ApplicantCreationModel model = new ApplicantCreationModel(id);
            if (model.tipstaffRecord.caseStatus.sequence > 3)
            {
                TempData["UID"] = model.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(ApplicantCreationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {

                ChildAbduction ca = db.ChildAbductions.Find(model.tipstaffRecordID);
                ca.Applicants.Add(model.applicant);
                db.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    string url = string.Format("window.location='{0}';", Url.Action("Details", "ChildAbduction", new { id = model.tipstaffRecordID }));
                    return JavaScript(url);
                }
                else
                {
                    return RedirectToAction("Details", "ChildAbduction", new { id = model.tipstaffRecordID });
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"DbUpdateException in ApplicantController in Create method, for user {((CPrincipal)User).UserID}");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in ApplicantController in Create method, for user {((CPrincipal)User).UserID}");
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = genericFunctions.GetLowestError(ex);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", errModel ?? null);
            }
        }

        public ActionResult Edit(int id)
        {
            ApplicantEditModel model = new ApplicantEditModel();
            model.applicant = db.Applicants.Find(id);
            if (model.applicant.childAbduction.caseStatus.sequence > 3)
            {
                TempData["UID"] = model.applicant.childAbduction.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ApplicantEditModel model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model.applicant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "ChildAbduction", new { id = model.applicant.tipstaffRecordID });
            }
            return View(model);
        }

        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            DeleteApplicant model = new DeleteApplicant(id);
            if (model.Applicant == null)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("Applicant {0} has been deleted, please raise a help desk call if you think this has been deleted in error.", id);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }

        //
        // POST: /ChildRelationship/Delete/5
        [HttpPost, ActionName("Delete"), AuthorizeRedirect(Roles = "Admin")]
        public ActionResult DeleteConfirmed(DeleteApplicant model)
        {
            model.Applicant = db.Applicants.Find(model.DeleteModelID);
            int tipstaffRecordID = model.Applicant.tipstaffRecordID;
            db.Applicants.Remove(model.Applicant);
            db.SaveChanges();
            //get the Audit Event we just created 
            string recDeleted = model.DeleteModelID.ToString();
            AuditEvent AE= db.AuditEvents.Where(a => a.auditEventDescription.AuditDescription == "Applicant deleted" && a.RecordChanged == recDeleted).OrderByDescending(a=>a.EventDate).Single();
            //add a deleted reason
            AE.DeletedReasonID = model.DeletedReasonID;
            //and save again
            db.SaveChanges();
            return RedirectToAction("Details", "ChildAbduction", new { id = tipstaffRecordID });
        }
    }
}
