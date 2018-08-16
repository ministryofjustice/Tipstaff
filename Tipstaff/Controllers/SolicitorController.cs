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
using Tipstaff.Services.Repositories;
using Tipstaff.MemoryCollections;
using Tipstaff.Presenters;
using TPLibrary.GuidGenerator;
using TPLibrary.Logger;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class SolicitorController : Controller
    {
        private readonly ISolicitorPresenter _solicitorPresenter;
        private readonly ITipstaffRecordPresenter _tipstaffRecordPresenter;
        private readonly ISolicitorFirmsPresenter _solicitorFirmsPresenter;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICloudWatchLogger _logger;


        public SolicitorController(ISolicitorPresenter solicitorPresenter, 
            ITipstaffRecordPresenter tipstaffRecordPresenter, 
            ISolicitorFirmsPresenter solicitorFirmsPresenter, 
            IGuidGenerator guidGenerator, 
            ICloudWatchLogger logger)
        {
            _solicitorPresenter = solicitorPresenter;
            _tipstaffRecordPresenter = tipstaffRecordPresenter;
            _solicitorFirmsPresenter = solicitorFirmsPresenter;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        //
        // GET: /Solicitor/

        public ActionResult Select(string id, ChooseSolicitorModel model)
        {
            try
            {
                var solicitors = _solicitorPresenter.GetSolicitors();
                var record = _tipstaffRecordPresenter.GetTipStaffRecord(id);
                var solicitorFirms = _solicitorFirmsPresenter.GetAllSolicitorFirms();
                var result = solicitors.Any();
                var solicitorFirmsResult = solicitorFirms.Any();

                if ((!result) && (!solicitorFirmsResult))
                {
                    //No solicitors or firms... add a firm first
                    return RedirectToAction("Create", "SolicitorFirm");
                }
                else if (!result)
                {
                    //No solicitors but some firms, redirect to solicitor creation
                    //and let the user choose a firm, or create new if needed
                    return RedirectToAction("Create");
                }
                //ChooseSolicitorModel model = new ChooseSolicitorModel();
                //////if (model.tipstaffRecord == null) model.tipstaffRecord = db.TipstaffRecord.Find(id);
                if (model.tipstaffRecord == null) model.tipstaffRecord = record;
                if (model.tipstaffRecord.caseStatus.Sequence > 3)
                {
                    TempData["UID"] = model.tipstaffRecord.UniqueRecordID;
                    HttpResponse.RemoveOutputCacheItem("/Error/ClosedFile");
                    return RedirectToAction("ClosedFile", "Error");
                }
                if (model.searchFirm == null) model.searchFirm = "";
                if (model.searchString == null) model.searchString = "";

                ////IQueryable<Solicitor> allSols = db.Solicitors;
                IQueryable<Solicitor> allSols = _solicitorPresenter.GetSolicitors().AsQueryable();

                if (!String.IsNullOrEmpty(model.searchString))
                {
                    allSols = allSols.Where(s => s.firstName.ToUpper().Contains(model.searchString.ToUpper()) || s.lastName.ToUpper().Contains(model.searchString.ToUpper()));
                }
                if (!String.IsNullOrEmpty(model.searchFirm))
                {
                    allSols = allSols.Where(s => s.SolicitorFirm.firmName.ToUpper().Contains(model.searchFirm.ToUpper()));
                }
                //Note: working except clause

                //VERONICA - REVISIT TIPSTAFFRECORDSOLICITORS
                ///////IEnumerable < Solicitor > availableSols = allSols.Except(solicitors.Where(s => db.TipstaffRecordSolicitors.Where(t => t.tipstaffRecordID == id).Select(t => t.solicitorID).Contains(s.solicitorID)));
                IEnumerable<Solicitor> availableSols = allSols;
                model.pSolicitors = availableSols.OrderBy(s => s.firstName).ToPagedList(model.page ?? 1, 8); //all
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Solicitors in Select with Model method, for user {((CPrincipal)User).UserID}");
                ErrorModel mdl = new ErrorModel(2);
                mdl.ErrorMessage = ex.Message;
                TempData["ErrorModel"] = mdl;
                return RedirectToAction("IndexByModel", "Error", mdl ?? null);
            }
        }
        //
        // GET: /ContactType/Details/5
        
        public ViewResult Details(string solicitorID, string tipstaffRecordID)
        {
            SolicitorbyTipstaffRecordViewModel model = new SolicitorbyTipstaffRecordViewModel(solicitorID, tipstaffRecordID);
            model.Solicitor = _solicitorPresenter.GetSolicitor(solicitorID);
            model.TipstaffRecord = _tipstaffRecordPresenter.GetTipStaffRecord(tipstaffRecordID);
            return View(model);
        }

        public ActionResult Edit(string solicitorID, string tipstaffRecordID)
        {
            EditSolicitorbyTipstaffRecordViewModel model = new EditSolicitorbyTipstaffRecordViewModel(solicitorID, tipstaffRecordID);
            model.Solicitor = _solicitorPresenter.GetSolicitor(solicitorID);
            model.TipstaffRecord = _tipstaffRecordPresenter.GetTipStaffRecord(tipstaffRecordID);
            var solicitorFirms = _solicitorFirmsPresenter.GetAllSolicitorFirms();
            model.SolicitorsFirmList = new SelectList(solicitorFirms, "solicitorFirmID", "firmName", model.Solicitor.solicitorFirmID);
            model.SalutationList = new SelectList(MemoryCollections.SalutationList.GetSalutationList().Where(x => x.Active == 1), "SalutationID", "Detail", model.Solicitor.salutation.SalutationId);

            // SalutationList = new SelectList(myDBContextHelper.CurrentContext.Salutations.Where(x => x.active == true), "salutationID", "Detail", Solicitor.salutationID);

            /////SolicitorsFirmList = new SelectList(myDBContextHelper.CurrentContext.SolicitorsFirms.OrderBy(s => s.firmName), "solicitorFirmID", "firmName", Solicitor.solicitorFirmID);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditSolicitorbyTipstaffRecordViewModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _solicitorPresenter.Update(model.Solicitor);
                    var tipstaffrecord = _tipstaffRecordPresenter.GetTipStaffRecord(model.tipstaffRecordID);
                    string controller = tipstaffrecord.Discriminator;
                    return RedirectToAction("Details", "Solicitor", new { solicitorID = model.solicitorID, tipstaffRecordID = model.tipstaffRecordID });
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Solicitora in Edit with Model method, for user {((CPrincipal)User).UserID}");
                ErrorModel mdl = new ErrorModel(2);
                mdl.ErrorMessage = ex.Message;
                TempData["ErrorModel"] = mdl;
                return RedirectToAction("IndexByModel", "Error", mdl ?? null);
            }
        }
        

        [HttpPost]
        public ActionResult CreateSolicitor(Solicitor solicitor, string warrantID)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TipstaffRecord tr = _tipstaffRecordPresenter.GetTipStaffRecord(warrantID);

                    solicitor.solicitorID = _guidGenerator.GenerateTimeBasedGuid().ToString();

                    _solicitorPresenter.AddSolicitor(solicitor);

                    if (Request.IsAjaxRequest())
                    {
                        return RedirectToAction("Create", "TipstaffRecordSolicitor", new { tipstaffRecord = warrantID, solicitor = solicitor.solicitorID });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                var solicitorFirms = _solicitorFirmsPresenter.GetAllSolicitorFirms();

                ViewBag.salutationID = new SelectList(MemoryCollections.SalutationList.GetSalutationList().Where(x => x.Active == 1), "SalutationID", "Detail");//new SelectList(db.Salutations.Where(x => x.active == true), "salutationID", "Detail");
                ViewBag.solicitorFirmID = new SelectList(solicitorFirms, "solicitorFirmID", "firmName", solicitor.solicitorFirmID);
                return PartialView("_createSolicitorForWarrant");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Solicitors in CreateSolicitor with Model method, for user {((CPrincipal)User).UserID}");
                ErrorModel mdl = new ErrorModel(2);
                mdl.ErrorMessage = ex.Message;
                TempData["ErrorModel"] = mdl;
                return RedirectToAction("IndexByModel", "Error", mdl ?? null);
            }
        }

        public PartialViewResult CreateFirm()
        {
            return PartialView("_createSolicitorFirm");
        }

        public ActionResult QuickSearch(string term)
        {
            var solicitors = _solicitorPresenter.GetSolicitors();

            /////var sols = db.Solicitors.Where(s => s.firstName.ToLower().Contains(term.ToLower()) || s.lastName.ToLower().Contains(term.ToLower())).ToList().Select(a => new { value = a.solicitorName });

            var sols = solicitors.Where(s => s.firstName.ToLower().Contains(term.ToLower()) || s.lastName.ToLower().Contains(term.ToLower())).ToList().Select(a => new { value = a.solicitorName });

            return Json(sols, JsonRequestBehavior.AllowGet);
        }

       // [OutputCache(Location = OutputCacheLocation.Server, Duration = 180)]
        public PartialViewResult ListSolicitorsByRecord(string id, int? page)
        {
            ////TipstaffRecord w = db.TipstaffRecord.Find(id);
            TipstaffRecord w = _tipstaffRecordPresenter.GetTipStaffRecord(id,new LazyLoader() { LoadSolicitors = true });
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
