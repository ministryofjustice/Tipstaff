using System;
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
    public class SolicitorsController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;
        //
        // GET: /Admin/Solicitor/
        public ViewResult Index(SolicitorListView model)
        {
            if (model.page < 1)
            {
                model.page = 1;
            }

            IEnumerable<Solicitor> Solicitors = db.Solicitors.Include(s=>s.salutation).Include(f => f.SolicitorFirm);//needed?

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
        public ActionResult Details(int id)
        {
            Solicitor model = db.Solicitors.Find(id);
            if (model.active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot view {0} as it has been deactivated, please raise a help desk call to re-activate it.", model.solicitorName);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }
        //
        // GET: /Admin/Solicitor/Create
        public ActionResult Create()
        {
            SolicitorAdmin model = new SolicitorAdmin();
            return View(model);
        }
        //
        // POST: /Admin/Solicitor/Create
        [HttpPost]
        public ActionResult Create(SolicitorAdmin model)
        {
            if (ModelState.IsValid)
            {
                model.solicitor.active = true;
                db.Solicitors.Add(model.solicitor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /Admin/Solicitor/Edit/5
        public ActionResult Edit(int id)
        {
            SolicitorAdmin model = new SolicitorAdmin(id);
            if (model.solicitor.active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot view {0} as it has been deactivated, please raise a help desk call to re-activate it.", model.solicitor.solicitorName);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }
        //
        // POST: /Admin/Solicitor/Edit/5
        [HttpPost]
        public ActionResult Edit(SolicitorAdmin model)
        {
            if (ModelState.IsValid)
            {

                db.Entry(model.solicitor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        // GET: /Admin/Solicitor/Delete/5
        public ActionResult Deactivate(int id)
        {
            Solicitor model = db.Solicitors.Find(id);
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
        //
        // POST: /Admin/Solicitor/Delete/5
        [HttpPost, ActionName("Deactivate")]
        public ActionResult DeactivateConfirmed(int id)
        {
            Solicitor model = db.Solicitors.Find(id);
            model.active = false;
            model.deactivated = DateTime.Now;
            model.deactivatedBy = User.Identity.Name;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
