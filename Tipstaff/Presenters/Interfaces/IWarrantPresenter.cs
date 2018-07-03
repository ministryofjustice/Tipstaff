using System.Collections.Generic;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public interface IWarrantPresenter
    {
        Warrant GetWarrant(string id);

        void RemoveWarrant(Warrant warrant);
        
        void AddWarrant(Warrant warrant);

        void UpdateWarrant(Warrant warrant);

        IEnumerable<Warrant> GetAllWarrants();

        IEnumerable<Warrant> GetAllWarrantsByCondition(string name, string value);

    }
}
