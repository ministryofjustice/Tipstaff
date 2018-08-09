using System;
using System.Collections.Generic;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public interface IWarrantPresenter
    {
        Warrant GetWarrant(string id, LazyLoader loader = null);
        
        void RemoveWarrant(Warrant warrant);
        
        void AddWarrant(Warrant warrant);

        void UpdateWarrant(Warrant warrant);

        IEnumerable<Warrant> GetAllWarrants();

        IEnumerable<Warrant> GetAllActiveWarrants();

        IEnumerable<Warrant> GetAllClosedWarrants(DateTime start, DateTime end);
    }
}
