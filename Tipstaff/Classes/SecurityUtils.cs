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
        private DateTime lastCheck;
        private User systemUser;
        public int UserID { get; private set; }
        private readonly TimeSpan refreshInterval = TimeSpan.FromMinutes(10);

        public IIdentity Identity { get; private set; }
        private ISourceRepository Db { get; }

        private readonly Guid instanceId = Guid.NewGuid();
        private static readonly ICloudWatchLogger logger = new CloudWatchLogger();

        // --- Constructors ---------------------------------------------------
        public ICurrentUser(ISourceRepository repository)
        {
            Log($"CTOR(repo) instance={instanceId}");
            Db = repository ?? throw new ArgumentNullException(nameof(repository));
            lastCheck = DateTime.MinValue;
        }

        public ICurrentUser(IIdentity identity) : this(identity, new TipstaffDB())
        {
            Log($"CTOR(identity only) instance={instanceId}");
        }

        public ICurrentUser(IIdentity identity, ISourceRepository repository)
        {
            Log($"CTOR(identity+repo) instance={instanceId} user={identity.Name}");
            Identity = identity ?? throw new ArgumentNullException(nameof(identity));
            Db = repository ?? throw new ArgumentNullException(nameof(repository));
            lastCheck = DateTime.MinValue;
            LoadUserIfNeeded();
        }

        // --- Public Properties ---------------------------------------------------
        public int UserId
        {
            get
            {
                LoadUserIfNeeded();
                return systemUser?.UserID ?? 0;
            }
        }

        public AccessLevel AccessLevel
        {
            get
            {
                LoadUserIfNeeded();
                return (AccessLevel)(systemUser?.Role.strength ?? 0);
            }
        }

        public string DisplayName
        {
            get
            {
                LoadUserIfNeeded();
                return systemUser?.DisplayName ?? string.Empty;
            }
        }

        public bool IsInRole(string role)
        {
            return (this.User.Role.Detail == role);
        }

        // --- Internal refresh logic ---------------------------------------------

        private void LoadUserIfNeeded()
        {
            var now = DateTime.Now;
            var shouldReload =
                systemUser == null ||
                (now - lastCheck) > refreshInterval;

            if (!shouldReload)
                return;

            string username = Identity?.Name?.Split('\\').Last();

            if (string.IsNullOrWhiteSpace(username))
                return;

            Log(
                $"LoadUserIfNeeded instance=<{instanceId}> " +
                $"UserName=<{username}>" +
                $"systemUserNull=<{systemUser == null}> " +
                $"lastCheck=<{lastCheck:o}> " +
                $"now=<{now:o}> " +
                $"shouldReload=<{shouldReload}>"
            );

            systemUser = Db.GetUserByLoginName(username);
            lastCheck = now;
        }

        private static void Log(string message)
        {
            logger.LogInfo($"{message}");
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeRedirect : AuthorizeAttribute
    {
        private bool _isAuthorized = false;
        private AccessLevel UserAccessLevel;
        public string RedirectPrivateUrl = "~/Private";
        public string RedirectUnAuthUrl = "~/Error/Unauthorised";
        public string RedirectWrongTeam = "~/Error/WrongTeam";
        public AccessLevel MinimumRequiredAccessLevel { get; set; }
        private static readonly ICloudWatchLogger logger = new CloudWatchLogger();

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!_isAuthorized && filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated && UserAccessLevel == AccessLevel.Denied)
            {
                filterContext.RequestContext.HttpContext.Response.Redirect(RedirectPrivateUrl);
            }
            else if (!_isAuthorized && filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated && (UserAccessLevel < MinimumRequiredAccessLevel))
            {
                filterContext.RequestContext.HttpContext.Response.Redirect(RedirectUnAuthUrl);
            }
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var identity = httpContext.User?.Identity;

            if (identity == null || !identity.IsAuthenticated)
            {
                return false;
            }

            logger.LogInfo($"User Identity <{identity}> Name <{identity.Name}>");

            using (ISourceRepository db = new TipstaffDB())
            {
                var userAccessLevel =
                    (AccessLevel)db.UserAccessLevel(httpContext.User);

                _isAuthorized = userAccessLevel >= MinimumRequiredAccessLevel;
                logger.LogInfo($"UserAccessLevel <{userAccessLevel}> MinimumRequiredAccessLevel <{MinimumRequiredAccessLevel}>");
            }
            return _isAuthorized;
        }

    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AllowAnonymousAttribute : Attribute { }

    public sealed class LogonAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

                base.OnAuthorization(filterContext);
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                IIdentity user = httpContext.User.Identity;
                ICurrentUser cPrincipal = new ICurrentUser(user);
                httpContext.User = cPrincipal;
                return true; // always true as anonymous allowed
            }
            catch
            {
                return false;
            }
        }
    }
}