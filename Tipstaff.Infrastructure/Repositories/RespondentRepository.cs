using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;
using System.Linq;
using System;

namespace Tipstaff.Infrastructure.Repositories
{
    public class RespondentRepository : IRespondentRepository
    {
        private readonly IDynamoAPI<Respondent> _dynamoAPI;
        private readonly IAuditEventRepository _auditRepo;

        public RespondentRepository(IDynamoAPI<Respondent> dynamoAPI, IAuditEventRepository auditRepo)
        {
            _dynamoAPI = dynamoAPI;
            _auditRepo = auditRepo;
        }
        
        public void Add(Respondent respondent)
        {
            _dynamoAPI.Save(respondent);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "Respondent added",
                EventDate = DateTime.Now,
                RecordChanged = respondent.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public void Delete(Respondent respondent)
        {
            _dynamoAPI.Delete(respondent);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "Respondent deleted",
                EventDate = DateTime.Now,
                RecordChanged = respondent.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }
        
        public Respondent GetRespondent(string id)
        {

            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition("Id", ScanOperator.Equal, id)
                }).FirstOrDefault();

        }

        public IEnumerable<Respondent> GetAllRespondentsByTipstaffRecordID(string id)
        {
            return _dynamoAPI.GetResultsByConditions(
                new ScanCondition[]
                {
                    new ScanCondition("TipstaffRecordID", ScanOperator.Equal, id)
                });
        }

        public Respondent GetRespondentByKeys(string id, string rangeKey)
        {
            return _dynamoAPI.GetEntityByKeys(id,rangeKey);
            //return _dynamoAPI.GetResultsByConditions(
            //    new ScanCondition[]
            //    {
            //        new ScanCondition("Id", ScanOperator.Equal, id)
            //    }).FirstOrDefault();

        }


        public void Update(Respondent respondent)
        {
            var entity = _dynamoAPI.GetEntityByKeys(respondent.Id, respondent.Id);
            if (entity.Build != respondent.Build)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Respondent amended",
                    EventDate = DateTime.Now,
                    RecordChanged = respondent.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Build",
                    Was = entity.Build,
                    Now = respondent.Build
                });
            }
            if (entity.ChildRelationship != respondent.ChildRelationship)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Respondent amended",
                    EventDate = DateTime.Now,
                    RecordChanged = respondent.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "ChildRelationship",
                    Was = entity.ChildRelationship,
                    Now = respondent.ChildRelationship
                });
            }
            if (entity.Country != respondent.Country)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Respondent amended",
                    EventDate = DateTime.Now,
                    RecordChanged = respondent.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Country",
                    Was = entity.Country,
                    Now = respondent.Country
                });
            }
            if (entity.DateOfBirth != respondent.DateOfBirth)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Respondent amended",
                    EventDate = DateTime.Now,
                    RecordChanged = respondent.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "DateOfBirth",
                    Was = entity.DateOfBirth.ToString(),
                    Now = respondent.DateOfBirth.ToString()
                });
            }
            if (entity.EyeColour != respondent.EyeColour)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Respondent amended",
                    EventDate = DateTime.Now,
                    RecordChanged = respondent.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "EyeColour",
                    Was = entity.EyeColour,
                    Now = respondent.EyeColour
                });
            }
            if (entity.Gender != respondent.Gender)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Respondent amended",
                    EventDate = DateTime.Now,
                    RecordChanged = respondent.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Gender",
                    Was = entity.Gender,
                    Now = respondent.Gender
                });
            }
            if (entity.HairColour != respondent.HairColour)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Respondent amended",
                    EventDate = DateTime.Now,
                    RecordChanged = respondent.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "HairColour",
                    Was = entity.HairColour,
                    Now = respondent.HairColour
                });
            }
            if (entity.Height != respondent.Height)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Respondent amended",
                    EventDate = DateTime.Now,
                    RecordChanged = respondent.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Height",
                    Was = entity.Height,
                    Now = respondent.Height
                });
            }
            if (entity.NameFirst != respondent.NameFirst)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Respondent amended",
                    EventDate = DateTime.Now,
                    RecordChanged = respondent.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "NameFirst",
                    Was = entity.NameFirst,
                    Now = respondent.NameFirst
                });
            }
            if (entity.NameLast != respondent.NameLast)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Respondent amended",
                    EventDate = DateTime.Now,
                    RecordChanged = respondent.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "NameLast",
                    Was = entity.NameLast,
                    Now = respondent.NameLast
                });
            }
            if (entity.NameMiddle != respondent.NameMiddle)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Respondent amended",
                    EventDate = DateTime.Now,
                    RecordChanged = respondent.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "NameMiddle",
                    Was = entity.NameMiddle,
                    Now = respondent.NameMiddle
                });
            }
            if (entity.Nationality != respondent.Nationality)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Respondent amended",
                    EventDate = DateTime.Now,
                    RecordChanged = respondent.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Nationality",
                    Was = entity.Nationality,
                    Now = respondent.Nationality
                });
            }
            if (entity.PNCID != respondent.PNCID)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Respondent amended",
                    EventDate = DateTime.Now,
                    RecordChanged = respondent.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "PNCID",
                    Was = entity.PNCID,
                    Now = respondent.PNCID
                });
            }
            if (entity.RiskOfDrugs != respondent.RiskOfDrugs)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Respondent amended",
                    EventDate = DateTime.Now,
                    RecordChanged = respondent.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "RiskOfDrugs",
                    Was = entity.RiskOfDrugs,
                    Now = respondent.RiskOfDrugs
                });
            }
            if (entity.RiskOfViolence != respondent.RiskOfViolence)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Respondent amended",
                    EventDate = DateTime.Now,
                    RecordChanged = respondent.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "RiskOfViolence",
                    Was = entity.RiskOfViolence,
                    Now = respondent.RiskOfViolence
                });
            }
            if (entity.SkinColour != respondent.SkinColour)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Respondent amended",
                    EventDate = DateTime.Now,
                    RecordChanged = respondent.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "SkinColour",
                    Was = entity.SkinColour,
                    Now = respondent.SkinColour
                });
            }
            if (entity.Specialfeatures != respondent.Specialfeatures)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "Respondent amended",
                    EventDate = DateTime.Now,
                    RecordChanged = respondent.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Specialfeatures",
                    Was = entity.Specialfeatures,
                    Now = respondent.Specialfeatures
                });
            }
            
            entity.Build = respondent.Build;
            entity.ChildRelationship = respondent.ChildRelationship;
            entity.Country = respondent.Country;
            entity.DateOfBirth = respondent.DateOfBirth;
            entity.EyeColour = respondent.EyeColour;
            entity.Gender = respondent.Gender;
            entity.HairColour = respondent.HairColour;
            entity.Height = respondent.Height;
            entity.NameFirst = respondent.NameFirst;
            entity.NameLast = respondent.NameLast;
            entity.NameMiddle = respondent.NameMiddle;
            entity.Nationality = respondent.Nationality;
            entity.PNCID = respondent.PNCID;
            entity.RiskOfDrugs = respondent.RiskOfDrugs;
            entity.RiskOfViolence = respondent.RiskOfViolence;
            entity.SkinColour = respondent.SkinColour;
            entity.Specialfeatures = respondent.Specialfeatures;
            
            _dynamoAPI.Save(entity);
        }
    }
}
