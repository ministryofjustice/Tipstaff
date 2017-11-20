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

namespace Tipstaff.Areas.Admin.Controllers
{
    
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]

    public class PoliceForcesController : Controller
    {
        private readonly IPoliceForcesPresenter _policeForcesPresenter;
        private readonly ITipstaffRecordPresenter _tipstaffRecordPresenter;
        
        //private TipstaffDB db = myDBContextHelper.CurrentContext;
        //
        // GET: /Admin/PoliceForces/
        public PoliceForcesController(IPoliceForcesPresenter policeForcesPresenter, ITipstaffRecordPresenter tipstaffRecordPresenter)
        {
            _policeForcesPresenter = policeForcesPresenter;
            _tipstaffRecordPresenter = tipstaffRecordPresenter;
        }

        [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
        public ActionResult Index(PoliceForcesListView model)
        {
            if (model.page < 1)
            {
                model.page = 1;
            }

            ////IEnumerable<PoliceForces> PoliceForces = db.PoliceForces;
            IEnumerable<PoliceForces> PoliceForces = _policeForcesPresenter.GetAllPoliceForces();

            //if (model.onlyActive == true)
            //{
            //    PoliceForces = PoliceForces.Where(c => c.active == true);
            //}
            model.PoliceForces = PoliceForces.OrderBy(c => c.policeForceName).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }

        //
        // GET: /Admin/PoliceForces/Details/5
        [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
        public ActionResult Details(string id)
        {
            //////PoliceForces model = db.PoliceForces.Find(id);

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
            if (ModelState.IsValid)
            {
                model.active = true;
                //////db.PoliceForces.Add(model);
                //////db.SaveChanges();
                _policeForcesPresenter.AddPoliceForces(model);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /Admin/PoliceForce/Edit/5
        [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
        public ActionResult Edit(string id)
        {
            //////PoliceForces model = db.PoliceForces.Find(id);
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
            if (ModelState.IsValid)
            {
                ////db.Entry(model).State = EntityState.Modified;
                ////db.SaveChanges();
                _policeForcesPresenter.Update(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //
        // GET: /Admin/PoliceForces/Delete/5
        [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
        public ActionResult Deactivate(string id)
        {
            ////PoliceForces model = db.PoliceForces.Find(id);
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
            //////PoliceForces model = db.PoliceForces.Find(id);
            PoliceForces model = _policeForcesPresenter.GetPoliceForces(id);
            model.active = false;
            model.deactivated = DateTime.Now;
            model.deactivatedBy = User.Identity.Name;
            //////db.Entry(model).State = EntityState.Modified;
            //////db.SaveChanges();
            _policeForcesPresenter.Update(model);
            return RedirectToAction("Index");
        }

        //
        // GET: /PoliceForces/
        [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
        public ActionResult Add(int id)
        {
            PoliceForceCreation model = new PoliceForceCreation();
            //////model.TS_PoliceForce.tipstaffRecord = db.TipstaffRecord.Find(id);
            model.TS_PoliceForce.tipstaffRecord = _tipstaffRecordPresenter.GetTipStaffRecord(id.ToString());
            model.TS_PoliceForce.tipstaffRecordID = id;
            return View(model);
        }

        [HttpPost,AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
        public ActionResult Add(PoliceForceCreation model)
        {
            if (ModelState.IsValid)
            {
                //REVISIT THIS ONE!!!!
                //////TipstaffRecord tr = db.TipstaffRecord.Find(model.TS_PoliceForce.tipstaffRecordID);
                ////////////////TipstaffRecord tr = _tipstaffRecordPresenter.GetTipStaffRecord(model.TS_PoliceForce.tipstaffRecordID.ToString());
                ////////////////model.TS_PoliceForce.policeForceID = model.policeForceID;
                ////////////////tr.policeForces.Add(model.TS_PoliceForce);
                ////////////////db.SaveChanges();
                ////////////////return RedirectToAction("Details", genericFunctions.TypeOfTipstaffRecord(tr), new { id = model.TS_PoliceForce.tipstaffRecordID, Area="" });
            }
            return View(model);
        }
        [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
        public PartialViewResult ListPoliceForcesByRecord(int id, int? page)
        {
            //////TipstaffRecord w = db.TipstaffRecord.Find(id);
            TipstaffRecord w = _tipstaffRecordPresenter.GetTipStaffRecord(id.ToString());

            ListPoliceForcesByTipstaffRecord model = new ListPoliceForcesByTipstaffRecord();
            model.tipstaffRecordID = w.tipstaffRecordID;
            model.PoliceForces = w.policeForces.OrderByDescending(d => d.policeForces.policeForceName).ToXPagedList<TipstaffPoliceForce>(page ?? 1, 8);
            return PartialView("_ListPoliceForcesByRecord", model);
        }
    }
}
