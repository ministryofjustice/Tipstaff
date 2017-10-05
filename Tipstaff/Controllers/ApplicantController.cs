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
using Tipstaff.Services.Repositories;
using Tipstaff.Infrastructure.Services;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class ApplicantController : Controller
    {
        //private TipstaffDB db = myDBContextHelper.CurrentContext;
        private readonly IApplicantRepository _applicantRepository;
        private readonly ICloudWatchLogger _logger;
        private readonly ITipstaffRecordRepository _tipstaffRecordRepository;
        private readonly IGuidGenerator _guidGenerator;

        public ApplicantController(ICloudWatchLogger telemetryLogger, IApplicantRepository applicantRepository, 
            ITipstaffRecordRepository tipstaffRecordRepository, IGuidGenerator guidGenerator)
        {
            _logger = telemetryLogger;
            _applicantRepository = applicantRepository;
            _tipstaffRecordRepository = tipstaffRecordRepository;
            _guidGenerator = guidGenerator;
        }


        [OutputCache(Location = OutputCacheLocation.Server, Duration = 180)]
        public PartialViewResult ListApplicantsByRecord(string id, int? page)
        {
            ListApplicantsByTipstaffRecord model = new ListApplicantsByTipstaffRecord();
            //try
            //{
            //    ChildAbduction ca = db.ChildAbductions.Find(id);
            //    model.tipstaffRecordID = ca.tipstaffRecordID;
            //    model.Applicants = ca.Applicants.ToXPagedList<Applicant>(page ?? 1, 8);
            //    model.TipstaffRecordClosed = ca.caseStatus.sequence > 3;
            //}
            //catch
            //{
            //    //do nothing!  Return empty model
            //}
            var tipstaff = _tipstaffRecordRepository.GetEntityByHashKey(id);
            model.tipstaffRecordID = id;
            model.Applicants = _applicantRepository.GetAllApplicantsByTipstaffRecordID(id).ToXPagedList<Tipstaff.Services.DynamoTables.Applicant>(page ?? 1, 8);
            model.TipstaffRecordClosed = (tipstaff.CaseStatus == "File Closed" || tipstaff.CaseStatus == "File Archived") ? true : false;

            return PartialView("_ListApplicantsByRecord", model);
        }
        public ActionResult Details(string id)
        {
            //Applicant model = db.Applicants.Find(id);
            var model = _applicantRepository.GetApplicant(id);
            return View(model);
        }

        public ActionResult Create(string id)
        {
            ApplicantCreationModel model = new ApplicantCreationModel(id);
            var tipstaff = _tipstaffRecordRepository.GetEntityByHashKey(id);

            //if (model.tipstaffRecord.caseStatus.sequence > 3)
            if (tipstaff.CaseStatus == "File Closed" || tipstaff.CaseStatus == "File Archived")
            {
                TempData["UID"] = "CA" + tipstaff.TipstaffRecordID; ;// model.tipstaffRecord.UniqueRecordID;
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

                //ChildAbduction ca = db.ChildAbductions.Find(model.tipstaffRecordID);
                //ca.Applicants.Add(model.applicant);
                //db.SaveChanges();
                string aid = (model.applicant.ApplicantID == null) ? _guidGenerator.GenerateTimeBasedGuid().ToString() : model.applicant.ApplicantID;
                _applicantRepository.AddApplicant(new Services.DynamoTables.Applicant()
                {
                    ApplicantID = aid,
                    NameFirst = model.applicant.nameFirst,
                    NameLast = model.applicant.nameLast,
                    AddressLine1 = model.applicant.addressLine1,
                    AddressLine2 = model.applicant.addressLine2,
                    AddressLine3 = model.applicant.addressLine3,
                    Town = model.applicant.town,
                    County = model.applicant.county,
                    Postcode = model.applicant.postcode,
                    Phone = model.applicant.phone,
                    Salutation = model.applicant.salutation.Detail,
                    TipstaffRecordID = model.tipstaffRecordID
                });
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
            var app = _applicantRepository.GetApplicant(id);
            //model.applicant = db.Applicants.Find(id);
            model.applicant = new Applicant
            {
                ApplicantID = app.ApplicantID,
                nameFirst = app.NameFirst,
                nameLast = app.NameLast,
                addressLine1 = app.AddressLine1,
                addressLine2 = app.AddressLine2,
                addressLine3 = app.AddressLine3,
                town = app.Town,
                county = app.County,
                postcode = app.Postcode,
                phone = app.Phone,
                //Salutation = model.applicant.salutation.Detail,
                tipstaffRecordID = app.TipstaffRecordID
            };
            var tipstaff = _tipstaffRecordRepository.GetEntityByHashKey(app.TipstaffRecordID);

            //if (model.applicant.childAbduction.caseStatus.sequence > 3)
            if (tipstaff.CaseStatus == "File Closed" || tipstaff.CaseStatus == "File Archived")
            {
                TempData["UID"] = "CA" + tipstaff.TipstaffRecordID; //This is not entirely correct. They use a numeric ID to identify records
                                //model.applicant.childAbduction.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ApplicantEditModel model)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(model.applicant).State = EntityState.Modified;
                //db.SaveChanges();
                _applicantRepository.Update(new Services.DynamoTables.Applicant()
                {
                    ApplicantID = model.applicant.ApplicantID,
                    NameFirst = model.applicant.nameFirst,
                    NameLast = model.applicant.nameLast,
                    AddressLine1 = model.applicant.addressLine1,
                    AddressLine2 = model.applicant.addressLine2,
                    AddressLine3 = model.applicant.addressLine3,
                    Town = model.applicant.town,
                    County = model.applicant.county,
                    Postcode = model.applicant.postcode,
                    Phone = model.applicant.phone,
                    Salutation = model.applicant.salutation.Detail,
                    TipstaffRecordID = model.applicant.tipstaffRecordID
                });
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
            //model.Applicant = db.Applicants.Find(model.DeleteModelID);
            //db.Applicants.Remove(model.Applicant);
            //db.SaveChanges();
            ////get the Audit Event we just created 
            //string recDeleted = model.DeleteModelID.ToString();
            //AuditEvent AE= db.AuditEvents.Where(a => a.auditEventDescription.AuditDescription == "Applicant deleted" && a.RecordChanged == recDeleted).OrderByDescending(a=>a.EventDate).Single();
            ////add a deleted reason
            //AE.DeletedReasonID = model.DeletedReasonID;
            ////and save again
            //db.SaveChanges();
            string tipstaffRecordID = model.Applicant.tipstaffRecordID;
            _applicantRepository.Delete(new Services.DynamoTables.Applicant()
            {
                ApplicantID = model.Applicant.ApplicantID,
                NameFirst = model.Applicant.nameFirst,
                NameLast = model.Applicant.nameLast,
                AddressLine1 = model.Applicant.addressLine1,
                AddressLine2 = model.Applicant.addressLine2,
                AddressLine3 = model.Applicant.addressLine3,
                Town = model.Applicant.town,
                County = model.Applicant.county,
                Postcode = model.Applicant.postcode,
                Phone = model.Applicant.phone,
                Salutation = model.Applicant.salutation.Detail,
                TipstaffRecordID = model.Applicant.tipstaffRecordID
            });
            return RedirectToAction("Details", "ChildAbduction", new { id = tipstaffRecordID });
        }
    }
}
