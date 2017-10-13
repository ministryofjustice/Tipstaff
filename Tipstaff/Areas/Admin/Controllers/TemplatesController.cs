﻿using System;
using System.Linq;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Data;
using System.IO;
using Tipstaff.Infrastructure.S3API;
using Tipstaff.Presenters;

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class TemplatesController : Controller
    {
        private readonly ITemplatePresenter _templatePresenter;
        private readonly IS3API _s3API;

        public TemplatesController(ITemplatePresenter templatePresenter, IS3API s3Repo)
        {
            _templatePresenter = templatePresenter;
            _s3API = s3Repo;
        }

        //
        // GET: /Admin/Template/

        public ActionResult Index()
        {
            //var model = db.Templates.OrderBy(t=>t.Discriminator).ThenBy(t=>t.templateName);
            var model = _templatePresenter.GetAllTemplates().OrderBy(t => t.Discriminator).ThenBy(t => t.templateName);

            return View(model);
        }

        public ActionResult Open(string id)
        {
            //Template template = db.Templates.Find(id);
            //XmlDocument xDoc = new XmlDocument();
            //xDoc.InnerXml = template.templateXML;
            //return File(genericFunctions.ConvertToBytes(xDoc), "application/msword", template.templateName +".xml"); 
            try
            {
                Template template = _templatePresenter.GetTemplate(id);
                return File(template.filePath, "application/msword", template.templateName + ".xml");
            }
            catch (Exception ex)
            {
                return View("Error");
            }

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
                    if (!Path.GetExtension(model.uploadFile.FileName.ToLower()).EndsWith("xml"))
                    {
                        throw new NotUploaded("Please select an XML file to upload");
                    }
                    if (model.uploadFile.ContentLength == 0)
                    {
                        throw new NotUploaded("The selected file appears to be empty, please select a different file and re-try");
                    }
                    model.Template.filePath = _s3API.Save("tipstaff", "templates", model.uploadFile.FileName, model.uploadFile.InputStream);
                    model.Template.active = true;
                    //Upload
                    //var fileName = Path.Combine(Server.MapPath("~/uploads"), Path.GetFileName(model.uploadFile.FileName));
                    //model.uploadFile.SaveAs(fileName); //Save to uploads folder     
                    //XmlDocument document = new XmlDocument();
                    //document.Load(fileName);
                    //xml = document.InnerXml;
                    ////Delete file
                    //System.IO.File.Delete(fileName);


                    _templatePresenter.AddTemplate(model);

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

        public ActionResult Edit(string id)
        {
            TemplateEdit model = new TemplateEdit();

            model.Template = _templatePresenter.GetTemplate(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(TemplateEdit model)
        {
            //Template oldTemplate = db.Templates.Find(model.Template.templateID);
            Template oldTemplate = _templatePresenter.GetTemplate(model.Template.templateID);
            string filePath = string.Empty;
            try
            {
                //Tests before uploading
                if (model.uploadFile != null)
                {
                    if (!Path.GetExtension(model.uploadFile.FileName.ToLower()).EndsWith("xml"))
                    {
                        throw new NotUploaded("Please select an XML file to upload");
                    }
                    if (model.uploadFile.ContentLength == 0)
                    {
                        throw new NotUploaded("The selected file appears to be empty, please select a different file and re-try");
                    }
                    //Upload
                    filePath = _s3API.Save("tipstaff", "templates", model.uploadFile.FileName, model.uploadFile.InputStream);
                    //var fileName = Path.Combine(Server.MapPath("~/uploads"), Path.GetFileName(model.uploadFile.FileName));
                    //model.uploadFile.SaveAs(fileName); //Save to uploads folder     
                    //XmlDocument document = new XmlDocument();
                    //document.Load(fileName);
                    //xml = document.InnerXml;
                    ////Delete file
                    //System.IO.File.Delete(fileName);
                }
                else
                {
                    filePath = oldTemplate.filePath; //db.Templates.Find(model.Template.templateID).templateXML;
                }
                model.Template.filePath = filePath;
                _templatePresenter.UpdateTemplate(model);

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
        public ActionResult Deactivate(string id)
        {
            Template model = _templatePresenter.GetTemplate(id);

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
        public ActionResult DeactivateConfirmed(string id)
        {
            TemplateEdit model = new TemplateEdit();
            model.Template = _templatePresenter.GetTemplate(id);
            model.Template.active = false;
            model.Template.deactivated = DateTime.Now;
            model.Template.deactivatedBy = User.Identity.Name;
            _templatePresenter.UpdateTemplate(model);

            //model.templateXML = null;
            //db.Entry(model).State = EntityState.Modified;
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}