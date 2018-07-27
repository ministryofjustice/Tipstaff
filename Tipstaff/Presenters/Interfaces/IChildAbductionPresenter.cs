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

        IEnumerable<ChildAbduction> GetAllActiveChildAbductions();

        ChildAbduction GetChildAbduction(string id);

        void RemoveChildAbduction(ChildAbduction childAbduction);
        
        void DeletedTipstaffRecords(DeletedTipstaffRecord record);

        void AddDeletedTipstaffRecord(DeletedTipstaffRecord record);

        void UpdateChildAbduction(ChildAbduction childAbduction);

        void AddTipstaffRecord(ChildAbduction childabduction);

        IEnumerable<ChildAbduction> GetAllChildAbductionsWithConditions();

        IEnumerable<ChildAbduction> GetAllClosedChildAbductions(DateTime start, DateTime end);

        //AttendanceNotes
        //Addresses
        //Respondents
        //Child
        //CaseReviews
        //Documents
    }
}
