using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tipstaff.Areas.Admin.Models
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
}