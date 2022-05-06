using System.Linq;
using System.Web.Mvc;
using Tipstaff.Models;
using System;
using System.Web.UI;
using TPLibrary.Logger;
using System.Data;
using System.Data.Entity;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class PassportController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;

        private readonly ICloudWatchLogger _logger;

        public PassportController(ICloudWatchLogger logger)
        {
            _logger = logger;
        }

        public ActionResult Details(int id)
        {
            Passport model = db.Passports.Find(id);
            return View(model);
        }

        [HttpGet]
        public ActionResult Upload(int id, bool initial = false)
        {
            PassportUploadModel model = new PassportUploadModel(id);
            model.tipstaffRecordID = id;
            model.tipstaffRecord = db.TipstaffRecord.Find(id);
            if (model.tipstaffRecord.caseStatus.sequence > 3)
            {
                TempData["UID"] = model.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }
            model.initial = initial;
            return View(model);
        }
        [HttpPost]
        public ActionResult Upload(PassportUploadModel model)
        {
            User user = db.GetUserByLoginName(User.Identity.Name.Split('\\').Last());
            model.passport.createdBy = user.DisplayName;
            model.passport.createdOn = DateTime.Now;
            model.passport.tipstaffRecordID = model.tipstaffRecordID;
            if (model.uploadFile != null)
            {
                var stream = model.uploadFile.InputStream;
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                model.passport.binaryFile = buffer;
                model.passport.fileName = System.IO.Path.GetFileName(model.uploadFile.FileName);
                model.passport.mimeType = model.uploadFile.ContentType;
            }
            if (ModelState.IsValid)
            {
                TipstaffRecord tr = db.TipstaffRecord.Find(model.tipstaffRecordID);
                tr.Passports.Add(model.passport);
                db.SaveChanges();
                return RedirectToAction("Details", genericFunctions.TypeOfTipstaffRecord(tr), new { id = model.tipstaffRecordID });
            }
            return View(model);
        }

        // GET: /Passport/Edit/5
        public ActionResult Edit(int id)
        {
            PassportUploadModel model = new PassportUploadModel();
            model.passport = db.Passports.Find(id);
            return View(model);
        }

        //
        // POST: /Passport/Edit/5
        [HttpPost]
        public ActionResult Edit(PassportUploadModel model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model.passport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", genericFunctions.TypeOfTipstaffRecord(model.passport.tipstaffRecordID), new { id = model.passport.tipstaffRecordID });
            }
            return View(model);
        }

        public PartialViewResult ListPassportsByRecord(int id, int? page)
        {
            TipstaffRecord w = db.TipstaffRecord.Find(id);

            ListPassportsByTipstaffRecord model = new ListPassportsByTipstaffRecord();
            model.tipstaffRecordID = w.tipstaffRecordID;
            model.TipstaffRecordClosed = w.caseStatusID > 2;
            model.Passports = w.Passports.OrderByDescending(d => d.createdOn).ToXPagedList<Passport>(page ?? 1, 8);
            return PartialView("_ListPassportsByRecord", model);
        }

        public ActionResult ExtractPassport(int id)
        {
            try
            {
                Passport doc = db.Passports.Find(id);
                if (doc == null)
                {
                    ErrorModel errModel = new ErrorModel(2);
                    errModel.ErrorMessage = string.Format("Passport {0} has been deleted, please raise a help desk call if you think this has been deleted in error.", id);
                    TempData["ErrorModel"] = errModel;
                    return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
                }
                byte[] file = doc.binaryFile;
                return File(file, doc.mimeType, doc.fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in PassportController in ExtractPassport method, for user {((CPrincipal)User).UserID}");

                return View("Error");
            }
        }

        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            DeletePassport model = new DeletePassport(id);
            if (model.Passport == null)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("Passport {0} has been deleted, please raise a help desk call if you think this has been deleted in error.", id);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }

        //
        // POST: /Passport/Delete/5

        [HttpPost, ActionName("Delete"), AuthorizeRedirect(Roles = "Admin")]
        public ActionResult DeleteConfirmed(DeletePassport model)
        {
            model.Passport = db.Passports.Find(model.DeleteModelID);
            int tipstaffRecordID = model.Passport.tipstaffRecordID;
            string controller = genericFunctions.TypeOfTipstaffRecord(tipstaffRecordID);
            db.Passports.Remove(model.Passport);
            db.SaveChanges();
            string recDeleted = model.DeleteModelID.ToString();
            AuditEvent AE = db.AuditEvents.Where(a => a.auditEventDescription.AuditDescription == "Passport deleted" && a.RecordChanged == recDeleted).OrderByDescending(a => a.EventDate).Take(1).Single();
            //add a deleted reason
            AE.DeletedReasonID = model.DeletedReasonID;
            //and save again
            db.SaveChanges();
            return RedirectToAction("Details", controller, new { id = tipstaffRecordID });
        }

    }
}
