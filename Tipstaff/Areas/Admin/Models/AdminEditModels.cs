using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Models;
using System.Web.Mvc;

namespace Tipstaff.Models
{
    public class SolicitorAdmin
    {
        public Solicitor solicitor { get; set; }
        public SelectList SalutationList { get; set; }
        public SelectList SolicitorFirmList { get; set; }

        public SolicitorAdmin(int id)
        {
           ///// solicitor = myDBContextHelper.CurrentContext.Solicitors.Find(id);
            /////SolicitorFirmList = new SelectList(myDBContextHelper.CurrentContext.SolicitorsFirms.OrderBy(s=>s.firmName), "solicitorFirmID", "firmName", solicitor.solicitorFirmID);
            SalutationList = new SelectList(MemoryCollections.SalutationList.GetSalutationList().Where(x => x.Active == 1), "SalutationID", "Detail", solicitor.salutation.SalutationId);
        }
        public SolicitorAdmin()
        {
           ///// SolicitorFirmList = new SelectList(myDBContextHelper.CurrentContext.SolicitorsFirms.OrderBy(s => s.firmName), "solicitorFirmID", "firmName");
            SalutationList = new SelectList(MemoryCollections.SalutationList.GetSalutationList().Where(x => x.Active == 1), "SalutationID", "Detail");
        }
    }
}