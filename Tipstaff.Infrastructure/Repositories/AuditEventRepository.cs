using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Infrastructure.Repositories
{
    public class AuditEventRepository : IAuditEventRepository
    {
        private readonly IDynamoAPI<AuditEvent> _dynamoAPI;

        public AuditEventRepository(IDynamoAPI<AuditEvent> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public IEnumerable<AuditEvent> GetAuditEvents()
        {
            return _dynamoAPI.GetAll();
        }
    }
}
