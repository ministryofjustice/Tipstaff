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
using Tipstaff.Services.Repositories;

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class NationalityController : Controller
    {
        //private TipstaffDB db = myDBContextHelper.CurrentContext;
        private readonly INationalityRepository _nationalityRepository;

        public NationalityController(INationalityRepository nationalityRepository)
        {
            _nationalityRepository = nationalityRepository;
        }
        //
        // GET: /Admin/Nationality/

        public ActionResult Index(NationalityListView model)
        {
            if (model.page < 1)
            {
                model.page = 1;
            }

            //IEnumerable<Nationality> Nationalities = db.Nationalities;
            var Nationalities = _nationalityRepository.GetAllNationalities();

            if (model.onlyActive == true)
            {
                Nationalities = Nationalities.Where(c => c.Active == true);
            }
            if (model.detailContains != "" && model.detailContains != null)
            {
                Nationalities = Nationalities.Where(c => c.Detail.ToLower().Contains(model.detailContains.ToLower().ToString()));
            }
            model.Nationalities = Nationalities.OrderBy(c => c.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }
        // GET: /Admin/Gender/Details/5

        public ActionResult Details(string id)
        {
            //Nationality model = db.Nationalities.Find(id);
            var model = _nationalityRepository.GetNationality(id);
            if (model.Active == false)
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
                //model.active = true;
                //db.Nationalities.Add(model);
                //db.SaveChanges();
                _nationalityRepository.AddNationality(new Services.DynamoTables.Nationality()
                {
                    NationalityId = model.nationalityID,
                    Detail = model.Detail,
                    Active = true
                });
                return RedirectToAction("Index");
            }

            return View(model);
        }
        // GET: /Admin/Nationality/Edit/5
        public ActionResult Edit(string id)
        {
            //Nationality model = db.Nationalities.Find(id);
            var model = _nationalityRepository.GetNationality(id);
            if (model.Active == false)
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
                //db.Entry(model).State = EntityState.Modified;
                //db.SaveChanges();
                _nationalityRepository.Update(new Services.DynamoTables.Nationality()
                {
                    NationalityId = model.nationalityID,
                    Detail = model.Detail,
                    Active = model.active,
                    Deactivated = model.deactivated,
                    DeactivatedBy = model.deactivatedBy
                });
                return RedirectToAction("Index");
            }
            return View(model);
        }
        //
        // GET: /Admin/Nationality/Delete/5
        public ActionResult Deactivate(string id)
        {
            //Nationality model = db.Nationalities.Find(id);
            var model = _nationalityRepository.GetNationality(id);
            if (model.Active == false)
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
        public ActionResult DeactivateConfirmed(string id)
        {
            //Nationality model = db.Nationalities.Find(id);
            var model = _nationalityRepository.GetNationality(id);
            //model.Active = false;
            //model.Deactivated = DateTime.Now;
            //model.DeactivatedBy = User.Identity.Name;
            _nationalityRepository.Update(new Services.DynamoTables.Nationality()
            {
                NationalityId = model.NationalityId,
                Detail = model.Detail,
                Active = false,
                Deactivated = DateTime.Now,
                DeactivatedBy = User.Identity.Name
            });
            //db.Entry(model).State = EntityState.Modified;
            //db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
