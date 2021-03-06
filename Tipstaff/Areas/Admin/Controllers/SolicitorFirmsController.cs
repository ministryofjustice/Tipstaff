﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Configuration;
using PagedList;
using System.Data.Entity;
using System.Data;

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class SolicitorFirmsController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;
        //
        // GET: /Admin/SolicitorFirms/
        public ViewResult Index(SolicitorFirmListView model)
        {
            if (model.page < 1)
            {
                model.page = 1;
            }

            IEnumerable<SolicitorFirm> Solicitorfirms = db.SolicitorsFirms;

            if (model.onlyActive == true)
            {
                Solicitorfirms = Solicitorfirms.Where(c => c.active == true);
            }
            if (model.detailContains != "" && model.detailContains != null)
            {
                Solicitorfirms = Solicitorfirms.Where(c => c.firmName.ToLower().Contains(model.detailContains.ToLower().ToString()));
            }

            switch (model.sortOrder)
            {
                case "solCount desc":
                    Solicitorfirms = Solicitorfirms.OrderByDescending(c => c.Solicitors.Count());
                    break;
                case "solCount asc":
                    Solicitorfirms = Solicitorfirms.OrderBy(c => c.Solicitors.Count());
                    break;
                case "firmAddress desc":
                    Solicitorfirms = Solicitorfirms.OrderByDescending(c => c.addressLine1);
                    break;
                case "firmAddress asc":
                    Solicitorfirms = Solicitorfirms.OrderBy(c => c.addressLine1);
                    break;
                case "firmName desc":
                    Solicitorfirms = Solicitorfirms.OrderByDescending(c => c.firmName);
                    break;
                case "firmName asc":
                default:
                    Solicitorfirms = Solicitorfirms.OrderBy(c => c.firmName);
                    break;
            }
            model.SolicitorFirms = Solicitorfirms.ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }
        // GET: /Admin/SolicitorFirms/Details/5
        public ActionResult Details(int id)
        {
            SolicitorFirm model = db.SolicitorsFirms.Find(id);
            if (model.active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot view {0} as it has been deactivated, please raise a help desk call to re-activate it.", model.firmName);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }
        //
        // GET: /Admin/SolicitorFirms/Create
        public ActionResult Create()
        {
            SolicitorFirm model = new SolicitorFirm();
            return View(model);
        }
        //
        // POST: /Admin/SolicitorFirms/Create
        [HttpPost]
        public ActionResult Create(SolicitorFirm model)
        {
            if (ModelState.IsValid)
            {
                model.active = true;
                db.SolicitorsFirms.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /Admin/SolicitorFirms/Edit/5
        public ActionResult Edit(int id)
        {
            SolicitorFirm model = db.SolicitorsFirms.Find(id);
            if (model.active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot view {0} as it has been deactivated, please raise a help desk call to re-activate it.", model.firmName);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }
        //
        // POST: /Admin/SolicitorFirms/Edit/5
        [HttpPost]
        public ActionResult Edit(SolicitorFirm model)
        {
            if (ModelState.IsValid)
            {

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        // GET: /Admin/SolicitorFirms/Delete/5
        public ActionResult Deactivate(int id)
        {
            SolicitorFirm model = db.SolicitorsFirms.Find(id);
            if (model.active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot view {0} as it has been deactivated, please raise a help desk call to re-activate it.", model.firmName);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            if (model.Solicitors.Count() > 0)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot deactivate {0} as they are still linked to cases, please re-allocate all cases before deactivating it.", model.firmName);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }
        //
        // POST: /Admin/SolicitorFirms/Delete/5
        [HttpPost, ActionName("Deactivate")]
        public ActionResult DeactivateConfirmed(int id)
        {
            SolicitorFirm model = db.SolicitorsFirms.Find(id);
            model.active = false;
            model.deactivated = DateTime.Now;
            model.deactivatedBy = User.Identity.Name;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
