using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Tipstaff.Models;
using Tipstaff.Presenters.Interfaces;

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class UsersController : Controller
    {
        ////TipstaffDB db = new TipstaffDB();
        ////public UsersController()
        ////    : this(new TipstaffDB())
        ////{ }
        ////public UsersController(TipstaffDB repository)
        ////{
        ////    db = repository;
        ////}

        //
        // GET: /Admin/Users/

        private readonly IUsersPresenter _usersPresenter;

        public UsersController(IUsersPresenter usersPresenter)
        {
            _usersPresenter = usersPresenter;
        }


        public ActionResult Index()
        {
            Tipstaff.CPrincipal thisUser = (User as Tipstaff.CPrincipal);
            //////IEnumerable<User> allUsers = db.GetAllUsers();
            IEnumerable<User> allUsers = _usersPresenter.GetAllUsers();
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

            var roles = _usersPresenter.GetAllRoles();

            //////model.Roles = new SelectList(db.GetAllRoles(), "Strength", "Detail");
            model.Roles = new SelectList(roles, "Strength", "Detail");
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
                        //////db.UserAdd(model.User);
                        _usersPresenter.Add(model.User);
                        return RedirectToAction("Index");
                    }
                }
                throw new Exception("Modelstate invalid");
            }
            catch (Exception)
            {
                var roles = _usersPresenter.GetAllRoles();
                //////model.Roles = new SelectList(db.GetAllRoles(), "Strength", "Detail");
                model.Roles = new SelectList(roles, "Strength", "Detail");
                return View(model);
            }
        }


        public ActionResult Edit(string id)
        {
            UserAdminVM model = new UserAdminVM();
            Tipstaff.CPrincipal thisUser = (User as Tipstaff.CPrincipal);
            //////model.User = db.GetUserByID(id);

            model.User = _usersPresenter.GetUserByID(id);
            ////model.Roles = new SelectList(db.GetAllRoles(), "Strength", "Detail", model.User.RoleStrength);
            var roles = _usersPresenter.GetAllRoles();
            model.Roles = new SelectList(roles, "Strength", "Detail", model.User.RoleStrength);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(UserAdminVM model)
        {
            if (ModelState.IsValid)
            {
                //////db.UpdateUser(model.User);
                _usersPresenter.Update(model.User);
                return RedirectToAction("Index");
            }

            var roles = _usersPresenter.GetAllRoles();
            //////model.Roles = new SelectList(db.GetAllRoles(), "Strength", "Detail", model.User.RoleStrength);
            model.Roles = new SelectList(roles, "Strength", "Detail", model.User.RoleStrength);
            return View(model);
        }

    }
}
