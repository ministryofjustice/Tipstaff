using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Security;
using System.Data.Entity.Validation;
using Tipstaff.Logger;
using Tipstaff.Services.Repositories;
using Tipstaff.Services.Services;
using Tipstaff.Infrastructure.S3API;
using Tipstaff.Presenter;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class TemplateController : Controller
    {
        //private TipstaffDB db = myDBContextHelper.CurrentContext;
        private readonly IPresenterTemplate _templatePresenter;

        private readonly IS3API _s3API;
        private readonly ICloudWatchLogger _logger;

        public TemplateController(ICloudWatchLogger logger, IS3API s3api, IPresenterTemplate templatePresenter)
        {
            _logger = logger;
            _s3API = s3api;
            _templatePresenter = templatePresenter;
        }
        //
        // GET: /Template/
        //public ActionResult Create(int tipstaffRecordID, int templateID)
        //{
        //    try
        //    {
        //        //Get TipstaffRecord from warrantID
        //        TipstaffRecord tipstaffRecord = db.TipstaffRecord.Find(tipstaffRecordID);
        //        if (tipstaffRecord.caseStatus.Sequence > 3)
        //        {
        //            TempData["UID"] = tipstaffRecord.UniqueRecordID;
        //            return RedirectToAction("ClosedFile", "Error");
        //        }
        //        //Get Template from templateID
        //        Template template = db.Templates.Find(templateID);
        //        if (template == null) throw new FileLoadException(string.Format("No database record found for template reference {0}",templateID));

        //        //set fileOutput details
        //        WordFile fileOutput = new WordFile(tipstaffRecord, Server.MapPath("~/Documents/"),template);

        //        //Create XML object for Template
        //        XmlDocument xDoc = new XmlDocument();

        //        //Merge Data
        //        xDoc.InnerXml = mergeData(template,tipstaffRecord,null);

        //        ////Save resulting document 
        //        //xDoc.Save(fileOutput.fullName); //Save physical file
        //        //if (!System.IO.File.Exists(fileOutput.fullName)) throw new FileNotFoundException(string.Format("File {0} could not be created", fileOutput.fileName));

        //        //Create and add a Document to TipstaffRecord
        //        Document doc = new Document();
        //        doc.binaryFile = genericFunctions.ConvertToBytes(xDoc);
        //        doc.mimeType = "application/msword";
        //        doc.fileName = fileOutput.fileName;
        //        doc.countryID = 244; //UK!
        //        doc.nationalityID = 27;
        //        doc.documentTypeID = 1;     //generated
        //        doc.documentStatusID = 1;   //generated
        //        doc.documentReference = template.templateName;
        //        doc.templateID = template.templateID;
        //        doc.createdOn = DateTime.Now;
        //        doc.createdBy = User.Identity.Name;
        //        tipstaffRecord.Documents.Add(doc);

        //        //Save Changes
        //        db.SaveChanges();

        //        //Return saved document
        //        //return File(fileOutput.fullName, "application/doc", fileOutput.fileName); // return physical file 
        //        return File(doc.binaryFile, doc.mimeType, doc.fileName); //return byte version
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        _logger.LogError(ex, $"DbEntityValidationException in TemplateController in Create method, for user {((CPrincipal)User).UserID}");

        //        ErrorModel model = new ErrorModel(2);
        //        model.ErrorMessage = ex.Message;
        //        TempData["ErrorModel"] = model;
        //        return RedirectToAction("IndexByModel", "Error", model ?? null);

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Exception in TemplateController in Create method, for user {((CPrincipal)User).UserID}");

        //        ErrorModel model = new ErrorModel(2);
        //        model.ErrorMessage = ex.Message;
        //        TempData["ErrorModel"] = model;
        //        return RedirectToAction("IndexByModel", "Error", model ?? null);
        //        //Note: working redirect to view with Model
        //        //Note: Working error redirect
        //    }
        //}
        public ActionResult Create(string tipstaffRecordID, string templateID)
        {
            try
            {
                //Get TipstaffRecord from warrantID
                TipstaffRecord tipstaffRecord = _templatePresenter.GetTipstaffRecord(tipstaffRecordID);

                
                if (tipstaffRecord.caseStatus.Detail == "File Closed" || tipstaffRecord.caseStatus.Detail == "File Archived") 
                {
                    TempData["UID"] =  tipstaffRecord.UniqueRecordID;
                    return RedirectToAction("ClosedFile", "Error");
                }
                //Get Template from templateID
                Template template = _templatePresenter.GetTemplate(templateID);
                if (template == null) throw new FileLoadException(string.Format("No database record found for template reference {0}", templateID));


                //set fileOutput details
                // WordFile fileOutput = new WordFile(tipstaffRecord, Server.MapPath("~/Documents/"), template);
                WordFile fileOutput = new WordFile(tipstaffRecordID, Server.MapPath("~/Documents/"), templateID, template.templateName, tipstaffRecordID); //INCORRECT! THE LAST TIPSTAFFRECORDID SHOULD BE UNIQUEREFERENCEID
                //Create XML object for Template
                XmlDocument xDoc = new XmlDocument();

                //Merge Data

                xDoc.InnerXml = mergeData(template, tipstaffRecord, null);

                ////Save resulting document 
                //xDoc.Save(fileOutput.fullName); //Save physical file
                //if (!System.IO.File.Exists(fileOutput.fullName)) throw new FileNotFoundException(string.Format("File {0} could not be created", fileOutput.fileName));

                //Create and add a Document to TipstaffRecord --> NOT REQUIRED TO SAVE GENERATED FILES!!!
                //Document doc = new Document();
                ////doc.binaryFile = genericFunctions.ConvertToBytes(xDoc);
                //doc.mimeType = "application/msword";
                //doc.fileName = fileOutput.fileName;
                //doc.country = MemoryCollections.CountryList.GetCountryByID(244); //UK!
                //doc.nationality = MemoryCollections.NationalityList.GetNationalityByID(27);
                //doc.documentType = MemoryCollections.DocumentTypeList.GetDocumentTypeByID(1);     //generated
                //doc.documentStatus = MemoryCollections.DocumentStatusList.GetDocumentStatusByID(1);   //generated
                //doc.documentReference = template.templateName;
                //doc.templateID = template.templateID;
                //doc.createdOn = DateTime.Now;
                //doc.createdBy = User.Identity.Name;
                //tipstaffRecord.Documents.Add(doc);

                ////Save Changes
                //db.SaveChanges();

                //Return saved document
                //return File(fileOutput.fullName, "application/doc", fileOutput.fileName); // return physical file 
                return File(genericFunctions.ConvertToBytes(xDoc), "application/msword", fileOutput.fileName); //return byte version
            }
            catch (DbEntityValidationException ex)
            {
                _logger.LogError(ex, $"DbEntityValidationException in TemplateController in Create method, for user {((CPrincipal)User).UserID}");

                ErrorModel model = new ErrorModel(2);
                model.ErrorMessage = ex.Message;
                TempData["ErrorModel"] = model;
                return RedirectToAction("IndexByModel", "Error", model ?? null);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in TemplateController in Create method, for user {((CPrincipal)User).UserID}");

                ErrorModel model = new ErrorModel(2);
                model.ErrorMessage = ex.Message;
                TempData["ErrorModel"] = model;
                return RedirectToAction("IndexByModel", "Error", model ?? null);
                //Note: working redirect to view with Model
                //Note: Working error redirect
            }
        }
        //public ActionResult Create4(int tipstaffRecordID, int templateID, int solicitorID)
        //{
        //    try
        //    {
        //        //get solicitor from solicitorID
        //        Solicitor solicitor = db.Solicitors.Find(solicitorID);

        //        //Get TipstaffRecord from warrantID
        //        TipstaffRecord tipstaffRecord = db.TipstaffRecord.Find(tipstaffRecordID);
        //        if (tipstaffRecord.caseStatus.Sequence > 3)
        //        {
        //            TempData["UID"] = tipstaffRecord.UniqueRecordID;
        //            return RedirectToAction("ClosedFile", "Error");
        //        }

        //        //Get Template from templateID
        //        Template template = db.Templates.Find(templateID);
        //        if (template == null) throw new FileLoadException(string.Format("No database record found for template reference {0}",templateID));

        //        //set fileOutput details
        //        WordFile fileOutput = new WordFile(tipstaffRecord, Server.MapPath("~/Documents/"),template);

        //        //Create XML object for Template
        //        XmlDocument xDoc = new XmlDocument();

        //        //Merge Data
        //        xDoc.InnerXml = mergeData(template,tipstaffRecord, solicitor);

        //        ////Save resulting document 
        //        //xDoc.Save(fileOutput.fullName); //Save physical file
        //        //if (!System.IO.File.Exists(fileOutput.fullName)) throw new FileNotFoundException(string.Format("File {0} could not be created", fileOutput.fileName));

        //        //Create and add a Document to TipstaffRecord
        //        Document doc = new Document();
        //        doc.binaryFile = genericFunctions.ConvertToBytes(xDoc);
        //        doc.mimeType = "application/msword";
        //        doc.fileName = fileOutput.fileName;
        //        doc.countryID = 244; //UK!
        //        doc.nationalityID = 27; //English
        //        doc.documentTypeID = 1;     //generated
        //        doc.documentStatusID = 1;   //generated
        //        doc.documentReference = template.templateName;
        //        doc.templateID = template.templateID;
        //        doc.createdOn = DateTime.Now;
        //        doc.createdBy = User.Identity.Name;
        //        tipstaffRecord.Documents.Add(doc);

        //        //Save Changes
        //        db.SaveChanges();

        //        //Return saved document
        //        //return File(fileOutput.fullName, "application/doc", fileOutput.fileName); // return physical file 
        //        return File(doc.binaryFile, doc.mimeType, doc.fileName); //return byte version
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Exception in TemplateController in Create4 method, for user {((CPrincipal)User).UserID}");

        //        ErrorModel model = new ErrorModel(2);
        //        model.ErrorMessage = ex.Message;
        //        TempData["ErrorModel"] = model;
        //        return RedirectToAction("IndexByModel", "Error", model ?? null);
        //        //Note: working redirect to view with Model
        //        //Note: Working error redirect
        //    }
        //}

        public ActionResult Create4(string tipstaffRecordID, string templateID, string solicitorID)
        {
            try
            {
                //get solicitor from solicitorID
                Solicitor solicitor = _templatePresenter.GetSolicitor(solicitorID);

                //Get TipstaffRecord from warrantID
                TipstaffRecord tipstaffRecord = _templatePresenter.GetTipstaffRecord(tipstaffRecordID);
                if (tipstaffRecord.caseStatus.Sequence > 3)
                {
                    TempData["UID"] = tipstaffRecord.UniqueRecordID;
                    return RedirectToAction("ClosedFile", "Error");
                }

                //Get Template from templateID
                Template template = _templatePresenter.GetTemplate(templateID);
                if (template == null) throw new FileLoadException(string.Format("No database record found for template reference {0}", templateID));

                //set fileOutput details
                WordFile fileOutput = new WordFile(tipstaffRecord, Server.MapPath("~/Documents/"), template);

                //Create XML object for Template
                XmlDocument xDoc = new XmlDocument();

                //Merge Data
                xDoc.InnerXml = mergeData(template, tipstaffRecord, solicitor);

                ////Save resulting document 
                //xDoc.Save(fileOutput.fullName); //Save physical file
                //if (!System.IO.File.Exists(fileOutput.fullName)) throw new FileNotFoundException(string.Format("File {0} could not be created", fileOutput.fileName));

                //WE DON'T NEED THIS AS WE DON'T WANT TO SAVE GENERATED FILES
                ////Create and add a Document to TipstaffRecord
                //Document doc = new Document();
                //doc.binaryFile = genericFunctions.ConvertToBytes(xDoc);
                //doc.mimeType = "application/msword";
                //doc.fileName = fileOutput.fileName;
                //doc.countryID = 244; //UK!
                //doc.nationalityID = 27; //English
                //doc.documentTypeID = 1;     //generated
                //doc.documentStatusID = 1;   //generated
                //doc.documentReference = template.templateName;
                //doc.templateID = template.templateID;
                //doc.createdOn = DateTime.Now;
                //doc.createdBy = User.Identity.Name;
                //tipstaffRecord.Documents.Add(doc);

                ////Save Changes
                //db.SaveChanges();

                //Return saved document
                //return File(fileOutput.fullName, "application/doc", fileOutput.fileName); // return physical file 
                return File(genericFunctions.ConvertToBytes(xDoc), "application/msword", fileOutput.fileName); //return byte version
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in TemplateController in Create4 method, for user {((CPrincipal)User).UserID}");

                ErrorModel model = new ErrorModel(2);
                model.ErrorMessage = ex.Message;
                TempData["ErrorModel"] = model;
                return RedirectToAction("IndexByModel", "Error", model ?? null);
                //Note: working redirect to view with Model
                //Note: Working error redirect
            }
        }
        public ActionResult Create8(string tipstaffRecordID, string templateID, string applicantID)
        {
            try
            {
                //get applicant from applicantID
                Applicant applicant = _templatePresenter.GetApplicant(applicantID);

                //Get TipstaffRecord from warrantID
                TipstaffRecord tipstaffRecord = _templatePresenter.GetTipstaffRecord(tipstaffRecordID);
                if (tipstaffRecord.caseStatus.Sequence > 3)
                {
                    TempData["UID"] = tipstaffRecord.UniqueRecordID;
                    return RedirectToAction("ClosedFile", "Error");
                }

                //Get Template from templateID
                Template template = _templatePresenter.GetTemplate(templateID);
                if (template == null) throw new FileLoadException(string.Format("No database record found for template reference {0}", templateID));

                //set fileOutput details
                WordFile fileOutput = new WordFile(tipstaffRecord, Server.MapPath("~/Documents/"), template);

                //Create XML object for Template
                XmlDocument xDoc = new XmlDocument();

                //Merge Data
                xDoc.InnerXml = mergeDataA(template, tipstaffRecord, applicant);

                ////Save resulting document 
                //xDoc.Save(fileOutput.fullName); //Save physical file
                //if (!System.IO.File.Exists(fileOutput.fullName)) throw new FileNotFoundException(string.Format("File {0} could not be created", fileOutput.fileName));

                //WE DON'T NEED THIS AS WE DON'T SAVE GENERATED FILES!!!!
                ////Create and add a Document to TipstaffRecord
                //Document doc = new Document();
                //doc.binaryFile = genericFunctions.ConvertToBytes(xDoc);
                //doc.mimeType = "application/msword";
                //doc.fileName = fileOutput.fileName;
                //doc.countryID = 244; //UK!
                //doc.nationalityID = 27; //English
                //doc.documentTypeID = 1;     //generated
                //doc.documentStatusID = 1;   //generated
                //doc.documentReference = template.templateName;
                //doc.templateID = template.templateID;
                //doc.createdOn = DateTime.Now;
                //doc.createdBy = User.Identity.Name;
                //tipstaffRecord.Documents.Add(doc);

                ////Save Changes
                //db.SaveChanges();

                //Return saved document
                //return File(fileOutput.fullName, "application/doc", fileOutput.fileName); // return physical file 
                return File(genericFunctions.ConvertToBytes(xDoc), "application/msword", fileOutput.fileName); //return byte version
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in TemplateController in Create8 method, for user {((CPrincipal)User).UserID}");

                ErrorModel model = new ErrorModel(2);
                model.ErrorMessage = ex.Message;
                TempData["ErrorModel"] = model;
                return RedirectToAction("IndexByModel", "Error", model ?? null);
                //Note: working redirect to view with Model
                //Note: Working error redirect
            }
        }

        private string mergeData(Template template, TipstaffRecord tipstaffRecord, Solicitor solicitor)
        {
            string result = mergeData(template, tipstaffRecord);
            if (tipstaffRecord.NPO == null)
            {
                result = result.Replace("||NPOREFERENCE||", "");
            }
            else
            {
                result = result.Replace("||NPOREFERENCE||", tipstaffRecord.NPO);
            }
            if (tipstaffRecord.addresses != null)
            {
                string addresses = "";
                foreach (Address a in tipstaffRecord.addresses)
                {
                    addresses += a.printAddressMultiLine + "<w:br/><w:br/>";
                }
                if (addresses == "")
                {
                    result = result.Replace("||POSSIBLEADDRESSES||", "");
                }
                else
                {
                    result = result.Replace("||POSSIBLEADDRESSES||", addresses);
                }
            }
            if (genericFunctions.TypeOfTipstaffRecord(tipstaffRecord) != "Warrant") //Check PNCIDs
            { 
                string pncids = "";
                ChildAbduction ca = (ChildAbduction)tipstaffRecord;
                foreach (Child c in ca.children)
                {
                    if (c.PNCID != null && c.PNCID != "")
                    {
                        pncids += c.PNCID + "<w:br/>";
                    }
                }
                if (pncids == "")
                {
                    result = result.Replace("||PNCIDS||", "");
                }
                else
                {
                    result = result.Replace("||PNCIDS||", pncids);
                }
            }
            if (solicitor == null)
            {
                result = result.Replace("||ADDRESSEENAME||", "");
                result = result.Replace("||ADDRESS||", "Add Address here");
            }
            else
            {
                result = result.Replace("||ADDRESSEENAME||", solicitor.AddresseeName);
                if (solicitor.SolicitorFirm != null)
                {
                    result = result.Replace("||ADDRESS||", solicitor.SolicitorFirm.printAddressMultiLine);
                }
                else
                {
                    result = result.Replace("||ADDRESS||", "");
                }
            }
            return result;
        }

        private string mergeDataA(Template template, TipstaffRecord tipstaffRecord, Applicant applicant)
        {
            string result = mergeData(template, tipstaffRecord);
            if (tipstaffRecord.NPO == null)
            {
                result = result.Replace("||NPOREFERENCE||", "");
            }
            else
            {
                result = result.Replace("||NPOREFERENCE||", tipstaffRecord.NPO);
            }
            if (tipstaffRecord.addresses != null)
            {
                string addresses = "";
                foreach (Address a in tipstaffRecord.addresses)
                {
                    addresses += a.printAddressMultiLine + "<w:br/><w:br/>";
                }
                if (addresses == "")
                {
                    result = result.Replace("||POSSIBLEADDRESSES||", "");
                }
                else
                {
                    result = result.Replace("||POSSIBLEADDRESSES||", addresses);
                }
            }
            if (genericFunctions.TypeOfTipstaffRecord(tipstaffRecord) != "Warrant") //Check PNCIDs
            {
                string pncids = "";
                ChildAbduction ca = (ChildAbduction)tipstaffRecord;
                foreach (Child c in ca.children)
                {
                    if (c.PNCID != null && c.PNCID != "")
                    {
                        pncids += c.PNCID + "<w:br/>";
                    }
                }
                if (pncids == "")
                {
                    result = result.Replace("||PNCIDS||", "");
                }
                else
                {
                    result = result.Replace("||PNCIDS||", pncids);
                }
            }
            if (applicant == null)
            {
                result = result.Replace("||ADDRESSEENAME||", "");
                result = result.Replace("||ADDRESS||", "Add Address here");
            }
            else
            {
                result = result.Replace("||ADDRESSEENAME||", applicant.fullname);
                
                if (applicant.printAddressMultiLine != null)
                {
                    result = result.Replace("||ADDRESS||", applicant.printAddressMultiLine);
                }
                else
                {
                    result = result.Replace("||ADDRESS||", "");
                }
            }
            return result;
        }

        private string mergeData(Template template, TipstaffRecord tipstaffRecord)
        {

            string result = _s3API.ReadS3Object("tipstaff", "templates", Path.GetFileName(template.filePath)); //template.templateXML;
            int kids=1;
            //merge generic fields
            result = result.Replace("||DATE||", DateTime.Now.ToShortDateString());
            result = result.Replace("||TIME||", DateTime.Now.ToShortTimeString());
            result = result.Replace("||NOW||", DateTime.Now.ToString("dd/MM/yy @ HH:mm"));
            result = result.Replace("||UNIQUERECORDID||", tipstaffRecord.UniqueRecordID);
            result = result.Replace("||USERNAME||", User.Identity.Name);

            if (tipstaffRecord.NPO == null)
            {
                result = result.Replace("||NPOREFERENCE||", "");
            }
            else
            {
                result = result.Replace("||NPOREFERENCE||", tipstaffRecord.NPO);
            }
            if (tipstaffRecord.addresses != null)
            {
                string addresses = "";
                foreach (Address a in tipstaffRecord.addresses)
                {
                    addresses += a.printAddressMultiLine + "<w:br/><w:br/>";
                }
                if (addresses == "")
                {
                    result = result.Replace("||POSSIBLEADDRESSES||", "");
                }
                else
                {
                    result = result.Replace("||POSSIBLEADDRESSES||", addresses);
                }
            }
            else
            {
                result = result.Replace("||POSSIBLEADDRESSES||", "");
            }
            if (tipstaffRecord.Respondents != null)
            {
                string respNames = "";
                foreach (Respondent r in tipstaffRecord.Respondents)
                {
                    respNames += r.PoliceDisplayName + " | ";
                }
                if (respNames == "")
                {
                    result = result.Replace("||RESPONDENTSNAME||", "<<Please enter respondent's name");
                }
                else
                {
                    respNames = respNames.Substring(0, respNames.Length - 2);
                    result = result.Replace("||RESPONDENTSNAME||", respNames);
                }
            }
            else
            {
                result = result.Replace("||RESPONDENTSNAME||", "<<Please enter respondent's name");
            }

            foreach (Address addr in tipstaffRecord.addresses)
            {
                result = result.Replace("||ADDRESSES||", SecurityElement.Escape(addr.PrintAddressSingleLine) + "<w:br/>||ADDRESSES||");
                result = result.Replace("||ADDRESSBLOCK||", addr.xmlBlock + "||ADDRESSBLOCK||");
            }
            result = result.Replace("||ADDRESSES||", "");
            result = result.Replace("||ADDRESSBLOCK||", "");

            if (genericFunctions.TypeOfTipstaffRecord(tipstaffRecord)=="ChildAbduction" && template.Discriminator=="ChildAbduction")
            {
                ChildAbduction ca = (ChildAbduction)tipstaffRecord;
                PropertyInfo[] properties = typeof(ChildAbduction).GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    var propValue="";
                    object value = property.GetValue(ca, null);
                    if (value != null)
                    {
                        Type type = value.GetType();
                        if (type == typeof(string) || type == typeof(int))
                        {
                            propValue = value.ToString();
                        }
                        else if (type == typeof(DateTime))
                        {
                            propValue = ((DateTime)value).ToShortDateString();
                        }
                        else if (type == typeof(object))
                        {
                            //loop through properties of sub object
                            System.Diagnostics.Debug.Print(propValue.ToString());
                        }
                    }
                    result = result.Replace(string.Format("||{0}||",property.Name.ToUpper()), SecurityElement.Escape(propValue));
                }
                //child blocks
                foreach (Child child in ca.children)
                {
                    result = result.Replace("||CHILDBLOCK||", child.xmlBlock.Replace("||CHILDNUMBER||",kids.ToString()) + "||CHILDBLOCK||");
                    kids++;
                }
                result = result.Replace("||MULTICHILD||", ca.children.Count() > 1 ? "children" : "child");
                result = result.Replace("||CHILDBLOCK||", "");
                result = result.Replace("||CHILDNUMBER||", "");
                //respondent block
                foreach (Respondent resp in tipstaffRecord.Respondents)
                {
                    result = result.Replace("||RESPONDENTBLOCK||", resp.xmlBlock + "||RESPONDENTBLOCK||");        
                }
                result = result.Replace("||RESPONDENTBLOCK||", "");
                result = result.Replace("||MULTIRESP||", ca.Respondents.Count()>1?"people":"person");
                string pncids = "";
                foreach (Respondent r in ca.Respondents)
                {
                    if (r.PNCID != null && r.PNCID != "")
                    {
                        pncids += "(Respondent) " + r.PoliceDisplayName + " " + r.PNCID + "<w:br/>";
                    }
                }
                foreach (Child c in ca.children)
                {
                    if (c.PNCID != null && c.PNCID != "")
                    {
                        pncids += "(Child) " + c.PoliceDisplayName + " " + c.PNCID + "<w:br/>";
                    }
                }
                if (pncids == "")
                {
                    result = result.Replace("||PNCIDS||", "");
                }
                else
                {
                    result = result.Replace("||PNCIDS||", pncids);
                }
            }
            else if (template.Discriminator == "Warrant")
            {
                Warrant warrant = tipstaffRecord as Warrant;
                //result = result.Replace("||DIVISION||", warrant.division.Detail);
                PropertyInfo[] properties = typeof(Warrant).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    var propValue = "";
                    object value = property.GetValue(warrant, null);
                    if (value != null)
                    {
                        Type type = value.GetType();
                        if (type == typeof(string) || type == typeof(int))
                        {
                            propValue = value.ToString();
                        }
                        else if (type == typeof(DateTime))
                        {
                            propValue = ((DateTime)value).ToShortDateString();
                        }
                        else if (type == typeof(object))
                        {
                            //loop through properties of sub object
                            System.Diagnostics.Debug.Print(propValue.ToString());
                        }
                    }
                    result = result.Replace(string.Format("||{0}||", property.Name.ToUpper()), SecurityElement.Escape(propValue));
                }
                if (warrant.Respondents.Count() == 1)
                {
                    PropertyInfo[] respProp = typeof(Respondent).GetProperties();
                    foreach (PropertyInfo property in respProp)
                    {
                        var propValue = "";
                        object value = property.GetValue(warrant.Respondents.FirstOrDefault(), null);
                        if (value != null)
                        {
                            Type type = value.GetType();
                            if (type == typeof(string) || type == typeof(int))
                            {
                                propValue = value.ToString();
                            }
                            else if (type == typeof(DateTime))
                            {
                                propValue = ((DateTime)value).ToShortDateString();
                            }
                            else if (type == typeof(object))
                            {
                                //loop through properties of sub object
                                System.Diagnostics.Debug.Print(propValue.ToString());
                            }
                        }
                        result = result.Replace(string.Format("||{0}||", property.Name.ToUpper()), SecurityElement.Escape(propValue));
                    }
                    result = result.Replace("||GENDER.DETAIL||", warrant.Respondents.FirstOrDefault().gender.Detail);
                    result = result.Replace("||NATIONALITY.DETAIL||", warrant.Respondents.FirstOrDefault().nationality.Detail);
                    result = result.Replace("||COUNTRY.DETAIL||", warrant.Respondents.FirstOrDefault().country.Detail);
                    result = result.Replace("||SKINCOLOUR.DETAIL||", warrant.Respondents.FirstOrDefault().skinColour.Detail);
                }
   
            }
            return result;
        }

   }

}

