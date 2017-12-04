using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using PagedList;
using Tipstaff.Models;
using Tipstaff.MemoryCollections;
using Tipstaff.Presenters.Interfaces;
using Tipstaff.Infrastructure.Services;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class ContactController : Controller
    {
        ////private TipstaffDB db = new TipstaffDB();
        /// private readonly IContactsRepository _contactsRepository;
        private readonly IContactPresenter _contactPresenter;
        private readonly IGuidGenerator _guidGenerator;

        public ContactController(IContactPresenter contactPresenter, IGuidGenerator guidGenerator)
        {
            _contactPresenter = contactPresenter;
            _guidGenerator = guidGenerator;
        }
        //
        // GET: /Contacts/

        public ViewResult Index(ContactListView model)
        {
            int pageSize = Int32.Parse(ConfigurationManager.AppSettings["pageSize"]);

            ////IQueryable<Contact> Contacts = db.Contacts;
            var Contacts = _contactPresenter.GetContacts();
            var contactType = ContactTypeList.GetContactTypeList().Where(x=>x.ContactTypeId== model.ContactTypeID).SingleOrDefault();

            if (model.ContactTypeID > -1)
            {
                Contacts = Contacts.Where(w => w.contactType.Detail == contactType.Detail);
            }
            if (!string.IsNullOrEmpty(model.NameContains))
            {
                //TRs = TRs.Where(w=>w.children.OrderByDescending(c => c.dateOfBirth).ThenBy(c => c.childID).FirstOrDefault().nameLast.ToUpper().Contains(model.childNameContains.ToUpper()));#
                Contacts = Contacts.Where(w => w.firstName.ToUpper().Contains(model.NameContains.ToUpper()) || w.lastName.ToUpper().Contains(model.NameContains.ToUpper()));
            }
            var col = Contacts.OrderBy(c => c.lastName).ThenBy(c => c.firstName).ThenBy(c => c.salutation);
            model.Contacts = col.ToPagedList(model.page, pageSize);
            return View(model);
        }

        //
        // GET: /Contacts/Details/5

        public ViewResult Details(string id)
        {
            //////Contact contact = db.Contacts.Find(id);


            var mdl = _contactPresenter.GetContact(id);

            return View(mdl);
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
                //////db.Contacts.Add(model.contact);
                //////db.SaveChanges();
                model.contact.contactID = _guidGenerator.GenerateTimeBasedGuid().ToString();
                _contactPresenter.AddContact(model.contact);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        //
        // GET: /Contacts/Edit/5
 
        public ActionResult Edit(string id)
        {
            ContactModification model = new ContactModification()
            {
                contact = _contactPresenter.GetContact(id)
            };

            return View(model);
        }

        //
        // POST: /Contacts/Edit/5

        [HttpPost]
        public ActionResult Edit(ContactModification model)
        {
            if (ModelState.IsValid)
            {
                ////////db.Entry(model.contact).State = EntityState.Modified;
                ////////db.SaveChanges();

                _contactPresenter.UpdateContact(model.contact);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult QuickSearch(string term)
        {
            var contacts = _contactPresenter.GetContacts();

            var aSols = contacts.Where(s => s.lastName.ToUpper().Contains(term.ToUpper()) || s.firstName.ToUpper().Contains(term.ToUpper())).ToList();
            //////var aSols = db.Contacts.Where(s => s.lastName.ToUpper().Contains(term.ToUpper()) || s.firstName.ToUpper().Contains(term.ToUpper())).ToList();

            ////var sols = aSols.Select(x => new Contact()
            ////{
            ////    firstName = x.FirstName,
            ////    lastName = x.LastName,
            ////    salutationID = SalutationList.GetSalutationList().First(z => z.Detail == x.Salutation).SalutationId
            ////});

            var fullNames = aSols.Select(a => new { value = a.fullName });
            
            return Json(fullNames, JsonRequestBehavior.AllowGet);
        }
    }
}