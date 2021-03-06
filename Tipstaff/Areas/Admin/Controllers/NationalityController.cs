﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Configuration;
using PagedList;
using System.Data;
using System.Data.Entity;

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class NationalityController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;
        //
        // GET: /Admin/Nationality/

        public ActionResult Index(NationalityListView model)
        {
            if (model.page < 1)
            {
                model.page = 1;
            }

            IEnumerable<Nationality> Nationalities = db.Nationalities;

            if (model.onlyActive == true)
            {
                Nationalities = Nationalities.Where(c => c.active == true);
            }
            if (model.detailContains != "" && model.detailContains != null)
            {
                Nationalities = Nationalities.Where(c => c.Detail.ToLower().Contains(model.detailContains.ToLower().ToString()));
            }
            model.Nationalities = Nationalities.OrderBy(c => c.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }
        // GET: /Admin/Gender/Details/5

        public ActionResult Details(int id)
        {
            Nationality model = db.Nationalities.Find(id);
            if (model.active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot view {0} as it has been deactivated, please raise a help desk call to re-activate it.", model.Detail);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }
        //
        // GET: /Admin/Nationality/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/Nationality/Create

        [HttpPost]
        public ActionResult Create(Nationality model)
        {
            if (ModelState.IsValid)
            {
                model.active = true;
                db.Nationalities.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }
        // GET: /Admin/Nationality/Edit/5
        public ActionResult Edit(int id)
        {
            Nationality model = db.Nationalities.Find(id);
            if (model.active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot view {0} as it has been deactivated, please raise a help desk call to re-activate it.", model.Detail);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }

        //
        // POST: /Admin/Nationality/Edit/5

        [HttpPost]
        public ActionResult Edit(Nationality model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        //
        // GET: /Admin/Nationality/Delete/5
        public ActionResult Deactivate(int id)
        {
            Nationality model = db.Nationalities.Find(id);
            if (model.active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot view {0} as it has been deactivated, please raise a help desk call to re-activate it.", model.Detail);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }

        //
        // POST: /Admin/Nationality/Delete/5
        [HttpPost, ActionName("Deactivate")]
        public ActionResult DeactivateConfirmed(int id)
        {
            Nationality model = db.Nationalities.Find(id);
            model.active = false;
            model.deactivated = DateTime.Now;
            model.deactivatedBy = User.Identity.Name;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
