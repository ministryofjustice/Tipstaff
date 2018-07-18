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
    public class AuditEventPresenter : IAuditEventPresenter , IMapper<Models.AuditEvent, Services.DynamoTables.AuditEvent>
    {
        private readonly IAuditEventRepository _auditEventRepository;

        public AuditEventPresenter(IAuditEventRepository auditEventRepository)
        {
            _auditEventRepository = auditEventRepository;
        }

        public void AddAuditEvent(Models.AuditEvent ae)
        {
            var entity = GetDynamoTable(ae);

            _auditEventRepository.AddAuditEvent(entity);
        }

        public IEnumerable<Models.AuditEvent> GetAllAuditEvents()
        {
            var list = _auditEventRepository.GetAllAuditEvents();
            var aeList = list.Select(x => GetModel(x));

            return aeList;
        }

        public IEnumerable<Models.AuditEvent> GetAllAuditEventsByIDAndAuditName(string id, string auditName)
        {
            IEnumerable<Models.AuditEvent> auditEvents;
            var aes = _auditEventRepository.GetAllAuditEventsByAuditEventDescriptionAndRecordChanged(auditName, id);

            auditEvents = aes.Select(x => GetModel(x));

            if (auditName == "Warrant" || auditName == "ChildAbduction")
            {
                aes = _auditEventRepository.GetAllAuditEventsByRecordAddedTo(id);
                auditEvents = auditEvents.Union(aes.Select(x => GetModel(x)));
            }

            return auditEvents.OrderByDescending(s => s.EventDate);
        }

        public Models.AuditEvent GetAuditEvent(string id)
        {
            var entity = _auditEventRepository.GetAuditEvent(id);
            var model = GetModel(entity);

            return model;
        }

        public Services.DynamoTables.AuditEvent GetDynamoTable(Models.AuditEvent model)
        {
            var table = new Services.DynamoTables.AuditEvent()
            {
                Id = model.idAuditEvent,
                AuditEventDescription = model.auditEventDescription.AuditDescription,
                ColumnName = model.ColumnName,
                DeletedReason = model.DeletedReason.Detail,
                EventDate = model.EventDate,
                Now = model.Now,
                RecordAddedTo = model.RecordAddedTo,
                RecordChanged = model.RecordChanged,
                UserId = model.UserID,
                Was = model.Was
            };

            return table;
        }

        public Models.AuditEvent GetModel(Services.DynamoTables.AuditEvent table)
        {
            var model = new Models.AuditEvent()
            {
                EventDate = table.EventDate,
                RecordChanged = table.RecordChanged,
                UserID = table.UserId,
                RecordAddedTo = table.RecordAddedTo,
                idAuditEvent = table.Id,
                auditEventDescription = MemoryCollections.AuditEventDescriptionList.GetAuditEventDescriptionByDetail(table.AuditEventDescription),
                DeletedReason = MemoryCollections.DeletedReasonList.GetDeletedReasonByDetail(table.DeletedReason),
                ColumnName = table.ColumnName,
                Now = table.Now,
                Was = table.Was
            };

            return model;
        }
    }
}