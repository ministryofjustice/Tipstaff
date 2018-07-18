﻿using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
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

        public IEnumerable<AuditEvent> GetAllAuditEventsByAuditEventDescriptionAndRecordChanged(string auditDesc, string id)
        {
            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition("RecordChanged", ScanOperator.Equal, id),
                    new ScanCondition("AuditEventDescription", ScanOperator.BeginsWith, auditDesc)
                });
        }

        public IEnumerable<AuditEvent> GetAllAuditEventsByRecordAddedTo(string id)
        {
            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition("RecordAddedTo", ScanOperator.Equal, id)
                });
        }

        public AuditEvent GetAuditEvent(string id)
        {
            return _dynamoAPI.GetEntityByKey(id);
        }
    }
}
