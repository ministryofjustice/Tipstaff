using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tipstaff.Models;
using PagedList;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Data.Entity;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class SolicitorController : Controller
    {

        private TipstaffDB db = myDBContextHelper.CurrentContext;

        //
        // GET: /Solicitor/

        public ActionResult Select(int id, ChooseSolicitorModel model)
        {
            if ((db.Solicitors.Count() == 0) && (db.SolicitorsFirms.Count() == 0))
            {
                //No solicitors or firms... add a firm first
                return RedirectToAction("Create", "SolicitorFirm");
            }
            else if (db.Solicitors.Count() == 0)
            {
                //No solicitors but some firms, redirect to solicitor creation
                //and let the user choose a firm, or create new if needed
                return RedirectToAction("Create");
            }
            //ChooseSolicitorModel model = new ChooseSolicitorModel();
            if (model.tipstaffRecord == null) model.tipstaffRecord = db.TipstaffRecord.Find(id);
            if (model.tipstaffRecord.caseStatus.Sequence > 3)
            {
                TempData["UID"] = model.tipstaffRecord.UniqueRecordID;
                HttpResponse.RemoveOutputCacheItem("/Error/ClosedFile");
                return RedirectToAction("ClosedFile", "Error");
            }
            if (model.searchFirm == null) model.searchFirm = "";
            if (model.searchString == null) model.searchString = "";

            IQueryable<Solicitor> allSols = db.Solicitors;
            if (!String.IsNullOrEmpty(model.searchString))
            {
                allSols = allSols.Where(s => s.firstName.ToUpper().Contains(model.searchString.ToUpper()) || s.lastName.ToUpper().Contains(model.searchString.ToUpper()));
            }
            if (!String.IsNullOrEmpty(model.searchFirm))
            {
                allSols = allSols.Where(s => s.SolicitorFirm.firmName.ToUpper().Contains(model.searchFirm.ToUpper()));
            }
            //Note: working except clause
            IEnumerable<Solicitor> availableSols = allSols.Except(db.Solicitors.Where(s => db.TipstaffRecordSolicitors.Where(t => t.tipstaffRecordID == id).Select(t => t.solicitorID).Contains(s.solicitorID)));
            model.pSolicitors = availableSols.OrderBy(s => s.firstName).ToPagedList(model.page ?? 1, 8); //all
            return View(model);
        }
        //
        // GET: /ContactType/Details/5

        public ViewResult Details(int solicitorID, int tipstaffRecordID)
        {
            SolicitorbyTipstaffRecordViewModel model = new SolicitorbyTipstaffRecordViewModel(solicitorID, tipstaffRecordID);

            return View(model);
        }

        public ActionResult Edit(int solicitorID, int tipstaffRecordID)
        {
            EditSolicitorbyTipstaffRecordViewModel model = new EditSolicitorbyTipstaffRecordViewModel(solicitorID, tipstaffRecordID);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditSolicitorbyTipstaffRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model.Solicitor).State = EntityState.Modified;
                db.SaveChanges();
                string controller = genericFunctions.TypeOfTipstaffRecord(model.TipstaffRecord);
                return RedirectToAction("Details", "Solicitor", new { solicitorID = model.solicitorID, tipstaffRecordID = model.tipstaffRecordID });
            }
            return View(model);
        }

        /// <summary>
        /// Called as Partial view from Solicitor/Select
        /// </summary>
        /// <param name="warrantID"></param>
        /// <returns></returns>
        public PartialViewResult CreateSolicitor(int warrantID)
        {
            ViewBag.warrantID = warrantID;
            ViewBag.solicitorFirmID = new SelectList(db.SolicitorsFirms.OrderBy(s => s.firmName), "solicitorFirmID", "firmName");
            ViewBag.salutationID =  new SelectList(MemoryCollections.SalutationList.GetSalutationList().Where(x => x.Active == 1), "SalutationID", "Detail"); //new SelectList(db.Salutations.Where(x => x.active == true), "salutationID", "Detail");

            return PartialView("_createSolicitor");
            //return PartialView("_createSolicitorForWarrant");
        }

        [HttpPost]
        public ActionResult CreateSolicitor(Solicitor solicitor, int warrantID)
        {

            if (ModelState.IsValid)
            {
                db.Solicitors.Add(solicitor);
                db.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return RedirectToAction("Create", "TipstaffRecordSolicitor", new { tipstaffRecord = warrantID, solicitor = solicitor.solicitorID });
                    //return View("_createSolicitorForWarrantSuccess", model);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.salutationID = new SelectList(MemoryCollections.SalutationList.GetSalutationList().Where(x => x.Active == 1), "SalutationID", "Detail");//new SelectList(db.Salutations.Where(x => x.active == true), "salutationID", "Detail");
            ViewBag.solicitorFirmID = new SelectList(db.SolicitorsFirms, "solicitorFirmID", "firmName", solicitor.solicitorFirmID);
            //return PartialView("_createSolicitor");
            return PartialView("_createSolicitorForWarrant");
        }

        public PartialViewResult CreateFirm()
        {
            return PartialView("_createSolicitorFirm");
        }

        public ActionResult QuickSearch(string term)
        {
            var sols = db.Solicitors.Where(s => s.firstName.ToLower().Contains(term.ToLower()) || s.lastName.ToLower().Contains(term.ToLower())).ToList().Select(a => new { value = a.solicitorName });
            return Json(sols, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Location = OutputCacheLocation.Server, Duration = 180)]
        public PartialViewResult ListSolicitorsByRecord(int id, int? page)
        {
            TipstaffRecord w = db.TipstaffRecord.Find(id);
            ListTipstaffRecordSolicitorByTipstaffRecord model = new ListTipstaffRecordSolicitorByTipstaffRecord();
            model.tipstaffRecordID = w.tipstaffRecordID;
            model.LinkedSolicitors = w.LinkedSolicitors;
            model.TipstaffRecordClosed = w.caseStatusID > 2;
            model.pLinkedSolicitors = w.LinkedSolicitors.ToXPagedList<TipstaffRecordSolicitor>(page ?? 1, 8);
            //Note: Working Internal Paged list generation
            return PartialView("_ListSolicitorsByRecord", model);
        }
    }
}
