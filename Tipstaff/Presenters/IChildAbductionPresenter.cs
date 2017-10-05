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

        ChildAbduction GetChildAbduction(string id);

        void RemoveChildAbduction(ChildAbduction childAbduction);

        TipstaffRecord GetTipStaffRecord(string id);

        void UpdateTipstaffRecord(TipstaffRecord record);

        void DeletedTipstaffRecords(DeletedTipstaffRecord record);

        void AddDeletedTipstaffRecord(DeletedTipstaffRecord record);

        void UpdateChildAbduction(ChildAbduction childAbduction);

        void AddTipstaffRecord(ChildAbduction childabduction);

        //AttendanceNotes
        //Addresses
        //Respondents
        //Child
        //CaseReviews
        //Documents
    }
}
