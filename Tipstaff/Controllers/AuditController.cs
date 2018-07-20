using System;
using System.Linq;
using System.Web.Mvc;
using Tipstaff.Models;
using PagedList;
using System.Configuration;
using Tipstaff.Presenters;
using TPLibrary.Logger;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class AuditController : Controller
    {
        private readonly IAuditEventPresenter _auditEventPresenter;
        private ICloudWatchLogger _logger;

        public AuditController(IAuditEventPresenter auditEventPresenter, ICloudWatchLogger logger)
        {
            _auditEventPresenter = auditEventPresenter;
            _logger = logger;
        }

        public ActionResult Audit(string auditType, string id, int? page)
        {
            if (page == null || page < 1)
            {
                page = 1;
            }

            AuditEventViewModel model = new AuditEventViewModel();
            try
            {
                string auditName = string.Format("{0}", auditType);
                var auditEvents = _auditEventPresenter.GetAllAuditEventsByIDAndAuditName(id, auditName);
                
                model.auditType = auditType;
                model.itemID = id;
                model.AuditEvents = auditEvents.ToPagedList(page ?? 1, 20);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in AuditController in Audit method, for user {((CPrincipal)User).UserID}");

            }
            return View(model);
        }
    }
}
