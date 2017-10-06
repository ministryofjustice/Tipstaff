using System;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Web.UI;
using Tipstaff.Logger;
using Tipstaff.Presenters;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class AddressController : Controller
    {
        /////private TipstaffDB db = myDBContextHelper.CurrentContext;
        private readonly ICloudWatchLogger _logger;
        private readonly IAddressPresenter _addressPresenter;
        
        // GET: /Address/

        public AddressController(ICloudWatchLogger telemetryLogger, IAddressPresenter addressPresenter)
        {
            _logger = telemetryLogger;
            _addressPresenter = addressPresenter;
        }
        
        public ActionResult Details(string id)
        {
            //////Address model = db.Addresses.Find(id);
            var model = _addressPresenter.GetAddress(id);
            return View(model);
        }

        public ActionResult Edit(string id)
        {
            ////Address model = db.Addresses.Find(id);
            var model = _addressPresenter.GetAddress(id);
            if (model == null)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("Address {0} has been deleted, please raise a help desk call if you think this has been deleted in error.", id);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            if (model.TipstaffRecord.caseStatus.Sequence > 3)
            {
                TempData["UID"] = model.TipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Address address)
        {
            if (ModelState.IsValid)
            {
                //////db.Entry(address).State = EntityState.Modified;
                //////db.SaveChanges();
                _addressPresenter.UpdateAddress(address);
                return RedirectToAction("Details", genericFunctions.TypeOfTipstaffRecord(address.tipstaffRecordID), new { id = address.tipstaffRecordID });
            }
            return View(address);
        }

        public ActionResult Create(string id)
        {
            ///AddressCreationModel model = new AddressCreationModel(id)

            AddressCreationModel model = new AddressCreationModel()
            {
                tipstaffRecord = _addressPresenter.GetTipstaffRecord(id)
            };

            if (model.tipstaffRecord.caseStatus.Sequence > 3)
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
                ////TipstaffRecord tr = db.TipstaffRecord.Find(model.tipstaffRecordID);
                ////string controller = genericFunctions.TypeOfTipstaffRecord(tr);
                TipstaffRecord tr = _addressPresenter.GetTipstaffRecord(model.tipstaffRecordID.ToString());

                //do stuff
                ////// VERONICA - INVESTIGATE THIS LOGIC!!! SOS
                //////tr.addresses.Add(model.address);

                //////db.SaveChanges();
                model.tipstaffRecord = tr;
                model.tipstaffRecordID = int.Parse(tr.tipstaffRecordID);
                _addressPresenter.AddAddress(model.address);


                if (Request.IsAjaxRequest())
                {
                    //////string url = string.Format("window.location='{0}';", Url.Action("Details", controller, new { id = model.tipstaffRecordID }));
                    string url = string.Format("window.location='{0}';", Url.Action("Details", tr.Descriminator, new { id = model.tipstaffRecordID }));

                    return JavaScript(url);
                }
                else
                {
                    ////////return RedirectToAction("Details", controller, new { id = model.tipstaffRecordID });
                    return RedirectToAction("Details", tr.Descriminator, new { id = model.tipstaffRecordID });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in AddressController in Create method, for user {((CPrincipal)User).UserID}");
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = genericFunctions.GetLowestError(ex);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", errModel ?? null);
            }
        }

        [OutputCache(Location = OutputCacheLocation.Server, Duration = 180)]
        public PartialViewResult ListAddressesByRecord(string id, int? page)
        {
            //////TipstaffRecord w = db.TipstaffRecord.Find(id);
            TipstaffRecord w = _addressPresenter.GetTipstaffRecord(id);

            ListAddressesByTipstaffRecord model = new ListAddressesByTipstaffRecord();
            model.tipstaffRecordID = w.tipstaffRecordID;
            model.TipstaffRecordClosed = w.caseStatusID > 2;
            model.Addresses = w.addresses.ToXPagedList<Address>(page ?? 1, 8);
            //model.Addresses = w.addresses.ToPagedList<Address>(page ?? 1, 8);
            return PartialView("_ListAddressesByRecord", model);
        }

        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Delete(string id)
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
            ////////model.Address = db.Addresses.Find(model.DeleteModelID);
            model.Address = _addressPresenter.GetAddress(model.DeleteModelID);

            int tipstaffRecordID = model.Address.tipstaffRecordID;
            string controller = genericFunctions.TypeOfTipstaffRecord(tipstaffRecordID);
            //////db.Addresses.Remove(model.Address);
            //////db.SaveChanges();
            _addressPresenter.RemoveAddress(model.Address);
            //get the Audit Event we just created 
            string recDeleted = model.DeleteModelID.ToString();
            ////////AuditEvent AE = db.AuditEvents.Where(a => a.auditEventDescription.AuditDescription == "Address deleted" && a.RecordChanged == recDeleted).OrderByDescending(a => a.EventDate).Take(1).Single();
            //add a deleted reason
            ////////AE.DeletedReasonID = model.DeletedReason.DeletedReasonID;
            //and save again
            ////////db.SaveChanges();
            return RedirectToAction("Details", controller, new { id = tipstaffRecordID });
        }

    }
}
