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
using Tipstaff.Services.Repositories;
using Tipstaff.MemoryCollections;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class ContactController : Controller
    {
        ////private TipstaffDB db = new TipstaffDB();
        private readonly IContactsRepository _contactsRepository;

        public ContactController(IContactsRepository contactsRepository)
        {
            _contactsRepository = contactsRepository;
        }
        //
        // GET: /Contacts/

        public ViewResult Index(ContactListView model)
        {
            int pageSize = Int32.Parse(ConfigurationManager.AppSettings["pageSize"]);

            ////IQueryable<Contact> Contacts = db.Contacts;
            var Contacts = _contactsRepository.GetContacts();
            var contactType = ContactTypeList.GetContactTypeList().Where(x=>x.ContactTypeId== model.ContactTypeID).SingleOrDefault();

            if (model.ContactTypeID > -1)
            {
                Contacts = Contacts.Where(w => w.ContactType == contactType.Detail);
            }
            if (!string.IsNullOrEmpty(model.NameContains))
            {
                //TRs = TRs.Where(w=>w.children.OrderByDescending(c => c.dateOfBirth).ThenBy(c => c.childID).FirstOrDefault().nameLast.ToUpper().Contains(model.childNameContains.ToUpper()));#
                Contacts = Contacts.Where(w => w.firstName.ToUpper().Contains(model.NameContains.ToUpper()) || w.lastName.ToUpper().Contains(model.NameContains.ToUpper()));
            }
            var col = Contacts.OrderBy(c => c.lastName).ThenBy(c => c.firstName).ThenBy(c => c.salutation);
            model.Contacts = col.Select(x => new Contact()
            {
                addressLine1 = x.addressLine1,
                addressLine2 = x.addressLine2,
                addressLine3 = x.addressLine3,
                contactID = x.ContactID,
                phoneHome = x.phoneHome,
                phoneMobile = x.phoneMobile,
                email = x.email,
                firstName = x.firstName,
                lastName = x.lastName,
                postcode = x.postcode,
                notes = x.notes,
                DX = x.DX,
                county = x.county,
                town = x.town,
                salutationID  = SalutationList.GetSalutationList().First(z=>z.Detail == x.salutation).SalutationId,
                contactTypeID = ContactTypeList.GetContactTypeList().First(z => z.Detail == x.ContactType).ContactTypeId,
                
            }).ToPagedList(model.page, pageSize);
            return View(model);
        }

        //
        // GET: /Contacts/Details/5

        public ViewResult Details(string id)
        {
            //////Contact contact = db.Contacts.Find(id);
            var contact = _contactsRepository.GetContact(id);

            var mdl = new Contact()
            {
                addressLine1 = contact.addressLine1,
                addressLine2 = contact.addressLine2,
                addressLine3 = contact.addressLine3,
                contactID = contact.ContactID,
                phoneHome = contact.phoneHome,
                phoneMobile = contact.phoneMobile,
                email = contact.email,
                firstName = contact.firstName,
                lastName = contact.lastName,
                postcode = contact.postcode,
                notes = contact.notes,
                DX = contact.DX,
                county = contact.county,
                town = contact.town,
                salutationID = SalutationList.GetSalutationList().First(z => z.Detail == contact.salutation).SalutationId,
                contactTypeID = ContactTypeList.GetContactTypeList().First(z => z.Detail == contact.ContactType).ContactTypeId
            };

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
                var contact = new Tipstaff.Services.DynamoTables.Contact()
                {
                    addressLine1 = model.contact.addressLine1,
                    addressLine2 = model.contact.addressLine2,
                    addressLine3 = model.contact.addressLine3,
                    ContactType = ContactTypeList.GetContactTypeList().First(z => z.ContactTypeId == model.contact.contactTypeID).Detail,
                    phoneHome = model.contact.phoneHome,
                    phoneMobile = model.contact.phoneMobile,
                    email = model.contact.email,
                    firstName = model.contact.firstName,
                    lastName = model.contact.lastName,
                    postcode = model.contact.postcode,
                    notes = model.contact.notes,
                    DX = model.contact.DX,
                    county = model.contact.county,
                    town = model.contact.town,
                    salutation = SalutationList.GetSalutationList().First(z => z.SalutationId == model.contact.salutationID).Detail,
                };

                _contactsRepository.AddContact(contact);

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
                ////////db.Entry(model.contact).State = EntityState.Modified;
                ////////db.SaveChanges();

                var contact = new Tipstaff.Services.DynamoTables.Contact()
                {
                    addressLine1 = model.contact.addressLine1,
                    addressLine2 = model.contact.addressLine2,
                    addressLine3 = model.contact.addressLine3,
                    ContactType = ContactTypeList.GetContactTypeList().First(z => z.ContactTypeId == model.contact.contactTypeID).Detail,
                    phoneHome = model.contact.phoneHome,
                    phoneMobile = model.contact.phoneMobile,
                    email = model.contact.email,
                    firstName = model.contact.firstName,
                    lastName = model.contact.lastName,
                    postcode = model.contact.postcode,
                    notes = model.contact.notes,
                    DX = model.contact.DX,
                    county = model.contact.county,
                    town = model.contact.town,
                    salutation = SalutationList.GetSalutationList().First(z => z.SalutationId == model.contact.salutationID).Detail,
                };

                _contactsRepository.AddContact(contact);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult QuickSearch(string term)
        {
            var contacts = _contactsRepository.GetContacts();

            var aSols = contacts.Where(s => s.lastName.ToUpper().Contains(term.ToUpper()) || s.firstName.ToUpper().Contains(term.ToUpper())).ToList();
            //////var aSols = db.Contacts.Where(s => s.lastName.ToUpper().Contains(term.ToUpper()) || s.firstName.ToUpper().Contains(term.ToUpper())).ToList();

            var sols = aSols.Select(x => new Contact()
            {
                firstName = x.firstName,
                lastName = x.lastName,
                salutationID = SalutationList.GetSalutationList().First(z => z.Detail == x.salutation).SalutationId
            });

            var fullNames = sols.Select(a => new { value = a.fullName });
            
            return Json(fullNames, JsonRequestBehavior.AllowGet);
        }
    }
}