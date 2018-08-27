using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tipstaff.Models;
using PagedList;
using System.Configuration;
using System.Data;
using System.Data.Entity;

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class ContactTypeController : Controller
    {
        public ActionResult Index(ContactTypeListView model) 
        {
            var collection  = MemoryCollections.ContactTypeList.GetContactTypeList();

          IEnumerable<ContactType> contactTypes = collection.Select(x =>
           new ContactType()
           {
               contactTypeID = x.ContactTypeId,
               Detail = x.Detail,
               active = ((x.Active == 1) ? true : false)
           });

            if (model.onlyActive == true)
            {
                contactTypes = contactTypes.Where(c => c.active == true);
            }
            model.ContactTypes = contactTypes.OrderBy(c => c.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }

        

    }
}
