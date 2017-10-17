using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenters.Interfaces
{
    public interface IContactPresenter
    {
        Contact GetContact(string id);

        void AddContact(Contact contact);

        IEnumerable<Contact> GetContacts();

        void UpdateContact(Contact contact);
    }
}
