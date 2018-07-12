using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface IAuditEventRepository
    {
        IEnumerable<AuditEvent> GetAllAuditEvents();

        void AddAuditEvent(AuditEvent ae);

        AuditEvent GetAuditEvent(string id);

        void Delete(AuditEvent ae);
    }
}
