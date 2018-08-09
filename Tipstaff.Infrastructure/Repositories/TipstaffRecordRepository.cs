using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class TipstaffRecordRepository : ITipstaffRecordRepository
    {
        private readonly IDynamoAPI<TipstaffRecord> _dynamoAPI;
        private readonly IAuditEventRepository _auditRepo;

        public TipstaffRecordRepository(IDynamoAPI<TipstaffRecord> dynamoAPI, IAuditEventRepository auditRepo)
        {
            _dynamoAPI = dynamoAPI;
            _auditRepo = auditRepo;
           
        }

        public void Add(TipstaffRecord record)
        {
            _dynamoAPI.Save(record);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = record.Discriminator + " added",
                EventDate = DateTime.Now,
                RecordChanged = record.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }
        
        public TipstaffRecord GetEntityByObjectKey(object hashKey, object rangeKey)
        {
            return _dynamoAPI.GetEntityByKeys(hashKey, rangeKey);
        }

        public TipstaffRecord GetEntityByHashKey(object hashKey)
        {
            return _dynamoAPI.GetEntityByKey(hashKey);
        }

        public void Update(TipstaffRecord record)
        {
            var entity = _dynamoAPI.GetEntityByKey(record.Id);

            if (entity.EldestChild != record.EldestChild)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "EldestChild",
                    Was = entity.EldestChild,
                    Now = record.EldestChild
                });
            }
            if (entity.ArrestCount != record.ArrestCount)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "ArrestCount",
                    Was = entity.ArrestCount.ToString(),
                    Now = record.ArrestCount.ToString()
                });
            }
            if (entity.ProtectiveMarkingId != record.ProtectiveMarkingId)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "ProtectiveMarkingId",
                    Was = entity.ProtectiveMarkingId.ToString(),
                    Now = record.ProtectiveMarkingId.ToString()
                });
            }
            if (entity.ResultId != record.ResultId)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "ResultId",
                    Was = entity.ResultId.ToString(),
                    Now = record.ResultId.ToString()
                });
            }
            if (entity.NextReviewDate != record.NextReviewDate)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "NextReviewDate",
                    Was = entity.NextReviewDate.ToString(),
                    Now = record.NextReviewDate.ToString()
                });
            }
            if (entity.ResultDate != record.ResultDate)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "ResultDate",
                    Was = entity.ResultDate.ToString(),
                    Now = record.ResultDate.ToString()
                });
            }
            if (entity.DateExecuted != record.DateExecuted)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "DateExecuted",
                    Was = entity.DateExecuted.ToString(),
                    Now = record.DateExecuted.ToString()
                });
            }
            if (entity.PrisonCount != record.PrisonCount)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PrisonCount",
                    Was = entity.PrisonCount.ToString(),
                    Now = record.PrisonCount.ToString()
                });
            }
            if (entity.NPO != record.NPO)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "NPO",
                    Was = entity.NPO,
                    Now = record.NPO
                });
            }
            if (entity.DivisionId != record.DivisionId)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "DivisionId",
                    Was = entity.DivisionId.ToString(),
                    Now = record.DivisionId.ToString()
                });
            }
            if (entity.CaseStatusId != record.CaseStatusId)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "CaseStatusId",
                    Was = entity.CaseStatusId.ToString(),
                    Now = record.CaseStatusId.ToString()
                });
            }
            if (entity.SentSCD26 != record.SentSCD26)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "SentSCD26",
                    Was = entity.SentSCD26.ToString(),
                    Now = record.SentSCD26.ToString()
                });
            }
            if (entity.OrderDated != record.OrderDated)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "OrderDated",
                    Was = entity.OrderDated.ToString(),
                    Now = record.OrderDated.ToString()
                });
            }
            if (entity.OrderReceived != record.OrderReceived)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "OrderReceived",
                    Was = entity.OrderReceived.ToString(),
                    Now = record.OrderReceived.ToString()
                });
            }
            if (entity.OfficerDealing != record.OfficerDealing)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "OfficerDealing",
                    Was = entity.OfficerDealing,
                    Now = record.OfficerDealing
                });
            }
            if (entity.ExpiryDate != record.ExpiryDate)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "ExpiryDate",
                    Was = entity.ExpiryDate.ToString(),
                    Now = record.ExpiryDate.ToString()
                });
            }
            if (entity.CAOrderTypeId != record.CAOrderTypeId)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "CAOrderTypeId",
                    Was = entity.CAOrderTypeId.ToString(),
                    Now = record.CAOrderTypeId.ToString()
                });
            }
            if (entity.CreatedOn != record.CreatedOn)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "CreatedOn",
                    Was = entity.CreatedOn.ToString(),
                    Now = record.CreatedOn.ToString()
                });
            }
            if (entity.CreatedBy != record.CreatedBy)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "CreatedBy",
                    Was = entity.CreatedBy,
                    Now = record.CreatedBy
                });
            }
            if (entity.DateCirculated != record.DateCirculated)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "DateCirculated",
                    Was = entity.DateCirculated.ToString(),
                    Now = record.DateCirculated.ToString()
                });
            }
            if (entity.RespondentName != record.RespondentName)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "RespondentName",
                    Was = entity.RespondentName,
                    Now = record.RespondentName
                });
            }
            if (entity.CaseNumber != record.CaseNumber)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "CaseNumber",
                    Was = entity.CaseNumber,
                    Now = record.CaseNumber
                });
            }
            if (entity.DivisionId != record.DivisionId)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = record.Discriminator + " amended",
                    EventDate = DateTime.Now,
                    RecordChanged = record.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "DivisionId",
                    Was = entity.DivisionId.ToString(),
                    Now = record.DivisionId.ToString()
                });
            }
            entity.EldestChild = record.EldestChild;
            entity.ArrestCount = record.ArrestCount;
            entity.ProtectiveMarkingId = record.ProtectiveMarkingId;
            entity.ResultId = record.ResultId;
            entity.NextReviewDate = record.NextReviewDate;
            entity.ResultDate = record.ResultDate;
            entity.DateExecuted = record.DateExecuted;
            entity.PrisonCount = record.PrisonCount;
            entity.ResultEnteredBy = record.ResultEnteredBy;
            entity.NPO = record.NPO;
            entity.DivisionId = record.DivisionId;
            entity.CaseStatusId = record.CaseStatusId;
            entity.SentSCD26 = record.SentSCD26;
            entity.OrderDated = record.OrderDated;
            entity.OrderReceived = record.OrderReceived;
            entity.OfficerDealing = record.OfficerDealing;
            entity.ExpiryDate = record.ExpiryDate;
            entity.CAOrderTypeId = record.CAOrderTypeId;
            entity.CreatedOn = record.CreatedOn;
            entity.CreatedBy = record.CreatedBy;
            entity.DateCirculated = record.DateCirculated;
            entity.RespondentName = entity.RespondentName;
            entity.CaseNumber = entity.CaseNumber;
            entity.DivisionId = entity.DivisionId;
            
            _dynamoAPI.Save(entity);
        }

        public IEnumerable<TipstaffRecord> GetAll()
        {
            return _dynamoAPI.GetAll();
        }

        public IEnumerable<TipstaffRecord> GetAllByCondition<T>(string name, T value)
        {
            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition(name, ScanOperator.Equal, value)
                }
            );
        }

        public IEnumerable<TipstaffRecord> GetAllByConditions<T>(IDictionary<string,T> conditions)
        {
            int size = conditions.Count;
            var scanConditions = new ScanCondition[size];
            int counter = 0;

            foreach (var item in conditions)
            {
                scanConditions[counter] = new ScanCondition(item.Key, ScanOperator.Equal, item.Value);
                counter++;
            }
            return _dynamoAPI.GetResultsByConditions(scanConditions);
        }

        public IEnumerable<TipstaffRecord> GetAllByConditionsOr<T>(IDictionary<string, T> conditions)
        {
            int size = conditions.Count;
            var scanConditions = new ScanCondition[size];
            int counter = 0;

            foreach (var item in conditions)
            {
                scanConditions[counter] = new ScanCondition(item.Key, ScanOperator.Equal, item.Value);
                counter++;
            }
            return _dynamoAPI.GetResultsByConditions(scanConditions, 
                new DynamoDBOperationConfig()
                {
                    ConditionalOperator =ConditionalOperatorValues.Or
                });
        }

        public void Delete(TipstaffRecord record)
        {
            _dynamoAPI.Delete(record);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = record.Discriminator + " deleted",
                EventDate = DateTime.Now,
                RecordChanged = record.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        
    }
}
