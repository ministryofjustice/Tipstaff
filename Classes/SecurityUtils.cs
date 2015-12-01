using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Principal;
using Tipstaff.Models;

namespace Tipstaff
{
    public enum AccessLevel
    {
        Denied = -1,
        Deactivated = 0,
        User = 25,
        Admin = 75,
        SystemAdmin = 100
    }

    public class CPrincipal : IPrincipal
    {
        //private UserDAL userDAL = new UserDAL();
        public User User { get; private set; }
        public IIdentity Identity { get; private set; }
        public AccessLevel AccessLevel { get; private set; }
        public int UserID { get; private set; }
        private TipstaffDB db { get; set; }

        #region Constructors
        public CPrincipal(TipstaffDB repository)
        {
            db = repository;
        }
        public CPrincipal(IIdentity identity): this(new TipstaffDB())
        {
            this.Identity = identity;
            User = db.GetUserByLoginName(Identity.Name.Split('\\').Last());
            this.AccessLevel = (AccessLevel)User.RoleStrength;
            this.UserID = User.UserID;
        }

        public CPrincipal(IIdentity identity, TipstaffDB rep)
        {
            db = rep;
            this.Identity = identity;
        }
        #endregion Constructors
        public bool IsInRole(string role)
        {
            return (this.User.Role.Detail == role);
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeRedirect : AuthorizeAttribute
    {
        private bool _isAuthorized;
        private AccessLevel UserAccessLevel;
        public string RedirectPrivateUrl = "~/Private";
        public string RedirectUnAuthUrl = "~/Error/Unauthorised";
        public string RedirectWrongTeam = "~/Error/WrongTeam";
        public AccessLevel MinimumRequiredAccessLevel { get; set; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            if (filterContext.HttpContext.User.GetType() != typeof(CPrincipal))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                _isAuthorized = (((CPrincipal)filterContext.HttpContext.User).AccessLevel >= MinimumRequiredAccessLevel);
            }
            if (!_isAuthorized && filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated && UserAccessLevel == AccessLevel.Denied)
            {
                filterContext.RequestContext.HttpContext.Response.Redirect(RedirectPrivateUrl);
            }
            else if (!_isAuthorized && filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated && (UserAccessLevel < MinimumRequiredAccessLevel))
            {
                filterContext.RequestContext.HttpContext.Response.Redirect(RedirectUnAuthUrl);
            }
            else // TODO find a way of trapping 'not it right team' issues here
            {
                //filterContext.RequestContext.HttpContext.Response.Redirect(RedirectWrongTeam);
            }
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            _isAuthorized = false;
            UserAccessLevel = AccessLevel.Denied;
            //check groups (strart with them for a bigger group target!)
            using (TipstaffDB db = new TipstaffDB())
            {
                UserAccessLevel = (AccessLevel)db.UserAccessLevel(httpContext.User);
            }
            _isAuthorized = (UserAccessLevel > AccessLevel.Denied && UserAccessLevel >= MinimumRequiredAccessLevel);


            IIdentity user = httpContext.User.Identity;
            CPrincipal cPrincipal = new CPrincipal(user);
            httpContext.User = cPrincipal;
            return _isAuthorized;
        }
    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AllowAnonymousAttribute : Attribute { }

    //public class SecurityUtils
    //{
    //    public const int DefaultPasswordExpiryInDays = 90;
    //    public static int PasswordExpiryInDays
    //    {
    //        get
    //        {
    //            string expiry = ConfigurationManager.AppSettings["PasswordExpiryInDays"];
    //            if (string.IsNullOrEmpty(expiry))
    //            {
    //                return DefaultPasswordExpiryInDays;
    //            }
    //            else
    //            {
    //                return Convert.ToInt32(expiry);
    //            }
    //        }
    //    }
    //    /// <summary>
    //    /// Returns an Integer value for the number of days until the 
    //    /// Users (username) password expires
    //    /// </summary>
    //    /// <param name="username"></param>
    //    /// <returns></returns>
    //    public static int daysTillPasswordExpires(string username)
    //    {
    //        MembershipUser user = Membership.GetUser(username);
    //        int passwordExpiryInDays = Tipstaff.SecurityUtils.PasswordExpiryInDays;
    //        int daysSincePwdChange = Convert.ToInt32(DateTime.Now.Subtract(user.LastPasswordChangedDate).TotalDays);
    //        int daysTillExpiry = (passwordExpiryInDays - daysSincePwdChange);
    //        return daysTillExpiry;
    //    }
    //    /// <summary>
    //    /// Returns an Integer value for the number of days until the 
    //    /// current users password expires
    //    /// </summary>
    //    /// <returns></returns>
    //    public static int daysTillPasswordExpires()
    //    {
    //        MembershipUser user = Membership.GetUser();
    //        if (user != null)
    //        {
    //            int passwordExpiryInDays = Tipstaff.SecurityUtils.PasswordExpiryInDays;
    //            int daysSincePwdChange = Convert.ToInt32(DateTime.Now.Subtract(user.LastPasswordChangedDate).TotalDays);
    //            int daysTillExpiry = (passwordExpiryInDays - daysSincePwdChange);
    //            return daysTillExpiry;
    //        }
    //        else
    //        {
    //            return -1;
    //        }
    //    }

    //}

    //public class EnforcePasswordPolicy : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        MembershipUser user = Membership.GetUser(filterContext.HttpContext.User.Identity.Name);
    //        if (user == null)
    //        {
    //            filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Error", action = "NotLoggedIn" }));
    //        }
    //        else if (user != null && user.IsApproved)
    //        {
    //            int daysSincePwdChange = Convert.ToInt32(DateTime.Now.Subtract(user.LastPasswordChangedDate).TotalDays);
    //            if ((user.CreationDate == user.LastPasswordChangedDate) || (daysSincePwdChange > SecurityUtils.PasswordExpiryInDays))
    //                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Account", action = "Changepassword" }));
    //        }
    //        else
    //        {
    //            filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Error", action = "unauthorised" }));
    //        }
    //        base.OnActionExecuting(filterContext);
    //    }
    //}
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    //public class AuthorizeRedirect : AuthorizeAttribute
    //{
    //    private bool _isAuthorized;

    //    public string RedirectUrl = "~/Error/unauthorised";

    //    protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
    //    {
    //        _isAuthorized = base.AuthorizeCore(httpContext);
    //        return _isAuthorized;
    //    }

    //    public override void OnAuthorization(AuthorizationContext filterContext)
    //    {
    //        base.OnAuthorization(filterContext);

    //        if (!_isAuthorized && filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated)
    //        {
    //            filterContext.RequestContext.HttpContext.Response.Redirect(RedirectUrl);
    //        }
    //    }
    //}
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    //public class PermissionRedirect : AuthorizeAttribute { }


    //public sealed class LogonAuthorize : AuthorizeAttribute
    //{
    //    public override void OnAuthorization(AuthorizationContext filterContext)
    //    {
    //        bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
    //        || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
    //        if (!skipAuthorization)
    //        {
    //            base.OnAuthorization(filterContext);
    //        }
    //    }
    //}
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    //public sealed class AllowAnonymousAttribute : Attribute { }

}