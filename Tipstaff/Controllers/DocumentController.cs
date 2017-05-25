using System.Linq;
using System.Web.Mvc;
using Tipstaff.Models;
using System;
using System.IO;
using System.Web.UI;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class DocumentController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;

        
        public ActionResult ChooseAddressee(int tipstaffRecordID, int templateID)
        {
            TipstaffRecord tr = db.TipstaffRecord.Find(tipstaffRecordID);
            if (tr.caseStatus.sequence > 3)
            {
                TempData["UID"] = tr.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }

            Template t = db.Templates.Find(templateID);
            if (t.addresseeRequired)
            {
                ChooseAddresseeModel model = new ChooseAddresseeModel();
                model.tipstaffRecord = tr;
                model.template = t;
                model.SolicitorsOnRecord = tr.LinkedSolicitors;
                if (genericFunctions.isTipstaffRecordChildAbduction(tr))
                {
                    model.Applicants = ((ChildAbduction)tr).Applicants;
                }
                return View(model);
            }
            else 
            {
                return RedirectToRoute(new
                {
                    action = "Create",
                    controller = "Template",
                    tipstaffRecordID = tr.tipstaffRecordID,
                    templateID = t.templateID
                });
            }
        }


        [HttpPost]
        public ActionResult ChooseAddressee(ChooseAddresseeModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.template.addresseeRequired)
                {
                    return RedirectToRoute(new
                                            {
                                                action = "Create4",
                                                controller = "Template",
                                                tipstaffRecordID = model.tipstaffRecord.tipstaffRecordID,
                                                templateID = model.template.templateID,
                                                solicitorID = model.solicitorID
                                            });
                }
                else
                {
                    return RedirectToRoute(new
                                            {
                                                action = "Create",
                                                controller = "Template",
                                                tipstaffRecordID = model.tipstaffRecord.tipstaffRecordID,
                                                templateID = model.template.templateID 
                                            });
                }

            }
            return View(model);
        }



        //
        // GET: /Document/

        public ActionResult Select(int id)
        {
            CreateDocumentViewModel model = new CreateDocumentViewModel();
            model.tipstaffRecord = db.TipstaffRecord.Find(id);
            if (model.tipstaffRecord.caseStatus.sequence > 3)
            {
                TempData["UID"] = model.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }

            string docType = genericFunctions.TypeOfTipstaffRecord(model.tipstaffRecord);
            model.TemplatesForRecordType = db.Templates.Where(t => (t.Discriminator == docType || t.Discriminator == "All") && t.active).OrderBy(t=>t.Discriminator);
            return View(model);
        }
        [HttpGet]
        public ActionResult Upload(int id)
        {
            DocumentUploadModel model = new DocumentUploadModel();
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
        public ActionResult Upload(DocumentUploadModel model)
        {
            User user = db.GetUserByLoginName(User.Identity.Name.Split('\\').Last());
            model.document.createdBy = user.DisplayName;
            model.document.createdOn = DateTime.Now;
            model.document.tipstaffRecordID = model.tipstaffRecordID;
            if (model.uploadFile != null)
            {
                var stream = model.uploadFile.InputStream;
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                model.document.binaryFile = buffer;
                model.document.fileName = System.IO.Path.GetFileName(model.uploadFile.FileName);
                model.document.mimeType = model.uploadFile.ContentType;
            }
            if (ModelState.IsValid)
            {
                TipstaffRecord tr = db.TipstaffRecord.Find(model.tipstaffRecordID);
                tr.Documents.Add(model.document);
                db.SaveChanges();
                return RedirectToAction("Details", genericFunctions.TypeOfTipstaffRecord(tr), new { id = model.tipstaffRecordID });
            }
            return View(model);
        }
        [OutputCache(Location = OutputCacheLocation.Server, Duration = 180)]
        public PartialViewResult ListDocumentsByRecord(int id, int? page)
        {
            TipstaffRecord w = db.TipstaffRecord.Find(id);

            ListDocumentsByTipstaffRecord model = new ListDocumentsByTipstaffRecord();
            model.tipstaffRecordID = w.tipstaffRecordID;
            model.TipstaffRecordClosed = w.caseStatusID > 2;
            model.Documents = w.Documents.OrderByDescending(d => d.createdOn).ToXPagedList<Document>(page ?? 1, 8);
            return PartialView("_ListDocumentsByRecord", model);
        }

        public ActionResult ExtractDocument(int id)
        {
            try
            {
                Document doc = db.Documents.Find(id);
                if (doc == null)
                {
                    ErrorModel errModel = new ErrorModel(2);
                    errModel.ErrorMessage = string.Format("Document {0} has been deleted, please raise a help desk call if you think this has been deleted in error.", id);
                    TempData["ErrorModel"] = errModel;
                    return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
                }
                byte[] file = doc.binaryFile;
                return File(file, doc.mimeType, doc.fileName);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            DeleteDocument model = new DeleteDocument(id);
            if (model.Document == null)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("Document {0} has been deleted, please raise a help desk call if you think this has been deleted in error.", id);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }

        //
        // POST: /Document/Delete/5

        [HttpPost, ActionName("Delete"), AuthorizeRedirect(Roles = "Admin")]
        public ActionResult DeleteConfirmed(DeleteDocument model)
        {
            model.Document = db.Documents.Find(model.DeleteModelID);
            int tipstaffRecordID = model.Document.tipstaffRecordID;
            string controller = genericFunctions.TypeOfTipstaffRecord(tipstaffRecordID);
            db.Documents.Remove(model.Document);
            db.SaveChanges();
            string recDeleted = model.DeleteModelID.ToString();
            AuditEvent AE = db.AuditEvents.Where(a => a.auditEventDescription.AuditDescription == "Document deleted" && a.RecordChanged == recDeleted).OrderByDescending(a => a.EventDate).Take(1).Single();
            //add a deleted reason
            AE.DeletedReasonID = model.DeletedReasonID;
            //and save again
            db.SaveChanges();
            return RedirectToAction("Details", controller, new { id = tipstaffRecordID });
        }

     }
}
