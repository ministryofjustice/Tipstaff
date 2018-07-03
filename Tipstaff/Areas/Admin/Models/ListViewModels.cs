using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using Tipstaff.Models;

namespace Tipstaff.Models
{

    public class AdminListView
    {
        public bool onlyActive { get; set; }
        public int page { get; set; }
        public string detailContains { get; set; }
        public string sortOrder { get; set; }

        public AdminListView()
        {
            onlyActive = true;
            page = 1;
            detailContains = "";
        }
    }
    
    public class SolicitorListView : AdminListView
    {
        public IPagedList<Solicitor> Solicitors { get; set; }
    }
    public class SolicitorFirmListView : AdminListView
    {
        public IPagedList<SolicitorFirm> SolicitorFirms { get; set; }
    }
    
    public class PoliceForcesListView : AdminListView
    {
        public IPagedList<PoliceForces> PoliceForces { get; set; }
    }
}