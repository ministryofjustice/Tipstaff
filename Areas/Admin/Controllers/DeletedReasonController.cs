using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Tipstaff.Models;
using System.Data;

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    public class DeletedReasonController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;

        //
        // GET: /Admin/DeletionReasons/
        public ActionResult Index(DeletionReasonsListView model)
        {
            if (model.page < 1)
            {
                model.page = 1;
            }

            IEnumerable<DeletedReason> DeletedReasons = db.DeletedReasons;

            if (model.onlyActive == true)
            {
                DeletedReasons = DeletedReasons.Where(c => c.active == true);
            }
            model.DeletionReasons = DeletedReasons.OrderBy(c => c.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }
        // GET: /Admin/DeletionReasons/Details/5

        public ActionResult Details(int id)
        {
            DeletedReason model = db.DeletedReasons.Find(id);
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
        // GET: /Admin/Gender/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/DeletionReasons/Create
        [HttpPost]
        public ActionResult Create(DeletedReason model)
        {
            if (ModelState.IsValid)
            {
                model.active = true;
                db.DeletedReasons.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }
        // GET: /Admin/DeletionReasons/Edit/5
        public ActionResult Edit(int id)
        {
            DeletedReason model = db.DeletedReasons.Find(id);
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
        // POST: /Admin/DeletionReasons/Edit/5
        [HttpPost]
        public ActionResult Edit(DeletedReason model)
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
        // GET: /Admin/DeletionReasons/Delete/5
        public ActionResult Deactivate(int id)
        {
            DeletedReason model = db.DeletedReasons.Find(id);
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
        // POST: /Admin/DeletionReasons/Delete/5
        [HttpPost, ActionName("Deactivate")]
        public ActionResult DeactivateConfirmed(int id)
        {
            DeletedReason model = db.DeletedReasons.Find(id);
            model.active = false;
            model.deactivated = DateTime.Now;
            model.deactivatedBy = User.Identity.Name;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
