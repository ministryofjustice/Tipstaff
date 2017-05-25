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

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class DocumentStatusController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;
        //
        // GET: /Admin/DocumentStatus/

        public ActionResult Index(DocumentStatusListView model)
        {
            IEnumerable<DocumentStatus> DocumentStatuses = db.DocumentStatuses;

            if (model.onlyActive == true)
            {
                DocumentStatuses = db.DocumentStatuses.Where(c => c.active == true);
            }
            model.DocumentStatuses = DocumentStatuses.OrderBy(c => c.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }

        //
        // GET: /Admin/DocumentStatus/Details/5

        public ActionResult Details(int id)
        {
            DocumentStatus Status = db.DocumentStatuses.Find(id);
            if (Status.active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot view {0} as it has been deactivated, please raise a help desk call to re-activate it.", Status.Detail);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(Status);
        }

        //
        // GET: /Admin/DocumentStatus/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Admin/DocumentStatus/Create

        [HttpPost]
        public ActionResult Create(DocumentStatus model)
        {
            if (ModelState.IsValid)
            {
                model.active = true;
                db.DocumentStatuses.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }
        
        //
        // GET: /Admin/DocumentStatus/Edit/5
 
        public ActionResult Edit(int id)
        {
            DocumentStatus model = db.DocumentStatuses.Find(id);
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
        // POST: /Admin/DocumentStatus/Edit/5

        [HttpPost]
        public ActionResult Edit(DocumentStatus model)
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
        // GET: /Admin/DocumentStatus/Delete/5

        public ActionResult Deactivate(int id)
        {
            DocumentStatus model = db.DocumentStatuses.Find(id);
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
        // POST: /Admin/DocumentStatus/Delete/5

        [HttpPost, ActionName("Deactivate")]
        public ActionResult DeactivateConfirmed(int id)
        {
            DocumentStatus model = db.DocumentStatuses.Find(id);
            model.active = false;
            model.deactivated = DateTime.Now;
            model.deactivatedBy = User.Identity.Name;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
