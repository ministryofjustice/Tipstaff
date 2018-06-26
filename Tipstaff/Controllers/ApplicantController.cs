using System;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Web.UI;
using System.Data.Entity.Infrastructure;
using Tipstaff.Presenters;
using TPLibrary.GuidGenerator;
using TPLibrary.Logger;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class ApplicantController : Controller
    {
        //private TipstaffDB db = myDBContextHelper.CurrentContext;
        private readonly IApplicantPresenter _applicantPresenter;
        private readonly ICloudWatchLogger _logger;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ITipstaffRecordPresenter _tipstaffRecordPresenter;

        public ApplicantController(ICloudWatchLogger telemetryLogger, IApplicantPresenter applicantPresenter, IGuidGenerator guidGenerator, ITipstaffRecordPresenter tipstaffRecordPresenter)
        {
            _logger = telemetryLogger;
            _applicantPresenter = applicantPresenter;
            _guidGenerator = guidGenerator;
            _tipstaffRecordPresenter = tipstaffRecordPresenter;
        }

        [OutputCache(Location = OutputCacheLocation.Server, Duration = 180)]
        public PartialViewResult ListApplicantsByRecord(string id, int? page)
        {
            ListApplicantsByTipstaffRecord model = new ListApplicantsByTipstaffRecord();
            try
            {
                TipstaffRecord tipstaff = _tipstaffRecordPresenter.GetTipStaffRecord(id);
                var applicants = _applicantPresenter.GetAllApplicantsByTipstaffRecordID(id);
                model.tipstaffRecordID = id;

                model.Applicants = applicants.ToXPagedList<Applicant>(page ?? 1, 8);
                model.TipstaffRecordClosed = (tipstaff.caseStatus.Detail == "File Closed" || tipstaff.caseStatus.Detail == "File Archived") ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in ApplicantController in ListApplicantsByRecord method, for user {((CPrincipal)User).UserID}");
            }

            return PartialView("_ListApplicantsByRecord", model);
        }

        public ActionResult Details(string id)
        {
            //Applicant model = db.Applicants.Find(id);
            Applicant model = _applicantPresenter.GetApplicant(id);
            return View(model);
        }

        public ActionResult Create(string id)
        {
            ApplicantCreationModel model = new ApplicantCreationModel(id);
            model.tipstaffRecord = _tipstaffRecordPresenter.GetTipStaffRecord(id);

            //if (model.tipstaffRecord.caseStatus.sequence > 3)
            if (model.tipstaffRecord.caseStatus.Detail == "File Closed" || model.tipstaffRecord.caseStatus.Detail == "File Archived")
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
                model.applicant.ApplicantID = _guidGenerator.GenerateTimeBasedGuid().ToString();
                _applicantPresenter.AddApplicant(model);

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

        public ActionResult Edit(string id)
        {
            ApplicantEditModel model = new ApplicantEditModel();
            model.applicant = _applicantPresenter.GetApplicant(id);
            TipstaffRecord tipstaff = _tipstaffRecordPresenter.GetTipStaffRecord(model.applicant.tipstaffRecordID);

            //if (model.applicant.childAbduction.caseStatus.sequence > 3)
            if (tipstaff.caseStatus.Detail == "File Closed" || tipstaff.caseStatus.Detail == "File Archived")
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
                _applicantPresenter.UpdateApplicant(model);
                return RedirectToAction("Details", "ChildAbduction", new { id = model.applicant.tipstaffRecordID });
            }
            return View(model);
        }

        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Delete(string id)
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
            ////IMPLEMENT AUDIT!!!
            ////get the Audit Event we just created 
            //string recDeleted = model.DeleteModelID.ToString();
            //AuditEvent AE= db.AuditEvents.Where(a => a.auditEventDescription.AuditDescription == "Applicant deleted" && a.RecordChanged == recDeleted).OrderByDescending(a=>a.EventDate).Single();
            ////add a deleted reason
            //AE.DeletedReasonID = model.DeletedReasonID;
            ////and save again
            //db.SaveChanges();
            string tipstaffRecordID = model.Applicant.tipstaffRecordID;
            _applicantPresenter.DeleteApplicant(model);
            return RedirectToAction("Details", "ChildAbduction", new { id = tipstaffRecordID });
        }
    }
}