using System.Collections.Generic;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public interface ITipstaffPoliceForcesPresenter
    {
        void Add(TipstaffPoliceForce tpf);

        TipstaffPoliceForce GetTipstaffPoliceForce(string id);

        IEnumerable<TipstaffPoliceForce> GetAllTipstaffPoliceForcesByTipstaffRecordID(string id);

        void Update(TipstaffPoliceForce tpf);

        void Delete(TipstaffPoliceForce tpf);
    }
}