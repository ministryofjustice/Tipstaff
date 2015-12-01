using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tipstaff.Models
{
    public class UserAdminVM
    {
        public User User { get; set; }
        public SelectList Roles { get; set; }
    }
}