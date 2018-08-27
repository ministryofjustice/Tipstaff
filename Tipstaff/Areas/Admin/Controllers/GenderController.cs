using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Configuration;
using PagedList;
using System.Data;
using System.Data.Entity;

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class GenderController : Controller
    {
        public ActionResult Index(GenderListView model)
        {
            if (model.page < 1)
            {
                model.page = 1;
            }

            var memoryCollection = MemoryCollections.GenderList.GetGenderList();
            IEnumerable<Gender> Genders = memoryCollection.Select(x =>
            new Gender()
            {
                genderID = x.GenderId,
                Detail = x.Detail,
                active = ((x.Active == 1) ? true : false)
            });

            if (model.onlyActive == true)
            {
                Genders = Genders.Where(c => c.active == true);
            }
            model.Genders = Genders.OrderBy(c => c.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }
        
    }
}
