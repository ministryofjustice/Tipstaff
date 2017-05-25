using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using PagedList;

using Tipstaff.Models;

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class ChildRelationshipController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;

        //
        // GET: /ChildRelationship/

        public ViewResult Index(ChildRelationshipListView model)
        {

            IEnumerable<ChildRelationship> ChildRelationships = db.ChildRelationships;

            if (model.onlyActive == true)
            {
                ChildRelationships = db.ChildRelationships.Where(c => c.active == true);
            }
            model.ChildRelationships = ChildRelationships.OrderBy(c => c.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));


            return View(model);
        }

        //
        // GET: /ChildRelationship/Details/5

        public ViewResult Details(int id)
        {
            ChildRelationship childrelationship = db.ChildRelationships.Find(id);
            return View(childrelationship);
        }

        //
        // GET: /ChildRelationship/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /ChildRelationship/Create

        [HttpPost]
        public ActionResult Create(ChildRelationship childrelationship)
        {
            if (ModelState.IsValid)
            {
                db.ChildRelationships.Add(childrelationship);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(childrelationship);
        }
        
        //
        // GET: /ChildRelationship/Edit/5
 
        public ActionResult Edit(int id)
        {
            ChildRelationship childrelationship = db.ChildRelationships.Find(id);
            return View(childrelationship);
        }

        //
        // POST: /ChildRelationship/Edit/5

        [HttpPost]
        public ActionResult Edit(ChildRelationship childrelationship)
        {
            if (ModelState.IsValid)
            {
                db.Entry(childrelationship).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(childrelationship);
        }

        //
        // GET: /ChildRelationship/Delete/5
 
        public ActionResult Deactivate(int id)
        {
            ChildRelationship childrelationship = db.ChildRelationships.Find(id);
            return View(childrelationship);
        }

        //
        // POST: /ChildRelationship/Delete/5

        [HttpPost, ActionName("Deactivate")]
        public ActionResult DeactivateConfirmed(int id)
        {            
            ChildRelationship childrelationship = db.ChildRelationships.Find(id);
            childrelationship.active = false;
            childrelationship.deactivated = DateTime.Now;
            childrelationship.deactivatedBy = User.Identity.Name;
            db.Entry(childrelationship).State = EntityState.Modified;
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