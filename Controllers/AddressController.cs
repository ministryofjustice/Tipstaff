using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Web.UI;
using PagedList;
namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    public class AddressController : Controller
    {

        private TipstaffDB db = myDBContextHelper.CurrentContext;
        //
        // GET: /Address/

        public ActionResult Details(int id)
        {
            Address model = db.Addresses.Find(id);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            Address model = db.Addresses.Find(id);
            if (model == null)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("Address {0} has been deleted, please raise a help desk call if you think this has been deleted in error.", id);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            if (model.tipstaffRecord.caseStatus.sequence > 3)
            {
                TempData["UID"] = model.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Address address)
        {
            if (ModelState.IsValid)
            {
                db.Entry(address).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", genericFunctions.TypeOfTipstaffRecord(address.tipstaffRecordID), new { id = address.tipstaffRecordID });
            }
            return View(address);
        }

        public ActionResult Create(int id)
        {
            AddressCreationModel model = new AddressCreationModel(id);
            if (model.tipstaffRecord.caseStatus.sequence > 3)
            {
                TempData["UID"] = model.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(AddressCreationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                TipstaffRecord tr = db.TipstaffRecord.Find(model.tipstaffRecordID);
                string controller = genericFunctions.TypeOfTipstaffRecord(tr);
                //do stuff
                tr.addresses.Add(model.address);
                db.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    string url = string.Format("window.location='{0}';", Url.Action("Details", controller, new { id = model.tipstaffRecordID }));
                    return JavaScript(url);
                }
                else
                {
                    return RedirectToAction("Details", controller, new { id = model.tipstaffRecordID });
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
        [OutputCache(Location = OutputCacheLocation.Server, Duration = 180)]
        public PartialViewResult ListAddressesByRecord(int id, int? page)
        {
            TipstaffRecord w = db.TipstaffRecord.Find(id);

            ListAddressesByTipstaffRecord model = new ListAddressesByTipstaffRecord();
            model.tipstaffRecordID = w.tipstaffRecordID;
            model.TipstaffRecordClosed = w.caseStatusID > 2;
            model.Addresses = w.addresses.ToXPagedList<Address>(page ?? 1, 8);
            //model.Addresses = w.addresses.ToPagedList<Address>(page ?? 1, 8);
            return PartialView("_ListAddressesByRecord", model);
        }

        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            DeleteAddress model = new DeleteAddress(id);
            if (model.Address == null)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("Address {0} has been deleted, please raise a help desk call if you think this has been deleted in error.", id);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }

        //
        // POST: /Address/Delete/5

        [HttpPost, ActionName("Delete"), AuthorizeRedirect(Roles = "Admin")]
        public ActionResult DeleteConfirmed(DeleteAddress model)
        {
            model.Address = db.Addresses.Find(model.DeleteModelID);
            int tipstaffRecordID = model.Address.tipstaffRecordID;
            string controller = genericFunctions.TypeOfTipstaffRecord(tipstaffRecordID);
            db.Addresses.Remove(model.Address);
            db.SaveChanges();
            //get the Audit Event we just created 
            string recDeleted = model.DeleteModelID.ToString();
            AuditEvent AE = db.AuditEvents.Where(a => a.auditEventDescription.AuditDescription == "Address deleted" && a.RecordChanged == recDeleted).OrderByDescending(a => a.EventDate).Take(1).Single();
            //add a deleted reason
            AE.DeletedReasonID = model.DeletedReasonID;
            //and save again
            db.SaveChanges();
            return RedirectToAction("Details", controller, new { id = tipstaffRecordID });
        }

    }
}
