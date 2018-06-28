using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public interface IAddressPresenter
    {
        Address GetAddress(string id);

        //TipstaffRecord GetTipstaffRecord(string id);

        void RemoveAddress(Address address);

        void UpdateAddress(Address address);

        void AddAddress(Address address);

        IEnumerable<Address> GetAddressesByTipstaffRecordId(string id);
    }
}
