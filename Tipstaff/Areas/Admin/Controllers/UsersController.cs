using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tipstaff.Models;
using Tipstaff;


namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class UsersController : Controller
    {
        TipstaffDB db = new TipstaffDB();
        public UsersController()
            : this(new TipstaffDB())
        { }
        public UsersController(TipstaffDB repository)
        {
            db = repository;
        }

        //
        // GET: /Admin/Users/

        public ActionResult Index()
        {
            Tipstaff.CPrincipal thisUser = (User as Tipstaff.CPrincipal);
            IEnumerable<User> allUsers = db.GetAllUsers();
            if (thisUser.AccessLevel == AccessLevel.SystemAdmin)
            {
                return View(allUsers);
            }
            else
            {
                List<User> model = new List<User>();
                foreach (User u in allUsers)
                {
                    if (u.RoleStrength <= (int)thisUser.AccessLevel)
                    {
                        model.Add(u);
                    }
                }
                return View(model);
            }
        }

        public ActionResult Create()
        {
            Tipstaff.CPrincipal thisUser = (User as Tipstaff.CPrincipal);
            UserAdminVM model = new UserAdminVM();
            model.Roles = new SelectList(db.GetAllRoles(), "strength", "Detail");
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(UserAdminVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    {
                        db.UserAdd(model.User);
                        return RedirectToAction("Index");
                    }
                }
                throw new Exception("Modelstate invalid");
            }
            catch (Exception)
            {
                model.Roles = new SelectList(db.GetAllRoles(), "strength", "Detail");
                return View(model);
            }
        }


        public ActionResult Edit(int id)
        {
            UserAdminVM model = new UserAdminVM();
            Tipstaff.CPrincipal thisUser = (User as Tipstaff.CPrincipal);
            model.User = db.GetUserByID(id);
            model.Roles = new SelectList(db.GetAllRoles(), "strength", "Detail", model.User.RoleStrength);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(UserAdminVM model)
        {
            if (ModelState.IsValid)
            {
                db.UpdateUser(model.User);
                return RedirectToAction("Index");
            }
            model.Roles = new SelectList(db.GetAllRoles(), "strength", "Detail", model.User.RoleStrength);
            return View(model);
        }

    }
}
