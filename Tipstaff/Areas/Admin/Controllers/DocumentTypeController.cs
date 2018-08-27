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
    public class DocumentTypeController : Controller
    {
         public ActionResult Index(DocumentTypeListView model)
        {
            if (model.page < 1)
            {
                model.page = 1;
            }

            var memoryCollection = MemoryCollections.DocumentTypeList.GetDocumentTypeList();
            IEnumerable<DocumentType> DocumentTypes = memoryCollection.Select(x =>
            new DocumentType()
            {
                documentTypeID = x.DocumentTypeID,
                Detail = x.Detail,
                active = ((x.Active == 1) ? true : false)
            });

            if (model.onlyActive == true)
            {
                DocumentTypes = DocumentTypes.Where(c => c.active == true);
            }
            model.DocumentTypes = DocumentTypes.OrderBy(c => c.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }
    }
}
