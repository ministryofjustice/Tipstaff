using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Data.Entity;

namespace Tipstaff.Controllers
{
    public class FAQController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;

        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var faqs = db.FAQs;
                return View(faqs.ToList());
            }
            else
            {
                var faqs = db.FAQs.Where(f => f.loggedInUser == User.Identity.IsAuthenticated);
                return View(faqs.ToList());
            }
        }
        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            FAQ faq = db.FAQs.Find(id);
            return View(faq);
        }

        [AuthorizeRedirect(Roles = "Admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(FAQ faq)
        {
            if (ModelState.IsValid)
            {
                db.Entry(faq).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(faq);
        }
        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BusinessArea/Create

        [AuthorizeRedirect(Roles = "Admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(FAQ faq)
        {
            if (ModelState.IsValid)
            {
                db.FAQs.Add(faq);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(faq);
        }

    }
}
