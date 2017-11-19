using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Tipstaff.Models;
using PagedList;
using System.Configuration;
using System.Web;
using System.Collections.Generic;

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    public class UserController : Controller
    {
        //private TipstaffDB db = myDBContextHelper.CurrentContext;
        private const string ResetPasswordBody = "Your new password is: {0}";
        private const string ResetPasswordSubject = "Your new TipstaffDB password";

        public ActionResult Index(int? page)
        {


            var allusers = Membership.GetAllUsers().Cast<MembershipUser>().AsEnumerable();
            IEnumerable<MembershipUser> admin = new List<MembershipUser>{Membership.GetUser("SSGAdmin")};
            var users = allusers.Except(admin, new MyUserComparer()).AsQueryable();
            return View(new IndexViewModel
            {
                Users = users.ToPagedList(page ?? 1, Int32.Parse(ConfigurationManager.AppSettings["pageSize"])),
                Roles = Roles.GetAllRoles(),
                IsRolesEnabled = true
            });
        }
        public class MyUserComparer : IEqualityComparer<MembershipUser>
        {
            public bool Equals(MembershipUser a, MembershipUser b)
            {
                return a.UserName == b.UserName;
            }

            public int GetHashCode(MembershipUser user)
            {
                return user.UserName.GetHashCode();
            }
        }

        #region Roles Section
        [ActionName("Roles")]
        public ViewResult URoles(Guid id)
        {
            var user = Membership.GetUser(id);
            var userRoles = Roles.GetRolesForUser(user.UserName); //_rolesService.Enabled
            //? _rolesService.FindByUser(user)
            //: Enumerable.Empty<string>();
            return View(new DetailsViewModel
            {
                DisplayName = user.UserName,
                User = user,
                Roles = Roles.GetAllRoles().ToDictionary(role => role, role => userRoles.Contains(role)),
                IsRolesEnabled = true,
                Status = user.IsOnline
                            ? DetailsViewModel.StatusEnum.Online
                            : !user.IsApproved
                                ? DetailsViewModel.StatusEnum.Disabled
                                : user.IsLockedOut
                                    ? DetailsViewModel.StatusEnum.LockedOut
                                    : DetailsViewModel.StatusEnum.Offline
            });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult AddToRole(Guid id, string role)
        {
            Roles.AddUserToRole(Membership.GetUser(id).UserName, role);
            return RedirectToAction("Roles", new { id });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult RemoveFromRole(Guid id, string role)
        {
            Roles.RemoveUserFromRole(Membership.GetUser(id).UserName, role);
            return RedirectToAction("Roles", new { id });
        }
        #endregion
        #region Password Section
        public ViewResult Password(Guid id)
        {
            var user = Membership.GetUser(id);
            var userRoles = Roles.GetRolesForUser(user.UserName); //_rolesService.Enabled
            //? _rolesService.FindByUser(user)
            //: Enumerable.Empty<string>();
            if (TempData["email"] != null)
            {
                ViewBag.email = TempData["email"];
                ViewBag.emailLink = TempData["emailLink"];
            }
            return View(new DetailsViewModel
            {
                DisplayName = user.UserName,
                User = user,
                IsRolesEnabled = true,
                Status = user.IsOnline
                            ? DetailsViewModel.StatusEnum.Online
                            : !user.IsApproved
                                ? DetailsViewModel.StatusEnum.Disabled
                                : user.IsLockedOut
                                    ? DetailsViewModel.StatusEnum.LockedOut
                                    : DetailsViewModel.StatusEnum.Offline
            });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult Unlock(Guid id)
        {
            var user = Membership.GetUser(id);
            user.UnlockUser();
            return RedirectToAction("Password", new { id });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult ResetPassword(Guid id)
        {
            var user = Membership.GetUser(id);
            string newPWD = user.ResetPassword();

            var body = string.Format(ResetPasswordBody, HttpUtility.UrlEncode(newPWD));

            TempData["email"] = string.Format("Password re-set to {0}", newPWD);
            TempData["emailLink"] = string.Format("mailto:{0}?subject={1}&body={2}", user.Email, ResetPasswordSubject, body);
            return RedirectToAction("Password", new { id });
        }
        #endregion
        #region Edit Section
        public ViewResult Edit(Guid id)
        {
            var user = Membership.GetUser(id);
            var userRoles = Roles.GetRolesForUser(user.UserName); //_rolesService.Enabled
            //? _rolesService.FindByUser(user)
            //: Enumerable.Empty<string>();
            return View(new DetailsViewModel
            {
                DisplayName = user.UserName,
                User = user,
                Roles = Roles.GetAllRoles().ToDictionary(role => role, role => userRoles.Contains(role)),
                IsRolesEnabled = true,
                Status = user.IsOnline
                            ? DetailsViewModel.StatusEnum.Online
                            : !user.IsApproved
                                ? DetailsViewModel.StatusEnum.Disabled
                                : user.IsLockedOut
                                    ? DetailsViewModel.StatusEnum.LockedOut
                                    : DetailsViewModel.StatusEnum.Offline
            });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ViewResult Edit(Guid id, string email, string comment)
        {
            var user = Membership.GetUser(id);
            try
            {
                user.Email = email;
                user.Comment = comment;
                Membership.UpdateUser(user);
                ViewBag.EmailMessage = "Updates saved";
            }
            catch(Exception ex)
            {
                ViewBag.EmailMessage = genericFunctions.GetLowestError(ex);
            }
            var userRoles = Roles.GetRolesForUser(user.UserName); //_rolesService.Enabled
            //return RedirectToAction("Edit", new { id });
            return View(new DetailsViewModel
            {
                DisplayName = user.UserName,
                User = user,
                Roles = Roles.GetAllRoles().ToDictionary(role => role, role => userRoles.Contains(role)),
                IsRolesEnabled = true,
                Status = user.IsOnline
                            ? DetailsViewModel.StatusEnum.Online
                            : !user.IsApproved
                                ? DetailsViewModel.StatusEnum.Disabled
                                : user.IsLockedOut
                                    ? DetailsViewModel.StatusEnum.LockedOut
                                    : DetailsViewModel.StatusEnum.Offline
            });

        }
        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult ChangeApproval(Guid id, bool isApproved)
        {
            var user = Membership.GetUser(id);
            user.IsApproved = isApproved;
            Membership.UpdateUser(user);
            return RedirectToAction("Edit", new { id });
        }
        #endregion
        #region Register Section
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;

                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    MembershipUser user = Membership.GetUser(model.UserName);
                    ////////////db.SaveChanges();
                    ViewData["newAccount"] = "newAccount";
                    return RedirectToAction("Roles", "User", new { id = user.ProviderUserKey });
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion
        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.InvalidPassword:
                    return "Invalid password, please check the requirements detailed above.";
                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
