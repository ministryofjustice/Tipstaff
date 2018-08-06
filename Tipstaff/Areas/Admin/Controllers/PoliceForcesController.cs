using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tipstaff.Models;
using PagedList;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using Tipstaff.Presenters;
using TPLibrary.GuidGenerator;
using TPLibrary.Logger;

namespace Tipstaff.Areas.Admin.Controllers
{
    
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]

    public class PoliceForcesController : Controller
    {
        private readonly IPoliceForcesPresenter _policeForcesPresenter;
        private readonly ITipstaffRecordPresenter _tipstaffRecordPresenter;
        private readonly ITipstaffPoliceForcesPresenter _tpfPresenter;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICloudWatchLogger _logger;

        // GET: /Admin/PoliceForces/
        public PoliceForcesController(ITipstaffPoliceForcesPresenter tpfPresenter, IPoliceForcesPresenter policeForcesPresenter, ITipstaffRecordPresenter tipstaffRecordPresenter, IGuidGenerator guidGenerator, ICloudWatchLogger logger)
        {
            _tpfPresenter = tpfPresenter;
            _policeForcesPresenter = policeForcesPresenter;
            _tipstaffRecordPresenter = tipstaffRecordPresenter;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
        public ActionResult Index(PoliceForcesListView model)
        {
            try
            {
                if (model.page < 1)
                {
                    model.page = 1;
                }

                IEnumerable<PoliceForces> PoliceForces = _policeForcesPresenter.GetAllPoliceForces();
                model.PoliceForces = PoliceForces.OrderBy(c => c.policeForceName).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in Admin-PoliceForcesController in Index method, for user {User.Identity.Name}");
                return View("Error");
            }
            
            return View(model);
        }

        //
        // GET: /Admin/PoliceForces/Details/5
        [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
        public ActionResult Details(string id)
        {
            PoliceForces model = new PoliceForces();
            try
            {
                model = _policeForcesPresenter.GetPoliceForces(id);
                if (model.active == false)
                {
                    ErrorModel errModel = new ErrorModel(2);
                    errModel.ErrorMessage = string.Format("You cannot view {0} as it has been deactivated, please raise a help desk call to re-activate it.", model.policeForceName);
                    TempData["ErrorModel"] = errModel;
                    return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in Admin-PoliceForcesController in Details method, for user {User.Identity.Name}");
                return View("Error");
            }
            
            return View(model);
        }

        //
        // GET: /Admin/PoliceForces/Create
        [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Admin/PoliceForces/Create

        [HttpPost,AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
        public ActionResult Create(PoliceForces model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.active = true;
                    model.policeForceID = _guidGenerator.GenerateTimeBasedGuid().ToString();
                    _policeForcesPresenter.AddPoliceForces(model);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in Admin-PoliceForcesController in Create method, for user {User.Identity.Name}");
                return View("Error");
            }

            return View(model);
        }

        // GET: /Admin/PoliceForce/Edit/5
        [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
        public ActionResult Edit(string id)
        {
            PoliceForces model = _policeForcesPresenter.GetPoliceForces(id);
            if (model.active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot view {0} as it has been deactivated, please raise a help desk call to re-activate it.", model.policeForceName);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }

        //
        // POST: /Admin/PoliceForce/Edit/5

        [HttpPost,AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
        public ActionResult Edit(PoliceForces model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _policeForcesPresenter.Update(model);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in Admin-PoliceForcesController in Edit method, for user {User.Identity.Name}");
                return View("Error");   
            }            
            return View(model);
        }

        //
        // GET: /Admin/PoliceForces/Delete/5
        [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
        public ActionResult Deactivate(string id)
        {
            PoliceForces model = _policeForcesPresenter.GetPoliceForces(id);
            if (model.active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot view {0} as it has been deactivated, please raise a help desk call to re-activate it.", model.policeForceName);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }

        //
        // POST: /Admin/PoliceForces/Delete/5

        [HttpPost, ActionName("Deactivate"),AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
        public ActionResult DeactivateConfirmed(string id)
        {
            PoliceForces model = new PoliceForces();
            try
            {
                model = _policeForcesPresenter.GetPoliceForces(id);
                model.active = false;
                model.deactivated = DateTime.Now;
                model.deactivatedBy = User.Identity.Name;
                _policeForcesPresenter.Update(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in Admin-PoliceForcesController in DeactivateConfirmed method, for user {User.Identity.Name}");
                return View("Error");
            }
        }

        //
        // GET: /PoliceForces/
        [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
        public ActionResult Add(string id)
        {
            PoliceForceCreation model = new PoliceForceCreation();
            model.PoliceForceList = new SelectList(_policeForcesPresenter.GetAllPoliceForces().Where(x => x.active == true).OrderBy(x => x.policeForceName).ToList(), "policeForceID", "policeForceName");
            model.TS_PoliceForce.tipstaffRecordID = id;
            return View(model);
        }

        [HttpPost,AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
        public ActionResult Add(PoliceForceCreation model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TipstaffRecord tr = _tipstaffRecordPresenter.GetTipStaffRecord(model.TS_PoliceForce.tipstaffRecordID.ToString());
                    model.TS_PoliceForce.policeForceID = model.policeForceID;
                    model.TS_PoliceForce.tipstaffRecordPoliceForceID = _guidGenerator.GenerateTimeBasedGuid().ToString();

                    _tpfPresenter.Add(model.TS_PoliceForce);
                    return RedirectToAction("Details", genericFunctions.TypeOfTipstaffRecord(tr), new { id = model.TS_PoliceForce.tipstaffRecordID, Area = "" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in Admin-PoliceForcesController in Add method, for user {User.Identity.Name}");
                return View("Error");
            }
            
            return View(model);
        }
        [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
        public PartialViewResult ListPoliceForcesByRecord(string id, int? page)
        {
            ListPoliceForcesByTipstaffRecord model = new ListPoliceForcesByTipstaffRecord();
            model.tipstaffRecordID = id;
            var tpfs = _tpfPresenter.GetAllTipstaffPoliceForcesByTipstaffRecordID(id);
            
            model.PoliceForces = tpfs.OrderByDescending(d => d.policeForce.policeForceName).ToXPagedList<TipstaffPoliceForce>(page ?? 1, 8);
                
            return PartialView("_ListPoliceForcesByRecord", model);
        }
    }
}
