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
    public class DocumentStatusController : Controller
    {
        public ActionResult Index(DocumentStatusListView model)
        {
            var memoryCollection = MemoryCollections.DocumentStatusList.GetDocumentStatusList();
            IEnumerable<DocumentStatus> DocumentStatuses = memoryCollection.Select(x =>
            new DocumentStatus()
            {
                DocumentStatusID = x.DocumentStatusID,
                Detail = x.Detail,
                active = ((x.Active == 1) ? true : false)
            });

            if (model.onlyActive == true)
            {
                DocumentStatuses = DocumentStatuses.Where(c => c.active == true);
            }
            model.DocumentStatuses = DocumentStatuses.OrderBy(c => c.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }
    }
}
