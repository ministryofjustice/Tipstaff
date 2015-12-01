using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Tipstaff.Models;

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    public class DeletedTipstaffRecordController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;
        
        //
        // GET: /Admin/DeletedTipstaffRecord/

        public ActionResult Index(int? page)
        {
            if (page == null || page < 1)
            {
                page = 1;
            }
            var model = db.DeletedTipstaffRecords.Include(d=>d.deletedReason).OrderBy(d=>d.TipstaffRecordID).ToPagedList(page ?? 1, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }
    }
}
