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
                Contacts = Contacts.Where(w => w.FirstName.ToUpper().Contains(model.NameContains.ToUpper()) || w.LastName.ToUpper().Contains(model.NameContains.ToUpper()));
            }
            var col = Contacts.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ThenBy(c => c.Salutation);
            model.Contacts = col.Select(x => new Contact()
            {
                addressLine1 = x.AddressLine1,
                addressLine2 = x.AddressLine2,
                addressLine3 = x.AddressLine3,
                contactID = x.ContactID,
                phoneHome = x.PhoneHome,
                phoneMobile = x.PhoneMobile,
                email = x.Email,
                firstName = x.FirstName,
                lastName = x.LastName,
                postcode = x.Postcode,
                notes = x.Notes,
                DX = x.DX,
                county = x.County,
                town = x.Town,
                salutationID  = SalutationList.GetSalutationList().First(z=>z.Detail == x.Salutation).SalutationId,
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
                addressLine1 = contact.AddressLine1,
                addressLine2 = contact.AddressLine2,
                addressLine3 = contact.AddressLine3,
                contactID = contact.ContactID,
                phoneHome = contact.PhoneHome,
                phoneMobile = contact.PhoneMobile,
                email = contact.Email,
                firstName = contact.FirstName,
                lastName = contact.LastName,
                postcode = contact.Postcode,
                notes = contact.Notes,
                DX = contact.DX,
                county = contact.County,
                town = contact.Town,
                salutationID = SalutationList.GetSalutationList().First(z => z.Detail == contact.Salutation).SalutationId,
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
                    AddressLine1 = model.contact.addressLine1,
                    AddressLine2 = model.contact.addressLine2,
                    AddressLine3 = model.contact.addressLine3,
                    ContactType = ContactTypeList.GetContactTypeList().First(z => z.ContactTypeId == model.contact.contactTypeID).Detail,
                    PhoneHome = model.contact.phoneHome,
                    PhoneMobile = model.contact.phoneMobile,
                    Email = model.contact.email,
                    FirstName = model.contact.firstName,
                    LastName = model.contact.lastName,
                    Postcode = model.contact.postcode,
                    Notes = model.contact.notes,
                    DX = model.contact.DX,
                    County = model.contact.county,
                    Town = model.contact.town,
                    Salutation = SalutationList.GetSalutationList().First(z => z.SalutationId == model.contact.salutationID).Detail,
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
                    AddressLine1 = model.contact.addressLine1,
                    AddressLine2 = model.contact.addressLine2,
                    AddressLine3 = model.contact.addressLine3,
                    ContactType = ContactTypeList.GetContactTypeList().First(z => z.ContactTypeId == model.contact.contactTypeID).Detail,
                    PhoneHome = model.contact.phoneHome,
                    PhoneMobile = model.contact.phoneMobile,
                    Email = model.contact.email,
                    FirstName = model.contact.firstName,
                    LastName = model.contact.lastName,
                    Postcode = model.contact.postcode,
                    Notes = model.contact.notes,
                    DX = model.contact.DX,
                    County = model.contact.county,
                    Town = model.contact.town,
                    Salutation = SalutationList.GetSalutationList().First(z => z.SalutationId == model.contact.salutationID).Detail,
                };

                _contactsRepository.AddContact(contact);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult QuickSearch(string term)
        {
            var contacts = _contactsRepository.GetContacts();

            var aSols = contacts.Where(s => s.LastName.ToUpper().Contains(term.ToUpper()) || s.FirstName.ToUpper().Contains(term.ToUpper())).ToList();
            //////var aSols = db.Contacts.Where(s => s.lastName.ToUpper().Contains(term.ToUpper()) || s.firstName.ToUpper().Contains(term.ToUpper())).ToList();

            var sols = aSols.Select(x => new Contact()
            {
                firstName = x.FirstName,
                lastName = x.LastName,
                salutationID = SalutationList.GetSalutationList().First(z => z.Detail == x.Salutation).SalutationId
            });

            var fullNames = sols.Select(a => new { value = a.fullName });
            
            return Json(fullNames, JsonRequestBehavior.AllowGet);
        }
    }
}