using System.Linq;
using System.Web.Mvc;
using Tipstaff.Models;
using System;
using System.Web.UI;
using TPLibrary.Logger;

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



        public PartialViewResult ListPassportsByRecord(int id, int? page)
        {
            TipstaffRecord w = db.TipstaffRecord.Find(id);

            ListPassportsByTipstaffRecord model = new ListPassportsByTipstaffRecord();
            model.tipstaffRecordID = w.tipstaffRecordID;
            model.TipstaffRecordClosed = w.caseStatusID > 2;
            model.Passports = w.Passports.OrderByDescending(d => d.createdOn).ToXPagedList<Passport>(page ?? 1, 8);
            return PartialView("_ListPassportsByRecord", model);
        }

        [HttpPost]
        public ActionResult Edit(PassportEditModel model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model.passport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "ChildAbduction", new { id = model.passport.tipstaffRecordID });
            }
            return View(model);
        }

    }
}
