using System.Linq;
using System.Data;
using System.Web.Mvc;
using Tipstaff.Models;
using Tipstaff.Presenters.Interfaces;

namespace Tipstaff.Controllers
{
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class FAQController : Controller
    {
        private readonly IFAQPresenter _faqPresenter;

        public FAQController(IFAQPresenter faqPresenter)
        {
            _faqPresenter = faqPresenter;
        }
        
        [AllowAnonymous]
        public ActionResult Index()
        {
            System.Security.Principal.IIdentity userIdentity = User.Identity;
            Tipstaff.CPrincipal thisUser = new CPrincipal(userIdentity);
            var faqs = _faqPresenter.GetAll();
            if (thisUser.IsInRole("Admin"))
            {
                //var faqs = db.FAQs;
                ////var faqs = _faqPresenter.GetAll();
                return View(faqs);
            }
            else
            {
                var filteredFaqs = faqs.Where(x=>x.loggedInUser == User.Identity.IsAuthenticated);
                //var faqs = db.FAQs.Where(f => f.loggedInUser == User.Identity.IsAuthenticated);
                return View(filteredFaqs);
            }
        }
        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            //FAQ faq = db.FAQs.Find(id);
            ////var faq = _faqRepository.GetFAQ(id);
            var faq =_faqPresenter.GetById(id);

            return View(faq);
        }

        [AuthorizeRedirect(Roles = "Admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(FAQ faq)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(faq).State = EntityState.Modified;
                //db.SaveChanges();
                //////_faqRepository.Update(new Services.DynamoTables.FAQ()
                //////{
                //////    Id = faq.faqID,
                //////    Answer = faq.answer,
                //////    LoggedInUser = faq.loggedInUser,
                //////    Question = faq.question
                //////});
                _faqPresenter.Update(faq);

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
                //////_faqRepository.AddFaQ(new Services.DynamoTables.FAQ()
                //////{
                //////    Id = faq.faqID,
                //////    Answer = faq.answer,
                //////    LoggedInUser = faq.loggedInUser,
                //////    Question = faq.question
                //////});
                _faqPresenter.Add(faq);

                return RedirectToAction("Index");
            }

            return View(faq);
        }

    }
}
