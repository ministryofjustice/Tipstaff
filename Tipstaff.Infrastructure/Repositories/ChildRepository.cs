//using Amazon.DynamoDBv2.DataModel;
//using Amazon.DynamoDBv2.DocumentModel;
//using System;
//using System.Collections.Generic;
//using Tipstaff.Services.DynamoTables;
//using Tipstaff.Services.Repositories;
//using TPLibrary.DynamoAPI;

//namespace Tipstaff.Infrastructure.Repositories
//{
//    public class ChildRepository : IChildRepository
//    {
//        private readonly IDynamoAPI<Child> _dynamoAPI;
//        private readonly IAuditEventRepository _auditRepo;

//        public ChildRepository(IDynamoAPI<Child> dynamoAPI, IAuditEventRepository auditRepo)
//        {
//            _dynamoAPI = dynamoAPI;
//            _auditRepo = auditRepo;
//        }

//        public void AddChild(Child child)
//        {
//            _dynamoAPI.Save(child);
//            _auditRepo.AddAuditEvent(new AuditEvent()
//            {
//                AuditEventDescription = "Child added",
//                EventDate = DateTime.Now,
//                RecordChanged = child.Id,
//                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
//            });
//        }

//        public void Delete(Child child)
//        {
//            _dynamoAPI.Delete(child);
//            _auditRepo.AddAuditEvent(new AuditEvent()
//            {
//                AuditEventDescription = "Child deleted",
//                EventDate = DateTime.Now,
//                RecordChanged = child.Id,
//                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
//            });
//        }

//        public IEnumerable<Child> GetAllChildren()
//        {
//            return _dynamoAPI.GetAll();
//        }

//        public IEnumerable<Child> GetAllChildrenByTipstaffRecordID(string id)
//        {
//            return _dynamoAPI.GetResultsByConditions(
//                new ScanCondition[]
//                {
//                    new ScanCondition("TipstaffRecordID", ScanOperator.Equal, id)
//                });
//        }

//        public Child GetChild(string id)
//        {
//            return _dynamoAPI.GetEntityByKey(id);
//        }

//        public Child GetChildByIdAndRange(string id, string range)
//        {
//            return _dynamoAPI.GetEntityByKeys(id, range);
//        }

//        public void Update(Child child)
//        {
//            var entity = _dynamoAPI.GetEntityByKeys(child.Id, child.Id);
//            if (entity.NameFirst != child.NameFirst)
//            {
//                _auditRepo.AddAuditEvent(new AuditEvent()
//                {
//                    AuditEventDescription = "Child amended",
//                    EventDate = DateTime.Now,
//                    RecordChanged = child.Id,
//                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
//                    ColumnName = "NameFirst",
//                    Was = entity.NameFirst,
//                    Now = child.NameFirst
//                });
//            }
//            if (entity.NameLast != child.NameLast)
//            {
//                _auditRepo.AddAuditEvent(new AuditEvent()
//                {
//                    AuditEventDescription = "Child amended",
//                    EventDate = DateTime.Now,
//                    RecordChanged = child.Id,
//                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
//                    ColumnName = "NameLast",
//                    Was = entity.NameLast,
//                    Now = child.NameLast
//                });
//            }
//            if (entity.NameMiddle != child.NameMiddle)
//            {
//                _auditRepo.AddAuditEvent(new AuditEvent()
//                {
//                    AuditEventDescription = "Child amended",
//                    EventDate = DateTime.Now,
//                    RecordChanged = child.Id,
//                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
//                    ColumnName = "NameMiddle",
//                    Was = entity.NameMiddle,
//                    Now = child.NameMiddle
//                });
//            }
//            if (entity.DateOfBirth != child.DateOfBirth)
//            {
//                _auditRepo.AddAuditEvent(new AuditEvent()
//                {
//                    AuditEventDescription = "Child amended",
//                    EventDate = DateTime.Now,
//                    RecordChanged = child.Id,
//                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
//                    ColumnName = "DateOfBirth",
//                    Was = entity.DateOfBirth.ToString(),
//                    Now = child.DateOfBirth.ToString()
//                });
//            }
//            if (entity.Gender != child.Gender)
//            {
//                _auditRepo.AddAuditEvent(new AuditEvent()
//                {
//                    AuditEventDescription = "Child amended",
//                    EventDate = DateTime.Now,
//                    RecordChanged = child.Id,
//                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
//                    ColumnName = "Gender",
//                    Was = entity.Gender,
//                    Now = child.Gender
//                });
//            }
//            if (entity.Height != child.Height)
//            {
//                _auditRepo.AddAuditEvent(new AuditEvent()
//                {
//                    AuditEventDescription = "Child amended",
//                    EventDate = DateTime.Now,
//                    RecordChanged = child.Id,
//                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
//                    ColumnName = "Height",
//                    Was = entity.Height,
//                    Now = child.Height
//                });
//            }
//            if (entity.Build != child.Build)
//            {
//                _auditRepo.AddAuditEvent(new AuditEvent()
//                {
//                    AuditEventDescription = "Child amended",
//                    EventDate = DateTime.Now,
//                    RecordChanged = child.Id,
//                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
//                    ColumnName = "Build",
//                    Was = entity.Build,
//                    Now = child.Build
//                });
//            }
//            if (entity.HairColour != child.HairColour)
//            {
//                _auditRepo.AddAuditEvent(new AuditEvent()
//                {
//                    AuditEventDescription = "Child amended",
//                    EventDate = DateTime.Now,
//                    RecordChanged = child.Id,
//                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
//                    ColumnName = "HairColour",
//                    Was = entity.HairColour,
//                    Now = child.HairColour
//                });
//            }
//            if (entity.EyeColour != child.EyeColour)
//            {
//                _auditRepo.AddAuditEvent(new AuditEvent()
//                {
//                    AuditEventDescription = "Child amended",
//                    EventDate = DateTime.Now,
//                    RecordChanged = child.Id,
//                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
//                    ColumnName = "EyeColour",
//                    Was = entity.EyeColour,
//                    Now = child.EyeColour
//                });
//            }
//            if (entity.SkinColour != child.SkinColour)
//            {
//                _auditRepo.AddAuditEvent(new AuditEvent()
//                {
//                    AuditEventDescription = "Child amended",
//                    EventDate = DateTime.Now,
//                    RecordChanged = child.Id,
//                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
//                    ColumnName = "SkinColour",
//                    Was = entity.SkinColour,
//                    Now = child.SkinColour
//                });
//            }
//            if (entity.Specialfeatures != child.Specialfeatures)
//            {
//                _auditRepo.AddAuditEvent(new AuditEvent()
//                {
//                    AuditEventDescription = "Child amended",
//                    EventDate = DateTime.Now,
//                    RecordChanged = child.Id,
//                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
//                    ColumnName = "Specialfeatures",
//                    Was = entity.Specialfeatures,
//                    Now = child.Specialfeatures
//                });
//            }
//            if (entity.Country != child.Country)
//            {
//                _auditRepo.AddAuditEvent(new AuditEvent()
//                {
//                    AuditEventDescription = "Child amended",
//                    EventDate = DateTime.Now,
//                    RecordChanged = child.Id,
//                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
//                    ColumnName = "Country",
//                    Was = entity.Country,
//                    Now = child.Country
//                });
//            }
//            if (entity.Nationality != child.Nationality)
//            {
//                _auditRepo.AddAuditEvent(new AuditEvent()
//                {
//                    AuditEventDescription = "Child amended",
//                    EventDate = DateTime.Now,
//                    RecordChanged = child.Id,
//                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
//                    ColumnName = "Nationality",
//                    Was = entity.Nationality,
//                    Now = child.Nationality
//                });
//            }
//            if (entity.PNCID != child.PNCID)
//            {
//                _auditRepo.AddAuditEvent(new AuditEvent()
//                {
//                    AuditEventDescription = "Child amended",
//                    EventDate = DateTime.Now,
//                    RecordChanged = child.Id,
//                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
//                    ColumnName = "PNCID",
//                    Was = entity.PNCID,
//                    Now = child.PNCID
//                });
//            }
//            entity.NameFirst = child.NameFirst;
//            entity.NameLast = child.NameLast;
//            entity.NameMiddle = child.NameMiddle;
//            entity.DateOfBirth = child.DateOfBirth;
//            entity.Gender = child.Gender;
//            entity.Height = child.Height;
//            entity.Build = child.Build;
//            entity.HairColour = child.HairColour;
//            entity.EyeColour = child.EyeColour;
//            entity.SkinColour = child.SkinColour;
//            entity.Specialfeatures = child.Specialfeatures;
//            entity.Country = child.Country;
//            entity.Nationality = child.Nationality;
//            entity.PNCID = child.PNCID;
            
//            _dynamoAPI.Save(entity);
//        }
//    }
//}
