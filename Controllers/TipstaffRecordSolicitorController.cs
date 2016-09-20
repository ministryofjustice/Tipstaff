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

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    public class TipstaffRecordSolicitorController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;
        
        //
        // POST: /TipstaffRecordSolicitor/Create
                
        public ActionResult Create(int tipstaffRecord, int solicitor)
        {
            TipstaffRecordSolicitor tipstaffrecordsolicitor=new TipstaffRecordSolicitor();
            tipstaffrecordsolicitor.solicitor=db.Solicitors.Find(solicitor);
            tipstaffrecordsolicitor.tipstaffRecord = db.TipstaffRecord.Find(tipstaffRecord);
            if (tipstaffrecordsolicitor.tipstaffRecord.caseStatus.sequence > 3)
            {
                TempData["UID"] = tipstaffrecordsolicitor.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }

            try
            {
                //throw new DbUpdateException(");
                db.TipstaffRecordSolicitors.Add(tipstaffrecordsolicitor);
                db.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    string url = string.Format("window.location='{0}';", Url.Action("Details", genericFunctions.TypeOfTipstaffRecord(tipstaffrecordsolicitor.tipstaffRecord), new { id = tipstaffRecord }));
                    //return JavaScript("location.reload(true)");
                    return JavaScript(url);
                }
                else
                {
                    return RedirectToAction("Details", genericFunctions.TypeOfTipstaffRecord(tipstaffrecordsolicitor.tipstaffRecord), new { id = tipstaffRecord });
                }
            }
            catch (DbUpdateException ex)
            {
                TipstaffRecordSolicitorErrorViewModel model = new TipstaffRecordSolicitorErrorViewModel();
                model.tipstaffrecordsolicitor = tipstaffrecordsolicitor;

                if (ex.InnerException.InnerException.Message.StartsWith("Violation of PRIMARY"))
                {
                    model.ErrorMessage = "The chosen solicitor is already linked to this record";
                }
                else
                {
                    model.ErrorMessage = ex.InnerException.InnerException.Message;
                }
                TempData["TRSError"] = model;
                return RedirectToAction("Error", "TipstaffRecordSolicitor");
            }
            catch (Exception ex)
            {
                ErrorModel model = new ErrorModel();
                model.ErrorMessage = ex.Message;
                return View("Error", model);
            }
        }

        public ActionResult Error()
        {
            TipstaffRecordSolicitorErrorViewModel model = (TipstaffRecordSolicitorErrorViewModel)TempData["TRSError"];
            return View(model);
        }

        public ActionResult Delete(int tipstaffRecordID, int solicitorID)
        {
            DeleteTipstaffRecordSolicitor model = new DeleteTipstaffRecordSolicitor();
            model.TipstaffRecordSolicitor = db.TipstaffRecordSolicitors.Single(t=>t.tipstaffRecordID==tipstaffRecordID && t.solicitorID==solicitorID);
            model.DeleteModelID = tipstaffRecordID;
            if (model.TipstaffRecordSolicitor == null)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("The solicitor {0} has been deleted from {1}, please raise a help desk call if you think this has been deleted in error.", model.TipstaffRecordSolicitor.solicitor.solicitorName, model.TipstaffRecordSolicitor.tipstaffRecord.UniqueRecordID);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }
        //
        // POST: /TipstaffRecordSolicitor/Delete/5/5

        [HttpPost, ActionName("Delete"), AuthorizeRedirect(Roles = "Admin")]
        public ActionResult DeleteConfirmed(DeleteTipstaffRecordSolicitor model)
        {

            model.TipstaffRecordSolicitor = db.TipstaffRecordSolicitors.Single(t => t.tipstaffRecordID == model.TipstaffRecordSolicitor.tipstaffRecordID && t.solicitorID == model.TipstaffRecordSolicitor.solicitorID);
            string controller = genericFunctions.TypeOfTipstaffRecord(model.TipstaffRecordSolicitor.tipstaffRecordID);
            db.TipstaffRecordSolicitors.Remove(model.TipstaffRecordSolicitor);
            db.SaveChanges();
            string recDeleted = model.DeleteModelID.ToString();
            var AE = db.AuditEvents.Where(a => a.auditEventDescription.AuditDescription == "TipstaffRecordSolicitor deleted" && a.RecordChanged == recDeleted).OrderByDescending(a => a.EventDate).Take(1).Single();
            //add a deleted reason
            AE.DeletedReasonID = model.DeletedReasonID;
            //and save again
            db.SaveChanges();
            return RedirectToAction("Details", controller, new { id = model.TipstaffRecordSolicitor.tipstaffRecordID });
        }
        //public ActionResult DeleteConfirmed(int tipstaffRecordID, int solicitorID)
        //{

        //    TipstaffRecordSolicitor model = db.TipstaffRecordSolicitors.Single(t => t.tipstaffRecordID == tipstaffRecordID && t.solicitorID == solicitorID);
        //    string controller = genericFunctions.TypeOfTipstaffRecord(tipstaffRecordID);
        //    db.TipstaffRecordSolicitors.Remove(model);
        //    //db.Entry(model).State = EntityState.Deleted;
        //    db.SaveChanges();
        //    return RedirectToAction("Details", controller, new { id = tipstaffRecordID });
        //}
    }
}