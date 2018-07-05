using System;
using System.Linq;
using System.Web.Mvc;
using Tipstaff.Models;
using PagedList;
using System.Configuration;
using Tipstaff.Presenters.Interfaces;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class AuditController : Controller
    {
        //private TipstaffDB db = myDBContextHelper.CurrentContext;
        private readonly IAuditEventPresenter _auditEventPresenter;

        public AuditController(IAuditEventPresenter auditEventPresenter)
        {
            _auditEventPresenter = auditEventPresenter;
        }

        public ActionResult Audit(string auditType, string id, int? page)
        {
            if (page == null || page < 1)
            {
                page = 1;
            }

            AuditEventViewModel model = new AuditEventViewModel();

            string AuditName = string.Format("{0}", auditType);
            ///var AuditMatches = db.AuditDescriptions.Where(a => a.AuditDescription.StartsWith(AuditName));

            var AuditMatches = MemoryCollections.AuditEventDescriptionList.GetAuditEventDescriptionList().Where(a => a.AuditDescription.StartsWith(AuditName));
            //////IQueryable<AuditEvent> auditEvents = null;
            var auditEvents = _auditEventPresenter.GetAuditEvents();
            if (AuditName == "Warrant" || AuditName == "ChildAbduction")
            {
                auditEvents = auditEvents.Where(s => s.RecordAddedTo == id).Union(auditEvents.Where(s => s.auditEventDescription.AuditDescription.StartsWith(AuditName) && s.RecordChanged == id));
            }
            else
            {
                auditEvents = auditEvents.Where(s => s.RecordChanged == id && AuditMatches.Select(d => d.Id).Contains(s.auditEventDescription.Id));
                //auditEvents = auditEvents.Where(s => s.RecordChanged == stringID && AuditMatches.Select(d => d.AuditDescription).Contains(s.idAuditEventDescription));
            }
            model.auditType = auditType;
            model.itemID = id;
            model.AuditEvents = auditEvents.OrderByDescending(s => s.EventDate).ToPagedList(page ?? 1, 20);
            return View(model);
        }
    }
}
