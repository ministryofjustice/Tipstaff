using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using Tipstaff.Controllers;
using Tipstaff.Models;
using TPLibrary.GuidGenerator;

namespace Tipstaff.Tests.Integration
{
    [TestFixture]
    public class CaseReviewControllerTests : BaseController
    {
        public CaseReviewController _sub;
        public Mock<IGuidGenerator> _guidGenerator;
        private Guid _id;
        private string _rangeKey;
        private string _record;
        private Tipstaff.Services.DynamoTables.TipstaffRecord _tipstaffRecord;
        private Tipstaff.Services.DynamoTables.CaseReview _caseReview;

        [SetUp]
        public void Setup()
        {
            _guidGenerator = new Mock<IGuidGenerator>();
            _sub = new CaseReviewController(_tipstaffRecordPresenter, _caseReviewPresenter, _guidGenerator.Object);
            _id = Guid.NewGuid();
            _record = Guid.NewGuid().ToString();
            _rangeKey = Guid.NewGuid().ToString();
         }


        //[Test]
        //public void Create_Should_Add_New_Case_Review()
        //{
        //    //Add TipStaff
        //    _tipstaffRecordRepository.Add(new Services.DynamoTables.TipstaffRecord()
        //    {
        //        ArrestCount = 1,
        //        //TrackItem = _rangeKey,
        //        Discriminator = "ChildAbduction",
        //        CreatedBy = "VZ",
        //        CreatedOn = DateTime.Now,
        //        OfficerDealing = "VZ",
        //        EldestChild = "John Doe",
        //        NPO = "NPO-Test",
        //        PrisonCount = 1,
        //        SentSCD26 = DateTime.Now,
        //        OrderReceived = DateTime.Now.AddDays(-1),
        //        DivisionId = 1,
        //        ResultDate = DateTime.Now.AddDays(10),
        //        Id = _record,
        //        DateExecuted = DateTime.Now.AddDays(-30),
        //        NextReviewDate = DateTime.Now.AddDays(90),
        //        ResultEnteredBy = "VZ",
        //        OrderDated = DateTime.Now,
        //        ProtectiveMarkingId = 1,
        //        CAOrderTypeId = 2,
        //        CaseStatusId = 3,
        //        ResultId = 4
        //    });

        //    _tipstaffRecord = _tipstaffRecordRepository.GetEntityByHashKey(_record);

        //    //CreateModel
        //    _guidGenerator.Setup(x => x.GenerateTimeBasedGuid()).Returns(_id);

        //    var model = new CaseReviewCreation()
        //    {
        //        CaseReview = new CaseReview()
        //        {
        //            reviewDate = DateTime.Now.AddDays(2),
        //            nextReviewDate = DateTime.Now.AddDays(10),
        //            actionTaken = "ActionTaken - Test",
        //            caseReviewStatus = MemoryCollections.CaseReviewStatusList.GetCaseReviewStatusList().First(),
        //            tipstaffRecordID = _tipstaffRecord.Id
        //        }
        //    };

        //    _sub.Create(model);

        //    _caseReview = _caseReviewRepository.GetEntityByKeys(_id.ToString(),_tipstaffRecord.Id);

        //    Assert.That(_caseReview != null);
        //    Assert.AreEqual(_caseReview.Id, _id.ToString());
        //}

        [TearDown]
        public void TearDown()
        {
            _tipstaffRecordRepository.Delete(_tipstaffRecord);
            _caseReviewRepository.Delete(_caseReview);
        }
    }
    
}
