using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Data;
using System.IO;
using System.Xml;
using System.Data.Entity;

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class TemplatesController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;

        //
        // GET: /Admin/Template/

        public ActionResult Index()
        {
            var model = db.Templates.OrderBy(t=>t.Discriminator).ThenBy(t=>t.templateName);
            return View(model);
        }

        public ActionResult Open(int id)
        {
            Template template = db.Templates.Find(id);
            XmlDocument xDoc = new XmlDocument();
            xDoc.InnerXml = template.templateXML;
            return File(genericFunctions.ConvertToBytes(xDoc), "application/msword", template.templateName +".xml"); 
        }
        public ActionResult Create()
        {
            TemplateEdit model = new TemplateEdit();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(TemplateEdit model)
        {
            var xml = string.Empty;
            try
            {
                //Tests before uploading
                if (model.uploadFile != null)
                {
                    if (!Path.GetExtension(model.uploadFile.FileName.ToLower()).EndsWith("xml")) { throw new NotUploaded("Please select an XML file to upload"); }
                    if (model.uploadFile.ContentLength == 0) { throw new NotUploaded("The selected file appears to be empty, please select a different file and re-try"); }
                    Directory.CreateDirectory(@"C:\TipstaffUploads");
                    //Upload
                    var fileName = Path.Combine("C:\\TipstaffUploads", Path.GetFileName(model.uploadFile.FileName));
                    model.uploadFile.SaveAs(fileName); //Save to uploads folder     
                    XmlDocument document = new XmlDocument();
                    document.Load(fileName);
                    xml = document.InnerXml;
                    //Delete file
                    System.IO.File.Delete(fileName);
                    model.Template.templateXML = xml;
                    model.Template.active = true;
                    db.Entry(model.Template).State = EntityState.Added;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    model.ErrorMessage = "Please select a template file";
                    model.UploadSuccessful = false;
                    ModelState.AddModelError("Error", "Please select a template file");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                model.ErrorMessage = genericFunctions.GetLowestError(ex);
                model.UploadSuccessful = false;
                return View(model);
            }
        }

        public ActionResult Edit(int id)
        {
            TemplateEdit model = new TemplateEdit(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(TemplateEdit model)
        {
            Template oldTemplate = db.Templates.Find(model.Template.templateID);

            var xml = string.Empty;
            try
            {
                //Tests before uploading
                if (model.uploadFile != null)
                {
                    if (!Path.GetExtension(model.uploadFile.FileName.ToLower()).EndsWith("xml")) { throw new NotUploaded("Please select an XML file to upload"); }
                    if (model.uploadFile.ContentLength == 0) { throw new NotUploaded("The selected file appears to be empty, please select a different file and re-try"); }
                    Directory.CreateDirectory(@"C:\TipstaffUploads");
                    //Upload
                    var fileName = Path.Combine("C:\\TipstaffUploads", Path.GetFileName(model.uploadFile.FileName));
                    model.uploadFile.SaveAs(fileName); //Save to uploads folder     
                    XmlDocument document = new XmlDocument();
                    document.Load(fileName);
                    xml = document.InnerXml;
                    //Delete file
                    System.IO.File.Delete(fileName);
                }
                else
                {
                    xml = db.Templates.Find(model.Template.templateID).templateXML;
                }
                model.Template.templateXML = xml;
                db.Entry(oldTemplate).CurrentValues.SetValues(model.Template);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                model.ErrorMessage = genericFunctions.GetLowestError(ex);
                model.UploadSuccessful = false;
                return View(model);
            }
        }

        // GET: /Admin/Template/Delete/5
        public ActionResult Deactivate(int id)
        {
            Template model = db.Templates.Find(id);
            if (model.active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot view {0} as it has been deactivated, please raise a help desk call to re-activate it.", model.templateName);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }
        //
        // POST: /Admin/Solicitor/Delete/5
        [HttpPost, ActionName("Deactivate")]
        public ActionResult DeactivateConfirmed(int id)
        {
            Template model = db.Templates.Find(id);
            model.active = false;
            model.deactivated = DateTime.Now;
            model.deactivatedBy = User.Identity.Name;
            //model.templateXML = null;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
