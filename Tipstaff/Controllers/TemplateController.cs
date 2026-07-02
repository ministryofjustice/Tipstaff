using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Security;
using System.Data.Entity.Validation;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using TPLibrary.Logger;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class TemplateController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;

        private readonly ICloudWatchLogger _logger;

        public TemplateController(ICloudWatchLogger logger)
        {
            _logger = logger;
        }
        //
        // GET: /Template/
        public ActionResult Create(int tipstaffRecordID, int templateID)
        {
            try
            {
                //Get TipstaffRecord from warrantID
                TipstaffRecord tipstaffRecord = db.TipstaffRecord.Find(tipstaffRecordID);
                if (tipstaffRecord.caseStatus.sequence > 3)
                {
                    TempData["UID"] = tipstaffRecord.UniqueRecordID;
                    return RedirectToAction("ClosedFile", "Error");
                }
                //Get Template from templateID
                Template template = db.Templates.Find(templateID);
                if (template == null) throw new FileLoadException(string.Format("No database record found for template reference {0}",templateID));

                //set fileOutput details
                WordFile fileOutput = new WordFile(tipstaffRecord, Server.MapPath("~/Documents/"), template);

                //Merge Data
                byte[] fileBytes = BuildPlaceholderFields(template, tipstaffRecord, null, null);

                //Create and add a Document to TipstaffRecord
                Document doc = CreateDocument(fileOutput, template, fileBytes);
                tipstaffRecord.Documents.Add(doc);

                //Save Changes
                db.SaveChanges();

                //Return saved document
                //return File(fileOutput.fullName, "application/doc", fileOutput.fileName); // return physical file 
                return File(doc.binaryFile, doc.mimeType, doc.fileName); //return byte version
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

        public ActionResult Create4(int tipstaffRecordID, int templateID, int solicitorID)
        {
            try
            {
                //get solicitor from solicitorID
                Solicitor solicitor = db.Solicitors.Find(solicitorID);

                //Get TipstaffRecord from warrantID
                TipstaffRecord tipstaffRecord = db.TipstaffRecord.Find(tipstaffRecordID);
                if (tipstaffRecord.caseStatus.sequence > 3)
                {
                    TempData["UID"] = tipstaffRecord.UniqueRecordID;
                    return RedirectToAction("ClosedFile", "Error");
                }

                //Get Template from templateID
                Template template = db.Templates.Find(templateID);
                if (template == null) throw new FileLoadException(string.Format("No database record found for template reference {0}",templateID));

                //set fileOutput details
                WordFile fileOutput = new WordFile(tipstaffRecord, Server.MapPath("~/Documents/"),template);

                //Merge Data
                var placeholderFields = BuildPlaceholderFields(template, tipstaffRecord, solicitor, null);
                byte[] fileBytes = GenerateDocument(template.TemplateDOTX, placeholderFields);

                //Create and add a Document to TipstaffRecord
                Document doc = CreateDocument(fileOutput, template, fileBytes);
                tipstaffRecord.Documents.Add(doc);

                //Save Changes
                db.SaveChanges();

                //Return saved document
                //return File(fileOutput.fullName, "application/doc", fileOutput.fileName); // return physical file 
                return File(doc.binaryFile, doc.mimeType, doc.fileName); //return byte version
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

        public ActionResult Create8(int tipstaffRecordID, int templateID, int applicantID)
        {
            try
            {
                //get applicant from applicantID
                Applicant applicant = db.Applicants.Find(applicantID);

                //Get TipstaffRecord from warrantID
                TipstaffRecord tipstaffRecord = db.TipstaffRecord.Find(tipstaffRecordID);
                if (tipstaffRecord.caseStatus.sequence > 3)
                {
                    TempData["UID"] = tipstaffRecord.UniqueRecordID;
                    return RedirectToAction("ClosedFile", "Error");
                }

                //Get Template from templateID
                Template template = db.Templates.Find(templateID);
                if (template == null) throw new FileLoadException(string.Format("No database record found for template reference {0}", templateID));

                //set fileOutput details
                WordFile fileOutput = new WordFile(tipstaffRecord, Server.MapPath("~/Documents/"), template);

                //Create XML object for Template
                byte[] fileBytes = BuildPlaceholderFields(template, tipstaffRecord, null, applicant);

                //Create and add a Document to TipstaffRecord
                Document doc = CreateDocument(fileOutput, template, fileBytes);
                tipstaffRecord.Documents.Add(doc);

                //Save Changes
                db.SaveChanges();

                //Return saved document
                return File(doc.binaryFile, doc.mimeType, doc.fileName); //return byte version
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

        private Dictionary<string, string> BuildPlaceholderFields(Template template, TipstaffRecord tipstaffRecord, Solicitor solicitor, Applicant applicant)
        {
            var ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            var ukTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ukTimeZone);

            var placeholderFields = new Dictionary<string, string>
            {
                //merge generic fields
                { "||DATE||", ukTime.ToShortDateString() },
                { "||TIME||", ukTime.ToShortTimeString() },
                { "||NOW||", ukTime.ToString("dd/MM/yy @ HH:mm") },
                { "||UNIQUERECORDID||", tipstaffRecord.UniqueRecordID },
                { "||USERNAME||", User.Identity.Name },
                { "||NPOREFERENCE||", tipstaffRecord.NPO ?? string.Empty }
            };

            // Possible Addresses (multi-line, each address separated by double line break)
            if (tipstaffRecord.addresses != null && tipstaffRecord.addresses.Any())
            {
                var addressLines = tipstaffRecord.addresses.Select(a => a.printAddressMultiLine);
                placeholderFields.Add("||POSSIBLEADDRESSES||", string.Join("\n\n", addressLines));
            }
            else
            {
                placeholderFields.Add("||POSSIBLEADDRESSES||", "");
            }

            if (tipstaffRecord.Respondents != null && tipstaffRecord.Respondents.Any())
            {
                string respNames = string.Join(" | ", tipstaffRecord.Respondents.Select(r => r.PoliceDisplayName));
                placeholderFields.Add("||RESPONDENTSNAME||", respNames);
            }
            else
            {
                placeholderFields.Add("||RESPONDENTSNAME||", "<<Please enter respondent's name");
            }

            // Addresses single line list (each on a new line)
            if (tipstaffRecord.addresses != null && tipstaffRecord.addresses.Any())
            {
                var addressesList = tipstaffRecord.addresses
                    .Select(a => a.PrintAddressSingleLine);
                placeholderFields.Add("||ADDRESSES||", string.Join("\n", addressesList));
            }
            else
            {
                placeholderFields.Add("||ADDRESSES||", "");
            }

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
                    placeholderFields[string.Format("||{0}||", property.Name.ToUpper())] = propValue;
                }

                placeholderFields.Add("||MULTICHILD||", ca.children.Count() > 1 ? "children" : "child");
                placeholderFields.Add("||MULTIRESP||", ca.Respondents.Count() > 1 ? "people" : "person");
                
                // PNCIDs
                var pncidLines = new List<string>();
                foreach (Respondent r in ca.Respondents)
                {
                    if (!string.IsNullOrEmpty(r.PNCID))
                    {
                        pncidLines.Add("(Respondent) " + r.PoliceDisplayName + " \u2013 " + r.PNCID + " \u2013 " + r.DateofBirthDisplay);
                    }
                }
                foreach (Child c in ca.children)
                {
                    if (!string.IsNullOrEmpty(c.PNCID))
                    {
                        pncidLines.Add("(Child) " + c.PoliceDisplayName + " \u2013 " + c.PNCID + " \u2013 " + c.DateofBirthDisplay);
                    }
                }
                placeholderFields.Add("||PNCIDS||", string.Join("\n", pncidLines));
            }
            else if (template.Discriminator == "Warrant")
            {
                Warrant warrant = tipstaffRecord as Warrant;
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
                    placeholderFields[string.Format("||{0}||", property.Name.ToUpper())] = propValue;
                }

                if (warrant.Respondents.Count() == 1)
                {
                    var resp = warrant.Respondents.FirstOrDefault();
                    PropertyInfo[] respProp = typeof(Respondent).GetProperties();
                    foreach (PropertyInfo property in respProp)
                    {
                        var propValue = "";
                        object value = property.GetValue(resp, null);
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
                        string key = string.Format("||{0}||", property.Name.ToUpper());
                        if (!placeholderFields.ContainsKey(key))
                        {
                            placeholderFields[key] = propValue;
                        }
                    }

                    placeholderFields.Add("||GENDER.DETAIL||", resp.gender.detail);
                    placeholderFields.Add("||NATIONALITY.DETAIL||", resp.nationality.Detail);
                    placeholderFields.Add("||COUNTRY.DETAIL||", resp.country.Detail);
                    placeholderFields.Add("||SKINCOLOUR.DETAIL||", resp.SkinColour.Detail);

                    // PNCID for warrant respondent
                    string pncid = !string.IsNullOrEmpty(resp.PNCID) ? resp.PNCID : "";
                    placeholderFields.Add("||PNCID||", pncid);
                }
            }

            // Check PNCIDs for non-Warrant types (when not already handled above in ChildAbduction)
            if (genericFunctions.TypeOfTipstaffRecord(tipstaffRecord) != "Warrant" && !placeholderFields.ContainsKey("||PNCIDS||"))
            {
                string pncids = "";
                ChildAbduction ca = (ChildAbduction)tipstaffRecord;
                foreach (Child c in ca.children)
                {
                    if (!string.IsNullOrEmpty(c.PNCID))
                    {
                        pncids += c.PNCID + "\n";
                    }
                }
                placeholderFields.Add("||PNCIDS||", pncids.TrimEnd('\n'));
            }

            // Solicitor fields
            if (solicitor == null)
            {
                placeholderFields.Add("||ADDRESSEENAME||", "");
                placeholderFields.Add("||ADDRESS||", "Add Address here");
            }
            else
            {
                placeholderFields.Add("||ADDRESSEENAME||", solicitor.AddresseeName);
                if (solicitor.SolicitorFirm != null)
                {
                    placeholderFields.Add("||ADDRESS||", solicitor.SolicitorFirm.printAddressMultiLine);
                }
                else
                {
                    placeholderFields.Add("||ADDRESS||", "");
                }
            }

            // Applicant fields (overrides solicitor address if applicant is provided)
            if (applicant != null)
            {
                placeholderFields["||ADDRESSEENAME||"] = applicant.fullname;
                placeholderFields["||ADDRESS||"] = applicant.printAddressMultiLine ?? "";
            }

            return placeholderFields;
            
        }

        private Tipstaff.Models.Document CreateDocument(WordFile fileOutput, Template template, byte[] fileBytes)
        {
            return new Tipstaff.Models.Document
            {
                binaryFile = fileBytes,
                mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                fileName = fileOutput.fileName,
                countryID = 244, //UK!
                nationalityID = 27,
                documentTypeID = 1, //generated
                documentStatusID = 1, //generated
                documentReference = template.templateName,
                templateID = template.templateID,
                createdOn = DateTime.Now,
                createdBy = User.Identity.Name
            };
        }

        private void ReplaceTextWithLineBreaks(Body body, string placeholder, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                foreach (var text in body.Descendants<Text>().Where(t => t.Text.Contains(placeholder)).ToList())
                {
                    text.Text = text.Text.Replace(placeholder, string.Empty);
                }
                return;
            }

            var lines = value.Split(new[] { "\n" }, StringSplitOptions.None);

            foreach (var text in body.Descendants<Text>().Where(t => t.Text.Contains(placeholder)).ToList())
            {
                var run = text.Parent as Run;
                if (run == null) continue;

                text.Text = text.Text.Replace(placeholder, lines[0]);

                for (int i = 1; i < lines.Length; i++)
                {
                    run.Append(new Break());
                    run.Append(new Text(lines[i]));
                }
            }
        }

        private byte[] GenerateDocument(byte[] templateBytes, Dictionary<string, string> replacementFields)
        {
            using (var outputStream = new MemoryStream())
            {
                outputStream.Write(templateBytes, 0, templateBytes.Length);
                outputStream.Position = 0;

                using (var wordDoc = WordprocessingDocument.Open(outputStream, true))
                {
                    // Convert from template to document
                    wordDoc.ChangeDocumentType(WordprocessingDocumentType.Document);

                    var body = wordDoc.MainDocumentPart.Document.Body;

                    // Handle multi-line placeholders with line breaks
                    string[] multiLinePlaceholders = new[]
                    {
                        "||ADDRESS||",
                        "||POSSIBLEADDRESSES||",
                        "||ADDRESSES||",
                        "||PNCIDS||",
                        "||PNCID||"
                    };

                    foreach (var placeholder in multiLinePlaceholders)
                    {
                        if (replacementFields.ContainsKey(placeholder))
                        {
                            ReplaceTextWithLineBreaks(body, placeholder, replacementFields[placeholder]);
                            replacementFields.Remove(placeholder);
                        }
                    }

                    // Standard replacements
                    foreach (var text in body.Descendants<Text>())
                    {
                        foreach (var replacement in replacementFields)
                        {
                            if (text.Text.Contains(replacement.Key))
                            {
                                text.Text = text.Text.Replace(replacement.Key, replacement.Value);
                            }
                        }
                    }

                    wordDoc.MainDocumentPart.Document.Save();
                }

                return outputStream.ToArray();
            }
        }

   }

}

