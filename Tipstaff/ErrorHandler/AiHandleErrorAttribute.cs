using System;
using System.Web.Mvc;
using TPLibrary.Logger;

namespace Tipstaff.ErrorHandler
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AiHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.HttpContext != null && filterContext.Exception != null)
            {
                //If customError is Off, then AI HTTPModule will report the exception
                if (filterContext.HttpContext.IsCustomErrorEnabled)
                {
                    var ai = new CloudWatchLogger();
                    ai.LogError(filterContext.Exception, "AiHandleErrorAttribute");
                }
            }
            base.OnException(filterContext);
        }
    }
}