﻿using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Infrastructure.Repositories
{
    public class ContactsRepository : IContactsRepository
    {
        private readonly IDynamoAPI<Contact> _dynamoAPI;

        public ContactsRepository(IDynamoAPI<Contact> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public void AddContact(Contact contact)
        {
            _dynamoAPI.Save(contact);
        }
        
        public Contact GetContact(string id)
        {
            return _dynamoAPI.GetEntityByHashKey(id);
        }
        
        public IEnumerable<Contact> GetContacts()
        {
            return _dynamoAPI.GetAll();
        }

        public void UpdateContact(Contact contact)
        {
            var entity = _dynamoAPI.GetEntityByHashKey(contact.Id);

            entity.SalutationId = contact.SalutationId;
            entity.FirstName = contact.FirstName;
            entity.LastName = contact.LastName;
            entity.AddressLine1 = contact.AddressLine1;
            entity.AddressLine2 = contact.AddressLine2;
            entity.AddressLine3 = contact.AddressLine3;
            entity.Town = contact.Town;
            entity.County = contact.County;
            entity.Postcode = contact.Postcode;
            entity.DX = contact.DX;
            entity.PhoneHome = contact.PhoneHome;
            entity.PhoneMobile = contact.PhoneMobile;
            entity.Email = contact.Email;
            entity.Notes = contact.Notes;
            entity.ContactType = contact.ContactType;
            _dynamoAPI.Save(entity);
        }

        public void Delete(Contact contact)
        {
            _dynamoAPI.Delete(contact);
        }
    }
}
