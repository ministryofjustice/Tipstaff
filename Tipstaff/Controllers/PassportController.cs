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

        [HttpGet]
        public ActionResult Upload(int id)
        {
            PassportUploadModel model = new PassportUploadModel();
            model.tipstaffRecordID = id;
            model.tipstaffRecord = db.TipstaffRecord.Find(id);
            if (model.tipstaffRecord.caseStatus.sequence > 3)
            {
                TempData["UID"] = model.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }

            return View(model);
        }
        [HttpPost]
        public ActionResult Upload(PassportUploadModel model)
        {
            User user = db.GetUserByLoginName(User.Identity.Name.Split('\\').Last());
            model.passport.createdBy = user.DisplayName;
            model.passport.createdOn = DateTime.Now;
            model.passport.tipstaffRecordID = model.tipstaffRecordID;
            model.passport.documentTypeID = 336;
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

        public ActionResult Edit(int id)
        {
            PassportUploadModel model = new PassportUploadModel();
            model.passport = db.Passports.Find(id);
            //model.tipstaffRecordID = model.passport.tipstaffRecordID;
            model.passport.tipstaffRecordID = model.tipstaffRecordID;
            if (model.passport == null)
            {
                ErrorModel errModel = new ErrorModel();
                errModel.ErrorMessage = "No respondent with that ID can be found";
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", errModel ?? null);
            }
            if (model.passport.tipstaffRecord.caseStatus.sequence > 3)
            {
                TempData["UID"] = model.passport.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(PassportUploadModel model)
        {
            if (ModelState.IsValid)
            {
                TipstaffRecord tr = db.TipstaffRecord.Find(model.tipstaffRecordID);
                db.Entry(model.passport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", genericFunctions.TypeOfTipstaffRecord(model.passport.tipstaffRecordID), new { id = model.passport.tipstaffRecordID });
                return RedirectToAction("Details", genericFunctions.TypeOfTipstaffRecord(tr), new { id = model.tipstaffRecordID });
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

    }
}
