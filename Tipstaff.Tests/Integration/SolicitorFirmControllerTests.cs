using Moq;
using NUnit.Framework;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tipstaff.Controllers;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Models;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;
using TPLibrary.GuidGenerator;

namespace Tipstaff.Tests.Integration
{
    [TestFixture]
    public class SolicitorFirmControllerTests:BaseController
    {
        private SolicitorFirmController _sub;
        private Mock<IGuidGenerator> _guidGeneratorMock;
        Guid id;
        Models.SolicitorFirm solicitorFirm;

        [SetUp]
        public void SetUp()
        {
            _tipstaffRecordRepository = new TipstaffRecordRepository(new DynamoAPI<Tipstaff.Services.DynamoTables.TipstaffRecord>(), _auditRepo);
            _guidGeneratorMock = new Mock<IGuidGenerator>();
            _solicitorFirmRepository = new SolicitorFirmRepository(new DynamoAPI<Tipstaff.Services.DynamoTables.SolicitorFirm>(), _auditRepo);
            _guidGeneratorMock = new Mock<IGuidGenerator>();
            id = Guid.NewGuid();
            _guidGeneratorMock.Setup(x => x.GenerateTimeBasedGuid()).Returns(id);
            _sub = new SolicitorFirmController(null,null);
            var mocks = new ContextMocks(_sub);
        }

        //[Test]
        //public void Create_Should_Add_New_SolicitorFirm()
        //{
        //    var response = _sub.Create(new Models.SolicitorFirm()
        //    {
        //        addressLine1 = "AddressLine1",
        //        addressLine2 = "AddressLine2",
        //        addressLine3 = "AddressLine3",
        //        county = "County",
        //        firmName = "IT Test",
        //        DX = "DX-Test",
        //        email = "test1@test.com",
        //        phoneDayTime = "0000000",
        //        town = "London",
        //        postcode = "E11",
        //        phoneOutofHours = "00:00--5:00",
        //        active = true,
        //        deactivated = DateTime.Now.ToUniversalTime(),
        //        deactivatedBy = "VZ"
        //    });

        //    var solicitorFirm = _solicitorFirmRepository.GetSolicitorFirm(id.ToString());

        //    Assert.AreEqual(id.ToString(), solicitorFirm.Id);
        //}

        //[Test]
        //public void Update_Should_Update_SolicitorFirm()
        //{
        //    var response = _sub.Create(new Models.SolicitorFirm()
        //    {
        //        addressLine1 = "AddressLine1",
        //        addressLine2 = "AddressLine2",
        //        addressLine3 = "AddressLine3",
        //        county = "County",
        //        firmName = "IT Test",
        //        DX = "DX-Test",
        //        email = "test1@test.com",
        //        phoneDayTime = "0000000",
        //        town = "London",
        //        postcode = "E11",
        //        phoneOutofHours = "00:00--5:00",
        //        active = true,
        //        deactivated = DateTime.Now.ToUniversalTime(),
        //        deactivatedBy = "VZ"
        //    });

        //    var modifiedFirm = new Models.SolicitorFirm()
        //    {
        //        addressLine1 = "AddressLine1-M",
        //        addressLine2 = "AddressLine2-M",
        //        addressLine3 = "AddressLine3-M",
        //        county = "County-M",
        //        firmName = "IT Test-M",
        //        DX = "DX-Test-MN",
        //        email = "test1@test.com-M",
        //        phoneDayTime = "0000000M",
        //        town = "London-Euston",
        //        postcode = "E11",
        //        phoneOutofHours = "00:00--5:09",
        //        active = true,
        //        deactivated = DateTime.Now.ToUniversalTime(),
        //        deactivatedBy = "VZ-M",
        //        solicitorFirmID = id.ToString()
        //    };

        //    response = _sub.Edit(new SolicitorFirmByTipstaffRecordViewModel()
        //    {
        //        SolicitorFirm = modifiedFirm
        //    });

        //    var solicitorFirm = _solicitorFirmRepository.GetSolicitorFirm(id.ToString());

        //    Assert.AreEqual(modifiedFirm.solicitorFirmID, solicitorFirm.Id);
        //    Assert.AreEqual(modifiedFirm.addressLine1, solicitorFirm.AddressLine1);
        //    Assert.AreEqual(modifiedFirm.addressLine2, solicitorFirm.AddressLine2);
        //    Assert.AreEqual(modifiedFirm.addressLine3, solicitorFirm.AddressLine3);
        //    Assert.AreEqual(modifiedFirm.county, solicitorFirm.County);
        //    Assert.AreEqual(modifiedFirm.email, solicitorFirm.Email);
        //    Assert.AreEqual(modifiedFirm.firmName, solicitorFirm.FirmName);

        //}

        public void DeleteConfirmed_Should_Delete_SolicitoprFirm()
        {

        }

        public void QuickSearch_Should_Return_JsonResult()
        {

        }
    }

    public class ContextMocks
    {
        public Mock<HttpContextBase> HttpContext { get; set; }
      
        public RouteData RouteData { get; set; }
        
        public ContextMocks(Controller controller)
        {
            var request = new Mock<HttpRequestBase>();
            //define context objects
            HttpContext = new Moq.Mock<HttpContextBase>();

            HttpContext.Setup(x => x.Request).Returns(request.Object);
           
            RequestContext rc = new RequestContext(HttpContext.Object, new RouteData());

            controller.ControllerContext = new ControllerContext(rc, controller);
        }
    }
}
