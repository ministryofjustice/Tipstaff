using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Tipstaff.Models;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    public class AccountController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;
        public ActionResult myProfile()
        {
            Tipstaff.Models.ProfileModel model = new ProfileModel();
            model.userName = User.Identity.Name;
           // model.passwordAge = SecurityUtils.daysTillPasswordExpires().ToString();
            return View(model);
        }

        public ActionResult passwordExpires()
        {
            int PasswordExpiresInDays = (int)TempData["passwordExpiresInDays"];
            if (PasswordExpiresInDays == 1)
            {
                ViewBag.ErrMessage = "Your password will expire in 1 day, do you want to change it?";
            }
            else if (PasswordExpiresInDays < 15)
            {
                ViewBag.ErrMessage = string.Format("Your password will expire in {0} days, do you want to change it?", PasswordExpiresInDays);
            }
            return View();
        }
        //
        // GET: /Account/LogOn
        [AllowAnonymous]
        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {

                    MembershipUser user = Membership.GetUser(model.UserName.ToLower());

                    int PasswordExpiresInDays = 10000; //SecurityUtils.daysTillPasswordExpires(model.UserName.ToString());
                    if ((PasswordExpiresInDays <= 0) || (user.CreationDate == user.LastPasswordChangedDate))
                    {
                        return RedirectToAction("ForcePassword", "Account");
                    }
                    FormsAuthentication.SetAuthCookie(user.UserName, model.RememberMe);
                    if (PasswordExpiresInDays < 15)
                    {
                        TempData["passwordExpiresInDays"] = PasswordExpiresInDays;
                        return RedirectToAction("passwordExpires");
                    }

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        //
        // GET: /Account/ChangePassword
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    ModelState.AddModelError("", "Please note: You cannot re-use previous passwords");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //[Authorize]
        [AllowAnonymous]
        public ActionResult ForcePassword()
        {
            return View();
        }

        //
        // POST: /Account/ForcePassword

        //[Authorize]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForcePassword(ForcePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.OldPassword))
                {

                    // ChangePassword will throw an exception rather
                    // than return false in certain failure scenarios.
                    bool ForcePasswordSucceeded;
                    try
                    {
                        MembershipUser currentUser = Membership.GetUser(model.UserName /* userIsOnline */);
                        ForcePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        ForcePasswordSucceeded = false;
                    }

                    if (ForcePasswordSucceeded)
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, false);
                        return RedirectToAction("ChangePasswordSuccess");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        //
        // GET: /Account/ChangePassword

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
