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
                AuditEventDescriptionId = 1,
                DeletedReasonId = 1,
                EventDate = DateTime.Now,
                RecordAddedTo = "1",
                RecordChanged = "222",
                UserId = "any user"
            });

            ae = _auditRepo.GetAuditEvent(aeIndex);

            Assert.AreEqual(1, ae.AuditEventDescriptionId);
            Assert.AreEqual(1, ae.DeletedReasonId);
            Assert.AreEqual("1", ae.RecordAddedTo);
            Assert.AreEqual("any user", ae.UserId);
        }

        [Test]
        public void Create_Should_Add_New_AuditEventa_CheckGetAll()
        {
            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                Id = aeIndex,
                AuditEventDescriptionId = 1,
                DeletedReasonId = 1,
                EventDate = DateTime.Now,
                RecordAddedTo = "1",
                RecordChanged = "222",
                UserId = "any user"
            });

            _auditRepo.AddAuditEvent(new AuditEvent()
            {
                Id = "aaa",
                AuditEventDescriptionId = 2,
                DeletedReasonId = 2,
                EventDate = DateTime.Now,
                RecordAddedTo = "1",
                RecordChanged = "222",
                UserId = "any user"
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
