using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

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
