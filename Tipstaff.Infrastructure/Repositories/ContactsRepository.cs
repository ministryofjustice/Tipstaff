using Amazon.DynamoDBv2.DocumentModel;
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
            var entity = _dynamoAPI.GetEntityByHashKey(contact.ContactID);
            entity.addressLine1 = contact.addressLine1;
            _dynamoAPI.Save(entity);
        }
    }
}
