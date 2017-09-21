using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Data.Entity;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Controllers
{
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class FAQController : Controller
    {
        //private TipstaffDB db;//= myDBContextHelper.CurrentContext;
        private readonly IFAQRepository _faqRepository;

        public FAQController(IFAQRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }
        

        [AllowAnonymous]
        public ActionResult Index()
        {
            System.Security.Principal.IIdentity userIdentity = User.Identity;
            Tipstaff.CPrincipal thisUser = new CPrincipal(userIdentity);
            if (thisUser.IsInRole("Admin"))
            {
                //var faqs = db.FAQs;
                var faqs = _faqRepository.GetAllFAQ();
                return View(faqs.ToList());
            }
            else
            {
                var faqs = _faqRepository.GetAllFAQ().Where(x=>x.LoggedInUser == User.Identity.IsAuthenticated);
                //var faqs = db.FAQs.Where(f => f.loggedInUser == User.Identity.IsAuthenticated);
                return View(faqs.ToList());
            }
        }
        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            //FAQ faq = db.FAQs.Find(id);
            var faq = _faqRepository.GetFAQ(id);

            return View(new FAQ()
            {
                faqID = faq.FaqId,
                answer = faq.Answer,
                question = faq.Question,
               loggedInUser = faq.LoggedInUser
            });
        }

        [AuthorizeRedirect(Roles = "Admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(FAQ faq)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(faq).State = EntityState.Modified;
                //db.SaveChanges();
                _faqRepository.Update(new Services.DynamoTables.FAQ()
                {
                    FaqId = faq.faqID,
                    Answer = faq.answer,
                    LoggedInUser = faq.loggedInUser,
                    Question = faq.question
                });

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
                //db.FAQs.Add(faq);
                //db.SaveChanges();
                _faqRepository.AddFaQ(new Services.DynamoTables.FAQ()
                {
                    FaqId = faq.faqID,
                    Answer = faq.answer,
                    LoggedInUser = faq.loggedInUser,
                    Question = faq.question
                });

                return RedirectToAction("Index");
            }

            return View(faq);
        }

    }
}
