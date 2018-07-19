using System.Linq;
using System.Web.Mvc;
using Tipstaff.Models;
using System;
using System.IO;
using System.Web.UI;
using TPLibrary.Logger;
using Tipstaff.Presenters;
using TPLibrary.GuidGenerator;
using TPLibrary.S3API;
using System.Text;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class DocumentController : Controller
    {
        //private TipstaffDB db = myDBContextHelper.CurrentContext;

        private readonly IDocumentPresenter _docPresenter;
        private readonly IS3API _s3API;
        private readonly ICloudWatchLogger _logger;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ITipstaffRecordPresenter _tipstaffPresenter;
        private readonly ITemplatePresenter _templatePresenter;
        
        public DocumentController(ICloudWatchLogger logger, IS3API s3api, IDocumentPresenter docPresenter, ITipstaffRecordPresenter tipstaffPresenter, ITemplatePresenter templatePresenter, IGuidGenerator guidGenerator)
        {
            _logger = logger;
            _docPresenter = docPresenter;
            _s3API = s3api;
            _guidGenerator = guidGenerator;
            _tipstaffPresenter = tipstaffPresenter;
            _templatePresenter = templatePresenter;
        }

        public ActionResult ChooseAddressee(string tipstaffRecordID, string templateID)
        {
            TipstaffRecord tr = _tipstaffPresenter.GetTipStaffRecord(tipstaffRecordID);
            if (tr.caseStatus.Sequence > 3)
            {
                TempData["UID"] = tr.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }

            Template t = _templatePresenter.GetTemplate(templateID);
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

        public ActionResult Select(string id)
        {
            CreateDocumentViewModel model = new CreateDocumentViewModel();
            model.tipstaffRecord = _tipstaffPresenter.GetTipStaffRecord(id);
            if (model.tipstaffRecord.caseStatus.Sequence > 3)
            {
                TempData["UID"] = model.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }

            string docType = model.tipstaffRecord.Discriminator; //genericFunctions.TypeOfTipstaffRecord(model.tipstaffRecord);
            model.TemplatesForRecordType = _templatePresenter.GetTemplatesForRecordType(docType); 
            return View(model);
        }

        [HttpGet]
        public ActionResult Upload(string id)
        {
            DocumentUploadModel model = new DocumentUploadModel();
            model.tipstaffRecordID = id;
            model.tipstaffRecord = _tipstaffPresenter.GetTipStaffRecord(id);
            if (model.tipstaffRecord.caseStatus.Sequence > 3)
            {
                TempData["UID"] = model.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Upload(DocumentUploadModel model)
        {
            //User user = _userPresenter.GetUserByLoginName(User.Identity.Name.Split('\\').Last());

            model.document.createdBy = (User ==null?"":User.Identity.Name.Split('\\').Last()); // user.DisplayName;
            model.document.createdOn = DateTime.Now;
            model.document.tipstaffRecordID = model.tipstaffRecordID;
            string filePath = String.Empty;
            if (model.uploadFile != null)
            {
                var stream = model.uploadFile.InputStream;
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                var filename = model.tipstaffRecordID + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + model.uploadFile.FileName;
                filePath = _s3API.Save("documents", filename, model.uploadFile.InputStream);

                model.document.fileName = filename;
                model.document.mimeType = model.uploadFile.ContentType;
                model.document.filePath = filePath;
            }
            if (ModelState.IsValid)
            {
                string did = (model.document.documentID == null) ? _guidGenerator.GenerateTimeBasedGuid().ToString() : model.document.documentID;
                model.document.documentID = did;
                _docPresenter.AddDocument(model);
                return RedirectToAction("Details", genericFunctions.TypeOfTipstaffRecord(model.tipstaffRecordID), new { id = model.tipstaffRecordID });
            }
            return View(model);
        }

        [OutputCache(Location = OutputCacheLocation.Server, Duration = 180)]
        public PartialViewResult ListDocumentsByRecord(string id, int? page)
        {
            TipstaffRecord w = _tipstaffPresenter.GetTipStaffRecord(id);
            ListDocumentsByTipstaffRecord model = new ListDocumentsByTipstaffRecord();
            model.tipstaffRecordID = id;
            model.TipstaffRecordClosed = (w.caseStatus.Detail == "File Closed" || w.caseStatus.Detail == "File Archived" || w.caseStatus.Detail == "Stayed");
            model.Documents = _docPresenter.GetAllDocumentsByTipstaffRecordID(id).OrderByDescending(d => d.createdOn).ToXPagedList<Document>(page ?? 1, 8);
            return PartialView("_ListDocumentsByRecord", model);
        }

        public ActionResult ExtractDocument(string id)
        {
            //try
            //{
            //    Document doc = db.Documents.Find(id);
            //    if (doc == null)
            //    {
            //        ErrorModel errModel = new ErrorModel(2);
            //        errModel.ErrorMessage = string.Format("Document {0} has been deleted, please raise a help desk call if you think this has been deleted in error.", id);
            //        TempData["ErrorModel"] = errModel;
            //        return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            //    }
            //    byte[] file = doc.binaryFile;
            //    return File(file, doc.mimeType, doc.fileName);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, $"Exception in DocumentController in ExtractDocument method, for user {((CPrincipal)User).UserID}");

            //    return View("Error");
            //}
            try
            {
                Document doc = _docPresenter.GetDocument(id);
                string filename = doc.fileName; //Path.GetFileName(doc.filePath);
                var response = _s3API.ReadS3Object("documents", filename);
                return File(new MemoryStream(Encoding.UTF8.GetBytes(response)), "application/msword", filename);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in DocumentController in ExtractDocument method, for user {((CPrincipal)User).UserID}");
                return View("Error");
            }

        }

        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            DeleteDocument model = new DeleteDocument();
            model.Document = _docPresenter.GetDocument(id);
            
            if (model.Document == null)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("Document {0} has been deleted, please raise a help desk call if you think this has been deleted in error.", id);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            model.DeleteModelID = model.Document.documentID;
            return View(model);
        }

        //
        // POST: /Document/Delete/5

        [HttpPost, ActionName("Delete"), AuthorizeRedirect(Roles = "Admin")]
        public ActionResult DeleteConfirmed(DeleteDocument model)
        {
            model.Document = _docPresenter.GetDocument(model.DeleteModelID); 
            string tipstaffRecordID = model.Document.tipstaffRecordID;
            string controller = genericFunctions.TypeOfTipstaffRecord(tipstaffRecordID);
            //db.Documents.Remove(model.Document);
            //db.SaveChanges();
            //string recDeleted = model.DeleteModelID.ToString();
            //AuditEvent AE = db.AuditEvents.Where(a => a.auditEventDescription.AuditDescription == "Document deleted" && a.RecordChanged == recDeleted).OrderByDescending(a => a.EventDate).Take(1).Single();
            ////add a deleted reason
            //AE.DeletedReasonID = model.DeletedReasonID;
            ////and save again
            //db.SaveChanges();

            _s3API.DeleteS3Object("documents", model.Document.fileName);
            _docPresenter.DeleteDocument(model);
            return RedirectToAction("Details", controller, new { id = tipstaffRecordID });
        }

     }
}
