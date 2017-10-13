using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public interface IWarrantPresenter
    {
        TipstaffRecord GetTipstaffRecord(string id);

        void UpdateTipstaffRecord(TipstaffRecord tipstaffRecord);

        Warrant GetWarrant(string id);

        void RemoveWarrant(Warrant warrant);

        void AddDeletedTipstaffRecord(DeletedTipstaffRecord record);

        void AddWarrant(Warrant warrant);

        IEnumerable<Warrant> GetAllWarrants();
    }
}
