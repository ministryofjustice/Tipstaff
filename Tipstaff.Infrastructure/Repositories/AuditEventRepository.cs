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

        public void AddAuditEvent(AuditEvent ae)
        {
            _dynamoAPI.Save(ae);
        }

        public void Delete(AuditEvent ae)
        {
            _dynamoAPI.Delete(ae);
        }

        public IEnumerable<AuditEvent> GetAllAuditEvents()
        {
            return _dynamoAPI.GetAll();
        }

        public AuditEvent GetAuditEvent(string id)
        {
            return _dynamoAPI.GetEntityByKey(id);
        }
    }
}
