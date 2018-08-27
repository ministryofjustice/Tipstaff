//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Web.Mvc;
//using PagedList;
//using Tipstaff.Models;
//using System.Data;
//using System.Data.Entity;

//namespace Tipstaff.Areas.Admin.Controllers
//{
//    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
//    [Authorize]
//    [ValidateAntiForgeryTokenOnAllPosts]
//    public class DeletedReasonController : Controller
//    {
//        private TipstaffDB db = myDBContextHelper.CurrentContext;

//        //
//        // GET: /Admin/DeletionReasons/
//        public ActionResult Index(DeletionReasonsListView model)
//        {
//            if (model.page < 1)
//            {
//                model.page = 1;
//            }

//            IEnumerable<DeletedReason> DeletedReasons = db.DeletedReasons;

//            if (model.onlyActive == true)
//            {
//                DeletedReasons = DeletedReasons.Where(c => c.active == true);
//            }
//            model.DeletionReasons = DeletedReasons.OrderBy(c => c.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
//            return View(model);
//        }
      
//    }
//}
