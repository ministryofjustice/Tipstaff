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

        public Contact GetContact(object key, object rangeKey=null)
        {
            return _dynamoAPI.GetEntity(key, rangeKey);
            
        }

        public Contact GetContact(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Contact> GetContacts(int id)
        {
            // var list = _dynamoAPI.GetResultsByCondition(id, ScanOperator.Contains, name)
            return null;
        }

        public IEnumerable<Contact> GetContacts(int id, string name)
        {
            throw new NotImplementedException();
        }

        public void UpdateContact(Contact contact)
        {
            var entity = _dynamoAPI.GetEntity(contact.ContactID, contact.contactTypeID);
            entity.addressLine1 = contact.addressLine1;
            _dynamoAPI.Save(entity);
        }
    }
}
