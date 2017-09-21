using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface IContactsRepository
    {
        void AddContact(Contact contact);

        Contact GetContact(string id);

        IEnumerable<Contact> GetContacts();

        void UpdateContact(Contact contact);
    }
}
