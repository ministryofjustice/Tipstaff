using System;
using System.Linq;
using System.Web.Mvc;
using Tipstaff.Models;
using PagedList;
using System.Configuration;
namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    public class AuditController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;

        public ActionResult Audit(string auditType, int id, int? page)
        {
            if (page == null || page < 1)
            {
                page = 1;
            }

            AuditEventViewModel model = new AuditEventViewModel();

            string stringID = id.ToString();
            string AuditName = string.Format("{0}", auditType);
            var AuditMatches = db.AuditDescriptions.Where(a => a.AuditDescription.StartsWith(AuditName));
            IQueryable<AuditEvent> auditEvents = null;
            if (AuditName == "Warrant" || AuditName == "ChildAbduction")
            {
                auditEvents = db.AuditEvents.Where(s => s.RecordAddedTo == id).Union(db.AuditEvents.Where(s => s.auditEventDescription.AuditDescription.StartsWith(AuditName) && s.RecordChanged == stringID));
            }
            else
            {
                auditEvents = db.AuditEvents.Where(s => s.RecordChanged == stringID && AuditMatches.Select(d => d.idAuditEventDescription).Contains(s.idAuditEventDescription));
            }
            model.auditType = auditType;
            model.itemID = stringID;
            model.AuditEvents = auditEvents.OrderByDescending(s => s.EventDate).ToPagedList(page ?? 1, 20);
            return View(model);
        }
    }
}
