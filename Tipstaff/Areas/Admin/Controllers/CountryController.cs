using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using PagedList;

using Tipstaff.Models;

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class CountryController : Controller
    {
        public ViewResult Index(CountryListView model)
        {
            if (model.page < 1)
            {
                model.page = 1;
            }

            var collection = MemoryCollections.CountryList.GetCountryList();

            IEnumerable<Country> Countries = collection.Select(x =>
             new Country()
             {
                 countryID = x.CountryID,
                 Detail = x.Detail,
                 active = ((x.Active == 1) ? true : false)
             });

            if (model.onlyActive == true)
            {
                Countries = Countries.Where(c => c.active == true);
            }
            if (model.detailContains != "" && model.detailContains != null)
            {
                Countries = Countries.Where(c => c.Detail.ToLower().Contains(model.detailContains.ToLower().ToString()));
            }
            model.Countries = Countries.OrderBy(c => c.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }
        
    }
}