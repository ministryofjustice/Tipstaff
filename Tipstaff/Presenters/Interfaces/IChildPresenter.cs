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
        Child GetChild(string id, string childId);

        IList<Child> GetAllChildrenByTipstaffRecordID(string id);

         void AddChild(ChildCreationModel model);

        void UpdateChild(ChildCreationModel model);

        void DeleteChild(DeleteChild model);

        IEnumerable<Child> GetAllChildren();
    }
}
