using System;
using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class FAQRepository : IFAQRepository
    {
        private readonly IDynamoAPI<FAQ> _dynamoAPI;
        private readonly IAuditEventRepository _auditRepo;

        public FAQRepository(IDynamoAPI<FAQ> dynamoAPI, IAuditEventRepository auditRepo)
        {
            _dynamoAPI = dynamoAPI;
            _auditRepo = auditRepo;
        }
        
        public void AddFaQ(FAQ faq)
        {
            _dynamoAPI.Save(faq);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "FAQ added",
                EventDate = DateTime.Now,
                RecordChanged = faq.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public void Delete(FAQ faq)
        {
            _dynamoAPI.Delete(faq);
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                AuditEventDescription = "FAQ deleted",
                EventDate = DateTime.Now,
                RecordChanged = faq.Id,
                UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            });
        }

        public IEnumerable<FAQ> GetAllFAQ()
        {
            return _dynamoAPI.GetAll();
        }

        public FAQ GetFAQ(string id)
        {
            return _dynamoAPI.GetEntityByKey(id);
        }

        public void Update(FAQ faq)
        {
            var entity = _dynamoAPI.GetEntityByKey(faq.Id);
            if (entity.Answer != faq.Answer)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "FAQ amended",
                    EventDate = DateTime.Now,
                    RecordChanged = faq.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Answer",
                    Was = entity.Answer,
                    Now = faq.Answer
                });
            }
            if (entity.LoggedInUser != faq.LoggedInUser)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "FAQ amended",
                    EventDate = DateTime.Now,
                    RecordChanged = faq.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "LoggedInUser",
                    Was = entity.LoggedInUser.ToString(),
                    Now = faq.LoggedInUser.ToString()
                });
            }
            if (entity.Question != faq.Question)
            {
                _auditRepo.AddAuditEvent(new AuditEvent()
                {
                    AuditEventDescription = "FAQ amended",
                    EventDate = DateTime.Now,
                    RecordChanged = faq.Id,
                    UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    ColumnName = "Question",
                    Was = entity.Question,
                    Now = faq.Question
                });
            }
            entity.Answer = faq.Answer;
            entity.LoggedInUser = faq.LoggedInUser;
            entity.Question = faq.Question;
            _dynamoAPI.Save(entity);
        }
    }
}
