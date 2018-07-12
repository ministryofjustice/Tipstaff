using System;
using NUnit.Framework;
using Tipstaff.Services.Repositories;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Services.DynamoTables;
using TPLibrary.DynamoAPI;
using TPLibrary.GuidGenerator;
using System.Collections.Generic;
using System.Linq;

namespace Tipstaff.Tests.UnitTests
{
    [TestFixture]
    public class AuditEventNunitTests
    {
        private IAuditEventRepository _auditRepo;
        private IDynamoAPI<AuditEvent> _dynamoAPI;
        string aeIndex = string.Empty;
        AuditEvent ae;
        IEnumerable<AuditEvent> aes;

        [SetUp]
        public void SetUp()
        {

            _dynamoAPI = new DynamoAPI<AuditEvent>();
            _auditRepo = new AuditEventRepository(_dynamoAPI);
            aeIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
        }

        [Test]
        public void Create_Should_Add_New_AuditEvent()
        {
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                Id = aeIndex,
                AuditEventDescription = "desc audit event",
                DeletedReason = "desc deleted reason",
                EventDate = DateTime.Now,
                RecordAddedTo = "1",
                RecordChanged = "222",
                UserId = "any user",
                ColumnName = "anyColumn",
                Now = "now value",
                Was = "was value"
            });

            ae = _auditRepo.GetAuditEvent(aeIndex);

            Assert.AreEqual("desc audit event", ae.AuditEventDescription);
            Assert.AreEqual("desc deleted reason", ae.DeletedReason);
            Assert.AreEqual("1", ae.RecordAddedTo);
            Assert.AreEqual("any user", ae.UserId);
            Assert.AreEqual("now value", ae.Now);
            Assert.AreEqual("was value", ae.Was);
        }

        [Test]
        public void Create_Should_Add_New_AuditEventa_CheckGetAll()
        {
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                Id = aeIndex,
                AuditEventDescription = "desc audit event",
                DeletedReason = "desc deleted reason",
                EventDate = DateTime.Now,
                RecordAddedTo = "1",
                RecordChanged = "222",
                UserId = "any user",
                ColumnName = "anyColumn",
                Now = "now value",
                Was = "was value"
            });

            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                Id = "aaa",
                AuditEventDescription = "desc audit event 2",
                DeletedReason = "desc deleted reason 2",
                EventDate = DateTime.Now,
                RecordAddedTo = "1",
                RecordChanged = "222",
                UserId = "any user",
                ColumnName = "anyColumn 2",
                Now = "now value 2",
                Was = "was value 2"
            });

            aes = _auditRepo.GetAllAuditEvents();

            Assert.AreEqual(2, aes.Count());
        }


        [TearDown]
        public void TearDown()
        {
            if (ae!=null)_auditRepo.Delete(ae);
            if (aes != null)
            {
                foreach (var a in aes)
                {
                    _auditRepo.Delete(a);
                }
            }
        }

    }
}
