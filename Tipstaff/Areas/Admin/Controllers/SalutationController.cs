//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Tipstaff.Models;
//using System.Configuration;
//using PagedList;
//using System.Data;
//using System.Data.Entity;

//namespace Tipstaff.Areas.Admin.Controllers
//{
//    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
//    [Authorize]
//    [ValidateAntiForgeryTokenOnAllPosts]
//    public class SalutationController : Controller
//    {
//        private TipstaffDB db = myDBContextHelper.CurrentContext;

//        //
//        // GET: /Admin/Salutation/
//        public ActionResult Index(SalutationListView model)
//        {
//            if (model.page < 1)
//            {
//                model.page = 1;
//            }

//            IEnumerable<Salutation> Salutations = db.Salutations;

//            if (model.onlyActive == true)
//            {
//                Salutations = Salutations.Where(c => c.active == true);
//            }
//            model.Salutations = Salutations.OrderBy(c => c.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
//            return View(model);
//        }
//    }
//}
