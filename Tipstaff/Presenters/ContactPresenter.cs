using System;
using System.Collections.Generic;
using Tipstaff.Mappers;
using Tipstaff.Presenters.Interfaces;
using Tipstaff.Services.Repositories;
using System.Linq;

namespace Tipstaff.Presenters
{
    public class ContactPresenter : IContactPresenter , IMapper<Models.Contact, Services.DynamoTables.Contact>
    {
        private readonly IContactsRepository _contactsRepository;

        public ContactPresenter(IContactsRepository contactsRepository)
        {
            _contactsRepository = contactsRepository;
        }


        public void AddContact(Models.Contact contact)
        {
            var entity = GetDynamoTable(contact);

            _contactsRepository.AddContact(entity);
        }

        public Models.Contact GetContact(string id)
        {
            var entity = _contactsRepository.GetContact(id);

            var model = GetModel(entity);

            return model;
        }

        public IEnumerable<Models.Contact> GetContacts()
        {
            var entities = _contactsRepository.GetContacts();

            var contacts = entities.Select(x => GetModel(x));

            return contacts;
        }

        public Services.DynamoTables.Contact GetDynamoTable(Models.Contact model)
        {
            var table = new Services.DynamoTables.Contact()
            {
                FirstName = model.firstName,
                Id = model.contactID,
                DX = model.DX,
                County = model.county,
                AddressLine1 = model.addressLine1,
                AddressLine2 = model.addressLine2,
                AddressLine3 = model.addressLine3,
                ContactType = MemoryCollections.ContactTypeList.GetContactTypeList().FirstOrDefault(x=>x.ContactTypeId == model.contactType.ContactTypeId).Detail,
                Email = model.email,
                LastName = model.lastName,
                Notes = model.notes,
                PhoneHome = model.phoneHome,
                PhoneMobile = model.phoneMobile,
                Postcode = model.postcode,
                SalutationId = MemoryCollections.SalutationList.GetSalutationByID(model.salutation.SalutationId).SalutationId,
                Town = model.town
            };

            return table;
        }

        public Models.Contact GetModel(Services.DynamoTables.Contact table)
        {
            var model = new Models.Contact()
            {
                addressLine1 = (string.IsNullOrEmpty(table.AddressLine1) ? "" : table.AddressLine1),
                addressLine2 = (string.IsNullOrEmpty(table.AddressLine2) ? "" : table.AddressLine2),
                addressLine3 = (string.IsNullOrEmpty(table.AddressLine3) ? "" : table.AddressLine3),
                DX = (string.IsNullOrEmpty(table.DX) ? "" : table.DX),
                email = (string.IsNullOrEmpty(table.Email) ? "" : table.Email),
                notes = (string.IsNullOrEmpty(table.Notes) ? "" : table.Notes),
                firstName = (string.IsNullOrEmpty(table.FirstName) ? "" : table.FirstName),
                lastName = (string.IsNullOrEmpty(table.LastName) ? "" : table.LastName),
                county = (string.IsNullOrEmpty(table.County) ? "" : table.County),
                town = (string.IsNullOrEmpty(table.Town) ? "" : table.Town),
                postcode = (string.IsNullOrEmpty(table.Postcode) ? "" : table.Postcode),
                phoneHome = (string.IsNullOrEmpty(table.PhoneHome) ? "" : table.PhoneHome),
                phoneMobile = (string.IsNullOrEmpty(table.PhoneMobile) ? "" : table.PhoneMobile),
                contactID = table.Id,
                contactType = MemoryCollections.ContactTypeList.GetContactTypeList().FirstOrDefault(x=>x.Detail == table.ContactType),
                salutation = MemoryCollections.SalutationList.GetSalutationByID(table.SalutationId)
            };

            return model;
        }

        public void UpdateContact(Models.Contact contact)
        {
            var entity = GetDynamoTable(contact);

            _contactsRepository.UpdateContact(entity);
        }
    }
}