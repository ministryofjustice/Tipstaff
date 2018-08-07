using System;
using System.Linq;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Configuration;
using PagedList;
using System.Data;
using Tipstaff.Presenters;
using TPLibrary.GuidGenerator;
using TPLibrary.Logger;

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class SolicitorsController : Controller
    {
        private readonly ISolicitorPresenter _solicitorPresenter;
        private readonly ISolicitorFirmsPresenter _solicitorFirmsPresenter;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICloudWatchLogger _logger;

        public SolicitorsController(ISolicitorPresenter solicitorPresenter, ISolicitorFirmsPresenter solicitorFirmsPresenter, IGuidGenerator guidGenerator, ICloudWatchLogger logger)
        {
            _solicitorPresenter = solicitorPresenter;
            _solicitorFirmsPresenter = solicitorFirmsPresenter;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        // GET: /Admin/Solicitor/
        public ViewResult Index(SolicitorListView model)
        {
            if (model.page < 1)
            {
                model.page = 1;
            }


            var Solicitors = _solicitorPresenter.GetSolicitors();
            if (model.onlyActive == true)
            {
                Solicitors = Solicitors.Where(c => c.active == true);
            }
            if (model.detailContains != "" && model.detailContains!=null)
            {
                Solicitors = Solicitors.Where(c=>c.solicitorName.ToLower().Contains(model.detailContains.ToLower().ToString()));
            }

            switch (model.sortOrder)
            {
                case "activeCount desc":
                    Solicitors = Solicitors.OrderByDescending(c => c.TipstaffRecords.Count());
                    break;
                case "activeCount asc": 
                    Solicitors = Solicitors.OrderBy(c => c.TipstaffRecords.Count());
                    break;
                case "solicitorFirm desc":
                    Solicitors = Solicitors.OrderByDescending(c => c.SolicitorFirm != null ? c.SolicitorFirm.firmName : string.Empty);
                    break;
                case "solicitorFirm asc":
                    Solicitors = Solicitors.OrderBy(c => c.SolicitorFirm!=null ? c.SolicitorFirm.firmName :string.Empty);
                    break;
                case "solicitorName desc":
                    Solicitors = Solicitors.OrderByDescending(c => c.lastName);
                    break;
                case "solicitorName asc":
                default:
                    Solicitors = Solicitors.OrderBy(c => c.lastName);
                    break;
            }
            model.Solicitors = Solicitors.ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }
        // GET: /Admin/Solicitor/Details/5
        public ActionResult Details(string id)
        {
            Solicitor model = _solicitorPresenter.GetSolicitor(id);

            if (model.active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot view {0} as it has been deactivated, please raise a help desk call to re-activate it.", model.solicitorName);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }

        // GET: /Admin/Solicitor/Create
        public ActionResult Create()
        {
            SolicitorAdmin model = new SolicitorAdmin();
            var solicitorFirms = _solicitorFirmsPresenter.GetAllSolicitorFirms();
            model.SolicitorFirmList = new SelectList(solicitorFirms.OrderBy(s => s.firmName), "solicitorFirmID", "firmName");

            return View(model);
        }

        // POST: /Admin/Solicitor/Create
        [HttpPost]
        public ActionResult Create(SolicitorAdmin model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.solicitor.active = true;
                    model.solicitor.solicitorID = _guidGenerator.GenerateTimeBasedGuid().ToString();
                    _solicitorPresenter.AddSolicitor(model.solicitor);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in Admin-Solicitors in Create method, for user {User.Identity.Name}");
                return View("Error");
            }
            
            return View(model);
        }

        // GET: /Admin/Solicitor/Edit/5
        public ActionResult Edit(string id)
        {
            SolicitorAdmin model = new SolicitorAdmin();
            model.solicitor  = _solicitorPresenter.GetSolicitor(id.ToString());
           
            var solicitorFirms = _solicitorFirmsPresenter.GetAllSolicitorFirms();
            model.SalutationList = new SelectList(MemoryCollections.SalutationList.GetSalutationList().Where(x => x.Active == 1), "SalutationId", "Detail", model.solicitor.salutation.SalutationId);
            model.SolicitorFirmList = new SelectList(solicitorFirms.OrderBy(s => s.firmName), "solicitorFirmID", "firmName", model.solicitor.solicitorFirmID);

            if (model.solicitor.active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot view {0} as it has been deactivated, please raise a help desk call to re-activate it.", model.solicitor.solicitorName);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }

        // POST: /Admin/Solicitor/Edit/5
        [HttpPost]
        public ActionResult Edit(SolicitorAdmin model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _solicitorPresenter.Update(model.solicitor);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in Admin-Solicitors in Edit method, for user {User.Identity.Name}");
                return View("Error");
            }

            return View(model);
        }
        // GET: /Admin/Solicitor/Delete/5
        public ActionResult Deactivate(int id)
        {
            Solicitor model = _solicitorPresenter.GetSolicitor(id.ToString());

            if (model.active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot view {0} as it has been deactivated, please raise a help desk call to re-activate it.", model.solicitorName);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            if (model.TipstaffRecords.Count() > 0)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot deactivate {0} as they are still linked to cases, please re-allocate all cases before deactivating it.", model.solicitorName);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }

        // POST: /Admin/Solicitor/Delete/5
        [HttpPost, ActionName("Deactivate")]
        public ActionResult DeactivateConfirmed(int id)
        {
            try
            {
                Solicitor model = _solicitorPresenter.GetSolicitor(id.ToString());
                model.active = false;
                model.deactivated = DateTime.Now;
                model.deactivatedBy = User.Identity.Name;

                _solicitorPresenter.Update(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in Admin-Solicitors in DeactivateConfirmed method, for user {User.Identity.Name}");
                return View("Error");
            }
        }
    }
}
