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
using Tipstaff.Areas.Admin.Models;

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class ChildRelationshipController : Controller
    {
        public ViewResult Index(ChildRelationshipListView model)
        {
            var memoryCollection = MemoryCollections.ChildRelationshipList.GetChildRelationshipList();
            IEnumerable<ChildRelationship> ChildRelationships = memoryCollection.Select(x =>
            new ChildRelationship()
            {
                childRelationshipID = x.ChildRelationshipID,
                Detail = x.Detail,
                active = ((x.Active == 1) ? true : false)
            });

            if (model.onlyActive == true)
            {
                ChildRelationships = ChildRelationships.Where(c => c.active == true);
            }
            model.ChildRelationships = ChildRelationships.OrderBy(c => c.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));


            return View(model);
        }
        
    }
}