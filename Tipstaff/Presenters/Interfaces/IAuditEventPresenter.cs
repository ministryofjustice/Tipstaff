using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public interface IAuditEventPresenter
    {
        IEnumerable<AuditEvent> GetAllAuditEvents();

        IEnumerable<AuditEvent> GetAllAuditEventsByIDAndAuditName(string id, string auditName);

        void AddAuditEvent(AuditEvent ae);

        AuditEvent GetAuditEvent(string id);
    }
}
