using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Tipstaff.Models;
using Tipstaff.Presenters.Interfaces;

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class DeletedTipstaffRecordController : Controller
    {
        ////private TipstaffDB db = myDBContextHelper.CurrentContext;
        private readonly IDeletedTipstaffRecordPresenter _deletedTipstaffRecordPresenter;

        public DeletedTipstaffRecordController(IDeletedTipstaffRecordPresenter deletedTipstaffRecordPresenter)
        {
            _deletedTipstaffRecordPresenter = deletedTipstaffRecordPresenter;
        }

        // GET: /Admin/DeletedTipstaffRecord/

        public ActionResult Index(int? page)
        {
            if (page == null || page < 1)
            {
                page = 1;
            }

            var records = _deletedTipstaffRecordPresenter.GetAll();

            var model = records.OrderBy(d=>d.TipstaffRecordID).ToPagedList(page ?? 1, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            //var model = records.Include(d => d.deletedReason).OrderBy(d => d.TipstaffRecordID).ToPagedList(page ?? 1, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }
    }
}
