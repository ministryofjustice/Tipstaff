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
using Tipstaff.Services.Repositories;
using Tipstaff.Infrastructure.S3API;
using Tipstaff.Infrastructure.Services;

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class TemplatesController : Controller
    {
        //private TipstaffDB db = myDBContextHelper.CurrentContext;

        private readonly ITemplateRepository _templateRepository;
        private readonly IS3API _s3API;

        public TemplatesController(ITemplateRepository templateRepo, IS3API s3Repo)
        {
            _templateRepository = templateRepo;
            _s3API = s3Repo;
        }

        //
        // GET: /Admin/Template/

        public ActionResult Index()
        {
            //var model = db.Templates.OrderBy(t=>t.Discriminator).ThenBy(t=>t.templateName);
            var model = _templateRepository.GetAllTemplates().OrderBy(t => t.Discriminator).ThenBy(t => t.templateName);
            List<Template> templates = new List<Template>();
            foreach (Services.DynamoTables.Template t in model)
            {
                Template temp = new Template() {
                    templateID = t.templateID,
                    Discriminator = t.Discriminator,
                    templateName = t.templateName,
                    filePath = t.filePath,
                    addresseeRequired = t.addresseeRequired,
                    active = t.active,
                    deactivated = t.deactivated,
                    deactivatedBy = t.deactivatedBy
                };
                templates.Add(temp);
            }
            return View(templates);
        }

        public ActionResult Open(string id)
        {
            //Template template = db.Templates.Find(id);
            //XmlDocument xDoc = new XmlDocument();
            //xDoc.InnerXml = template.templateXML;
            //return File(genericFunctions.ConvertToBytes(xDoc), "application/msword", template.templateName +".xml"); 
            try
            {
                var template = _templateRepository.GetTemplate(id);
                return File(template.filePath, "application/msword");
            }
            catch (Exception ex)
            {
            }
            return null;
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
                    var filePath = _s3API.Save("tipstaff", "templates", model.uploadFile.FileName, model.uploadFile.InputStream);

                    //Upload
                    //var fileName = Path.Combine(Server.MapPath("~/uploads"), Path.GetFileName(model.uploadFile.FileName));
                    //model.uploadFile.SaveAs(fileName); //Save to uploads folder     
                    //XmlDocument document = new XmlDocument();
                    //document.Load(fileName);
                    //xml = document.InnerXml;
                    ////Delete file
                    //System.IO.File.Delete(fileName);

                    string tid = (model.Template.templateID == null) ? GuidGenerator.GenerateTimeBasedGuid().ToString() : model.Template.templateID;
                    _templateRepository.AddTemplate(new Services.DynamoTables.Template()
                    {
                        templateID = tid,
                        Discriminator = model.Template.Discriminator,
                        templateName = model.Template.templateName,
                        filePath = filePath,
                        addresseeRequired = model.Template.addresseeRequired,
                        active = true
                    });
                    //db.Entry(model.Template).State = EntityState.Added;
                    //db.SaveChanges();
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
            var t = _templateRepository.GetTemplate(id);
            model.Template = new Template() {
                templateID = id,
                Discriminator = t.Discriminator,
                templateName = t.templateName,
                filePath = t.filePath,
                addresseeRequired = t.addresseeRequired,
                active = t.active,
                deactivated = t.deactivated,
                deactivatedBy = t.deactivatedBy
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(TemplateEdit model)
        {
            //Template oldTemplate = db.Templates.Find(model.Template.templateID);
            var oldTemplate = _templateRepository.GetTemplate(model.Template.templateID);
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
                //model.Template.templateXML = xml;
                
                //db.Entry(oldTemplate).CurrentValues.SetValues(model.Template);
                //db.SaveChanges();
                _templateRepository.Update(new Services.DynamoTables.Template()
                {
                    templateID = model.Template.templateID,
                    Discriminator = model.Template.Discriminator,
                    templateName = model.Template.templateName,
                    filePath = filePath,
                    addresseeRequired = model.Template.addresseeRequired,
                    active = model.Template.active
                });

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
            //Template model = db.Templates.Find(id);
            var model =_templateRepository.GetTemplate(id);
            if (model.active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot view {0} as it has been deactivated, please raise a help desk call to re-activate it.", model.templateName);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(new Template() {
                templateID = model.templateID,
                templateName = model.templateName
            });
        }
        //
        // POST: /Admin/Solicitor/Delete/5
        [HttpPost, ActionName("Deactivate")]
        public ActionResult DeactivateConfirmed(string id)
        {
            //Template model = db.Templates.Find(id);
            var model = _templateRepository.GetTemplate(id);
            _templateRepository.Update(new Services.DynamoTables.Template()
            {
                templateID = model.templateID,
                Discriminator = model.Discriminator,
                templateName = model.templateName,
                filePath = model.filePath,
                addresseeRequired = model.addresseeRequired,
                active = false,
                deactivated = DateTime.Now,
                deactivatedBy = User.Identity.Name
            });
           
            //model.templateXML = null;
            //db.Entry(model).State = EntityState.Modified;
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
