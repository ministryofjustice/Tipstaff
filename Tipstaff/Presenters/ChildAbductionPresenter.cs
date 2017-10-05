using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public class ChildAbductionPresenter : IChildAbductionPresenter
    {
        public void AddDeletedTipstaffRecord(DeletedTipstaffRecord record)
        {
            throw new NotImplementedException();
        }

        public void AddTipstaffRecord(TipstaffRecord record)
        {
            throw new NotImplementedException();
        }

        public void AddTipstaffRecord(ChildAbduction childabduction)
        {
            throw new NotImplementedException();
        }

        public void DeletedTipstaffRecords(DeletedTipstaffRecord record)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ChildAbduction> GetAllChildAbductions()
        {
            throw new NotImplementedException();
        }

        public ChildAbduction GetChildAbduction(string id)
        {
            throw new NotImplementedException();
        }

        public TipstaffRecord GetTipStaffRecord(string id)
        {
            throw new NotImplementedException();
        }

        public void RemoveChildAbduction(ChildAbduction childAbduction)
        {
            throw new NotImplementedException();
        }

        public void UpdateChildAbduction(ChildAbduction childAbduction)
        {
            throw new NotImplementedException();
        }

        public void UpdateTipstaffRecord(ChildAbduction childabduction)
        {
            throw new NotImplementedException();
        }

        public void UpdateTipstaffRecord(TipstaffRecord record)
        {
            throw new NotImplementedException();
        }
    }
}