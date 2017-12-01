using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Presenters.Interfaces;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class AuditEventPresenter : IAuditEventPresenter , IMapper<Models.AuditEvent, Tipstaff.Services.DynamoTables.AuditEvent>
    {
        private readonly IAuditEventRepository _auditEventRepository;

        public AuditEventPresenter(IAuditEventRepository auditEventRepository)
        {
            _auditEventRepository = auditEventRepository;
        }

        public IEnumerable<Models.AuditEvent> GetAuditEvents()
        {
            throw new NotImplementedException();
        }

        public Services.DynamoTables.AuditEvent GetDynamoTable(Models.AuditEvent model)
        {
            throw new NotImplementedException();
        }

        public Models.AuditEvent GetModel(Services.DynamoTables.AuditEvent dynamo)
        {
            var model = new Models.AuditEvent()
            {
                EventDate = dynamo.EventDate,
                RecordChanged = dynamo.RecordChanged,
                UserID = dynamo.UserId.ToString(),
                RecordAddedTo = dynamo.RecordAddedTo,
                idAuditEvent = int.Parse(dynamo.Id),
                auditEventDescription = MemoryCollections.AuditEventDescriptionList.GetAuditEventDescriptionList().FirstOrDefault(x=> x.Id == dynamo.AuditEventDescriptionId),

                
                
                
            };

            return model;
        }
    }
}