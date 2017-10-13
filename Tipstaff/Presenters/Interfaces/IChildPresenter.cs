using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public interface IChildPresenter
    {
        Child GetChild(string id);

        TipstaffRecord GetTipstaffRecord(string id);

        IEnumerable<Child> GetAllChildrenByTipstaffRecordID(string id);

        ChildAbduction GetChildAbduction(string id);

        void AddChild(ChildCreationModel model);

        void UpdateChild(ChildCreationModel model);

        void UpdateChildAbduction(ChildAbduction model);

        void DeleteChild(DeleteChild model);
    }
}
