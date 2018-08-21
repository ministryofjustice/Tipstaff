using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public interface IChildAbductionPresenter
    {
        IEnumerable<ChildAbduction> GetAllChildAbductions();

        IEnumerable<ChildAbduction> GetActiveChildAbductions();

        ChildAbduction GetChildAbduction(string id);

        void RemoveChildAbduction(ChildAbduction childAbduction);
        
        void DeletedTipstaffRecords(DeletedTipstaffRecord record);

        void AddDeletedTipstaffRecord(DeletedTipstaffRecord record);

        void UpdateChildAbduction(ChildAbduction childAbduction);

        void AddTipstaffRecord(ChildAbduction childabduction);
        
        IEnumerable<ChildAbduction> GetAllClosedChildAbductions(DateTime start, DateTime end);
    }
}
