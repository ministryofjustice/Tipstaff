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
    public class NationalityController : Controller
    {
        public ActionResult Index(NationalityListView model)
        {
            if (model.page < 1)
            {
                model.page = 1;
            }

            var collection = MemoryCollections.NationalityList.GetNationalityList();

            IEnumerable<Nationality> Nationalities = collection.Select(x =>
             new Nationality()
             {
                 nationalityID = x.NationalityID,
                 Detail = x.Detail,
                 active = ((x.Active == 1) ? true : false)
             });

            if (model.onlyActive == true)
            {
                Nationalities = Nationalities.Where(c => c.active == true);
            }
            if (model.detailContains != "" && model.detailContains != null)
            {
                Nationalities = Nationalities.Where(c => c.Detail.ToLower().Contains(model.detailContains.ToLower().ToString()));
            }
            model.Nationalities = Nationalities.OrderBy(c => c.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }
        
    }
}
