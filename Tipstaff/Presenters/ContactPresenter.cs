﻿using System;
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
                ContactType = MemoryCollections.ContactTypeList.GetContactTypeList().FirstOrDefault(x=>x.Detail == model.contactType.Detail).Detail,
                Email = model.email,
                LastName = model.lastName,
                Notes = model.notes,
                PhoneHome = model.phoneHome,
                PhoneMobile = model.phoneMobile,
                Postcode = model.postcode,
                Salutation = MemoryCollections.SalutationList.GetSalutationByDetail(model.salutation.Detail).Detail,
                Town = model.town
            };

            return table;
        }

        public Models.Contact GetModel(Services.DynamoTables.Contact table)
        {
            var model = new Models.Contact()
            {
                addressLine1 = table.AddressLine1,
                addressLine2 = table.AddressLine2,
                addressLine3 = table.AddressLine3,
                DX = table.DX,
                email = table.Email,
                notes = table.Notes,
                firstName = table.FirstName,
                lastName = table.LastName,
                county = table.County,
                town = table.Town,
                postcode = table.Postcode,
                phoneHome = table.PhoneHome,
                phoneMobile = table.PhoneMobile,
                contactID = table.Id,
                contactType = MemoryCollections.ContactTypeList.GetContactTypeList().FirstOrDefault(x=>x.Detail == table.ContactType),
                salutation = MemoryCollections.SalutationList.GetSalutationByDetail(table.Salutation)
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