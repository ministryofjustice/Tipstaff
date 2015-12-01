using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using PagedList;

using Tipstaff.Models;
using System.Data.Entity.Infrastructure;
using System.Web.UI;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    public class RespondentController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;

        //
        // GET: /Respondent/Details/5

        public ViewResult Details(int id)
        {
            Respondent respondent = db.Respondents.Find(id);
            return View(respondent);
        }

        //
        // GET: /Respondent/Create

        public ActionResult Create(int id, bool initial=false)
        {
            RespondentCreationModel model = new RespondentCreationModel(id);
            if (model.tipstaffRecord.caseStatus.sequence > 3)
            {
                TempData["UID"] = model.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }
            if (genericFunctions.TypeOfTipstaffRecord(model.tipstaffRecord) == "Warrant" && model.tipstaffRecord.Respondents.Count() == 1)
            {
                //redirect to error
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("Record {0} already has a contemnor linked, please check your records", model.tipstaffRecord.UniqueRecordID);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", errModel ?? null);
            }
            model.initial = initial;
            return View(model);
        } 

        //
        // POST: /Respondent/Create

        [HttpPost]
        public ActionResult Create(RespondentCreationModel model, string submitButton)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {

                TipstaffRecord tr = db.TipstaffRecord.Find(model.tipstaffRecordID);
                //if (genericFunctions.TypeOfTipstaffRecord(tr) == "Warrant")
                if (tr is Warrant)
                {
                    Warrant w = (Warrant)tr;
                    w.RespondentName = model.respondent.PoliceDisplayName;
                    w.Respondents.Add(model.respondent);
                }
                else
                {
                    tr.Respondents.Add(model.respondent);
                }
                db.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    string url = string.Format("window.location='{0}';", Url.Action("Details", genericFunctions.TypeOfTipstaffRecord(tr), new { id = model.tipstaffRecordID }));
                    return JavaScript(url);
                }
                else
                {
                    switch (submitButton)
                    {
                        case "Save,add new Respondent":
                            return RedirectToAction("Create", "Respondent", new { id = model.tipstaffRecordID, initial = model.initial });
                        case null:
                        default:
                            return RedirectToAction("Details", genericFunctions.TypeOfTipstaffRecord(tr), new { id = model.tipstaffRecordID });
                    }

                }
            }
            catch (DbUpdateException)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_createRespondentForRecord", model);
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = genericFunctions.GetLowestError(ex);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", errModel ?? null);
            }
        }
        
        //
        // GET: /Respondent/Edit/5
 
        public ActionResult Edit(int id)
        {
            RespondentCreationModel model = new RespondentCreationModel();
            model.respondent = db.Respondents.Find(id);
            model.tipstaffRecordID = model.respondent.tipstaffRecordID;
            if (model.respondent == null)
            {
                ErrorModel errModel = new ErrorModel();
                errModel.ErrorMessage = "No respondent with that ID can be found";
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", errModel ?? null);
            }
            if (model.respondent.tipstaffRecord.caseStatus.sequence > 3)
            {
                TempData["UID"] = model.respondent.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }
            return View(model);
        }

        //
        // POST: /Respondent/Edit/5

        [HttpPost]
        public ActionResult Edit(RespondentCreationModel model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model.respondent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", genericFunctions.TypeOfTipstaffRecord(model.respondent.tipstaffRecordID), new { id = model.respondent.tipstaffRecordID });
            }
            return View(model);
        }

        [OutputCache(Location = OutputCacheLocation.Server, Duration = 10)]
        public PartialViewResult ListRespondentsByRecord(int id, int? page)
        {
            ListRespondentsByTipstaffRecord model = new ListRespondentsByTipstaffRecord();
            try
            {
                ChildAbduction ca = db.ChildAbductions.Find(id);
                model.tipstaffRecordID = ca.tipstaffRecordID;
                model.Respondents = ca.Respondents.ToXPagedList<Respondent>(page ?? 1, 8);
            }
            catch
            {
                //do nothing!  Return empty model
            }
            return PartialView("_ListRespondentsByRecord", model);
        }
        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            DeleteRespondent model = new DeleteRespondent(id);
            if (model == null)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("Respondent record: {0} has been deleted, please raise a help desk call if you think this has been deleted in error.", id);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            if (model.Respondent.tipstaffRecord.caseStatus.sequence > 3)
            {
                TempData["UID"] = model.Respondent.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }

            return View(model);
        }

        //
        // POST: /Respondent/Delete/5

        [HttpPost, ActionName("Delete"), AuthorizeRedirect(Roles = "Admin")]
        public ActionResult DeleteConfirmed(DeleteRespondent model)
        {
            model.Respondent = db.Respondents.Find(model.DeleteModelID);
            int tipstaffRecordID = model.Respondent.tipstaffRecordID;
            string controller = genericFunctions.TypeOfTipstaffRecord(tipstaffRecordID);
            db.Respondents.Remove(model.Respondent);
            db.SaveChanges();
            //get the Audit Event we just created 
            string recDeleted = model.DeleteModelID.ToString();
            AuditEvent AE = db.AuditEvents.Where(a => a.auditEventDescription.AuditDescription == "Respondent deleted" && a.RecordChanged == recDeleted).OrderByDescending(a => a.EventDate).Take(1).Single();
            //add a deleted reason
            AE.DeletedReasonID = model.DeletedReasonID;
            //and save again
            db.SaveChanges();
            return RedirectToAction("Details", controller, new { id = tipstaffRecordID });
        }

    }
}