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
    public class DocumentTypeController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;
        //
        // GET: /Admin/DocumentTypes/

        public ActionResult Index(DocumentTypeListView model)
        {
            if (model.page < 1)
            {
                model.page = 1;
            }

            IEnumerable<DocumentType> DocumentTypes = db.DocumentTypes;

            if (model.onlyActive == true)
            {
                DocumentTypes = DocumentTypes.Where(c => c.active == true);
            }
            model.DocumentTypes = DocumentTypes.OrderBy(c => c.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }
        // GET: /Admin/DocumentType/Details/5

        public ActionResult Details(int id)
        {
            DocumentType model = db.DocumentTypes.Find(id);
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
        // GET: /Admin/DocumentType/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/DocumentType/Create

        [HttpPost]
        public ActionResult Create(DocumentType model)
        {
            if (ModelState.IsValid)
            {
                model.active = true;
                db.DocumentTypes.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /Admin/DocumentType/Edit/5
        public ActionResult Edit(int id)
        {
            DocumentType model = db.DocumentTypes.Find(id);
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
        // POST: /Admin/DocumentType/Edit/5

        [HttpPost]
        public ActionResult Edit(DocumentType model)
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
            DocumentType model = db.DocumentTypes.Find(id);
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
            DocumentType model = db.DocumentTypes.Find(id);
            model.active = false;
            model.deactivated = DateTime.Now;
            model.deactivatedBy = User.Identity.Name;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
