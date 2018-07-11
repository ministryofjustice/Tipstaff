using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Tipstaff.Models;
using Tipstaff.Presenters;
using TPLibrary.GuidGenerator;
using TPLibrary.Logger;

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class UsersController : Controller
    {
        private readonly IUsersPresenter _usersPresenter;
        private readonly ICloudWatchLogger _logger;
        private readonly IGuidGenerator _guidGenerator;

        public UsersController(IUsersPresenter usersPresenter, ICloudWatchLogger logger, IGuidGenerator guidGenerator)
        {
            _usersPresenter = usersPresenter;
            _logger = logger;
            _guidGenerator = guidGenerator;
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
                        model.User.UserID = _guidGenerator.GenerateTimeBasedGuid().ToString();
                        model.User.RoleStrength = model.User.RoleStrength;
                        model.User.Role = MemoryCollections.RolesList.GetRoleByStrength(model.User.RoleStrength);
                        _usersPresenter.Add(model.User);
                        return RedirectToAction("Index");
                    }
                }
                throw new Exception("Modelstate invalid");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in Users in Create method, for user {((CPrincipal)User).UserID}");
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

            model.User.Role = MemoryCollections.RolesList.GetRoleByStrength(model.User.RoleStrength);
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
                model.User.Role = MemoryCollections.RolesList.GetRoleByStrength(model.User.RoleStrength);
                _usersPresenter.Update(model.User);
                return RedirectToAction("Index");
            }

            var roles = _usersPresenter.GetAllRoles();
            
            model.Roles = new SelectList(roles, "Strength", "Detail", model.User.RoleStrength);
            return View(model);
        }

    }
}
