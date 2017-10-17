using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public interface ISolicitorFirmsPresenter 
    {
        void AddSolicitorFirm(SolicitorFirm solicitorFirm);

        SolicitorFirm GetSolicitorFirm(string id);

        IEnumerable<SolicitorFirm> GetAllSolicitorFirms();

        void Update(SolicitorFirm solicitorFirm);

        void Delete(SolicitorFirm solicitorFirm);
    }
}
