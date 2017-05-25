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

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class ContactController : Controller
    {
        private TipstaffDB db = new TipstaffDB();

        //
        // GET: /Contacts/

        public ViewResult Index(ContactListView model)
        {
            int pageSize = Int32.Parse(ConfigurationManager.AppSettings["pageSize"]);

            IQueryable<Contact> Contacts = db.Contacts;
            if (model.ContactTypeID > -1)
            {
                Contacts = Contacts.Where(w => w.contactTypeID == model.ContactTypeID);
            }
            if (!string.IsNullOrEmpty(model.NameContains))
            {
                //TRs = TRs.Where(w=>w.children.OrderByDescending(c => c.dateOfBirth).ThenBy(c => c.childID).FirstOrDefault().nameLast.ToUpper().Contains(model.childNameContains.ToUpper()));#
                Contacts = Contacts.Where(w => w.firstName.ToUpper().Contains(model.NameContains.ToUpper()) || w.lastName.ToUpper().Contains(model.NameContains.ToUpper()));
            }
            model.Contacts = Contacts.OrderBy(c=>c.lastName).ThenBy(c=>c.firstName).ThenBy(c=>c.salutation.Detail).ToPagedList(model.page, pageSize);
            return View(model);
        }

        //
        // GET: /Contacts/Details/5

        public ViewResult Details(int id)
        {
            Contact contact = db.Contacts.Find(id);
            return View(contact);
        }

        //
        // GET: /Contacts/Create

        public ActionResult Create()
        {
            ContactModification model = new ContactModification();
            return View(model);
        } 

        //
        // POST: /Contacts/Create
        [HttpPost]
        public ActionResult Create(ContactModification model)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(model.contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //
        // GET: /Contacts/Edit/5
 
        public ActionResult Edit(int id)
        {
            ContactModification model = new ContactModification(id);
            return View(model);
        }

        //
        // POST: /Contacts/Edit/5

        [HttpPost]
        public ActionResult Edit(ContactModification model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model.contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult QuickSearch(string term)
        {

            var aSols = db.Contacts.Where(s => s.lastName.ToUpper().Contains(term.ToUpper()) || s.firstName.ToUpper().Contains(term.ToUpper())).ToList();

            var sols = aSols.Select(a => new { value = a.fullName});
            return Json(sols, JsonRequestBehavior.AllowGet);
        }
    }
}