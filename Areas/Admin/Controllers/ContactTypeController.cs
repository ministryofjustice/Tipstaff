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
    public class ContactTypeController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;

        //
        // GET: /Admin/ContactTypes/

        public ActionResult Index(ContactTypeListView model)
        {

            IEnumerable<ContactType> contactTypes = db.ContactTypes;

            if (model.onlyActive == true)
            {
                contactTypes = contactTypes.Where(c => c.active == true);
            }
            model.ContactTypes = contactTypes.OrderBy(c => c.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }

        //
        // GET: /Admin/ContactTypes/Details/5
        public ViewResult Details(int id)
        {
            ContactType model = db.ContactTypes.Find(id);
            return View(model);
        }

        //
        // GET: /Admin/ContactTypes/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/ContactTypes/Create

        [HttpPost]
        public ActionResult Create(ContactType contactType)
        {
            if (ModelState.IsValid)
            {
                contactType.active = true;
                db.ContactTypes.Add(contactType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contactType);
        }
        //
        // GET: /Admin/ContactTypes/Edit/5

        public ActionResult Edit(int id)
        {
            ContactType model = db.ContactTypes.Find(id);
            return View(model);
        }

        //
        // POST: /Admin/ContactTypes/Edit/5

        [HttpPost]
        public ActionResult Edit(ContactType model)
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
        // GET: /Admin/ContactTypes/Delete/5

        public ActionResult Deactivate(int id)
        {
            ContactType model = db.ContactTypes.Find(id);
            return View(model);
        }

        //
        // POST: /Admin/ContactTypes/Delete/5

        [HttpPost, ActionName("Deactivate")]
        public ActionResult DeactivateConfirmed(int id)
        {
            ContactType model = db.ContactTypes.Find(id);
            model.active = false;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
