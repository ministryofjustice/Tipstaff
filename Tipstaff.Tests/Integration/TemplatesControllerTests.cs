using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using System.Web;
using Tipstaff.Areas.Admin.Controllers;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;
using TPLibrary.GuidGenerator;
using TPLibrary.Logger;
using TPLibrary.S3API;

namespace Tipstaff.Tests.Integration
{

    [TestFixture]
    public class TemplatesControllerTests:BaseController
    {
        private TemplatesController _sub;
        private Controllers.TemplateController _subMain;
        private IS3API _s3Repository;
        private IDynamoAPI<Template> _dynamoAPI;
        private IDynamoAPI<TipstaffRecord> _dynamoAPITR;
        public Mock<IGuidGenerator> _guidGenerator;
        Guid templateIndex;
        Guid trIndex;
        private Mock<ICloudWatchLogger> _cloudWatchLogger = new Mock<ICloudWatchLogger>();
        Template template;
        TipstaffRecord tr;


        [SetUp]
        public void SetUp()
        {

            _dynamoAPI = new DynamoAPI<Template>();
            _dynamoAPITR = new DynamoAPI<TipstaffRecord>();
            _templateRepository = new TemplateRepository(_dynamoAPI, _auditRepo);
            _tipstaffRecordRepository = new TipstaffRecordRepository(_dynamoAPITR, _auditRepo);
            _s3Repository = new S3API();
            _guidGenerator = new Mock<IGuidGenerator>();
            templateIndex = Guid.NewGuid();
            trIndex = Guid.NewGuid();
            _sub = new TemplatesController(_templatePresenter, _s3Repository, _guidGenerator.Object, _cloudWatchLogger.Object);
            _subMain = new Controllers.TemplateController(_cloudWatchLogger.Object, _s3Repository, _templatePresenter, _tipstaffRecordPresenter, _warrantPresenter, _applicantPresenter, _solicitorPresenter);
        }

        [Test]
        public void Create_Should_Add_New_Template()
        {
            Models.TemplateEdit te = new Models.TemplateEdit();
            te.Template.templateID = templateIndex.ToString();
            te.Template.templateName = "template name";
            te.Template.Discriminator = "Warrant";
            Mock<HttpPostedFileBase> uploadFile = new Mock<HttpPostedFileBase>();
            uploadFile
            .Setup(f => f.ContentLength)
            .Returns(10);

            uploadFile
                .Setup(f => f.FileName)
                .Returns("testtemplate.xml");

            uploadFile
                .Setup(f => f.InputStream)
                .Returns(new MemoryStream(Encoding.UTF8.GetBytes("test file")));

            te.uploadFile = uploadFile.Object;

            _guidGenerator.Setup(x => x.GenerateTimeBasedGuid()).Returns(templateIndex);
            var response = _sub.Create(te);

            template = _templateRepository.GetTemplate(templateIndex.ToString());
            Assert.AreEqual("Warrant", template.Discriminator);
            Assert.AreEqual("template name", template.TemplateName);
        }

        [Test]
        public void Update_Should_Update_Template()
        {
            Models.TemplateEdit te = new Models.TemplateEdit();
            te.Template.templateID = templateIndex.ToString();
            te.Template.templateName = "template name";
            te.Template.Discriminator = "Warrant";
            te.Template.addresseeRequired = true;
            Mock<HttpPostedFileBase> uploadFile = new Mock<HttpPostedFileBase>();
            uploadFile
            .Setup(f => f.ContentLength)
            .Returns(10);

            uploadFile
                .Setup(f => f.FileName)
                .Returns("testtemplate.xml");

            uploadFile
                .Setup(f => f.InputStream)
                .Returns(new System.IO.MemoryStream(Encoding.UTF8.GetBytes("test file")));

            te.uploadFile = uploadFile.Object;

            _guidGenerator.Setup(x => x.GenerateTimeBasedGuid()).Returns(templateIndex);
            var response = _sub.Create(te);



            te = new Models.TemplateEdit();
            te.Template.templateID = templateIndex.ToString();
            te.Template.templateName = "template name modified";
            te.Template.Discriminator = "Warrant";
            te.Template.addresseeRequired = false;

            response = _sub.Edit(te);

            template = _templateRepository.GetTemplate(templateIndex.ToString());

            Assert.AreEqual("template name modified", template.TemplateName);
            Assert.AreNotEqual(true, template.AddresseeRequired);
            Assert.AreEqual("Warrant", template.Discriminator);
        }

        [Test]
        public void Update_Should_Open_Template()
        {
            Models.TemplateEdit te = new Models.TemplateEdit();
            te.Template.templateID = templateIndex.ToString();
            te.Template.templateName = "template name";
            te.Template.Discriminator = "Warrant";
            te.Template.addresseeRequired = true;
            Mock<HttpPostedFileBase> uploadFile = new Mock<HttpPostedFileBase>();
            uploadFile
            .Setup(f => f.ContentLength)
            .Returns(10);

            uploadFile
                .Setup(f => f.FileName)
                .Returns("testtemplate.xml");

            uploadFile
                .Setup(f => f.InputStream)
                .Returns(new System.IO.MemoryStream(Encoding.UTF8.GetBytes("test file")));

            te.uploadFile = uploadFile.Object;

            _guidGenerator.Setup(x => x.GenerateTimeBasedGuid()).Returns(templateIndex);
            var response = _sub.Create(te);

            template = _templateRepository.GetTemplate(templateIndex.ToString());

            System.Web.Mvc.FileStreamResult resp = (System.Web.Mvc.FileStreamResult) _sub.Open(templateIndex.ToString());
            
            Assert.AreEqual("application/msword", resp.ContentType);
            Assert.AreEqual(Path.GetFileName(template.FilePath), resp.FileDownloadName);
        }

        [Test]
        public void Create_Should_Create_Document()
        {
            Models.TemplateEdit te = new Models.TemplateEdit();
            te.Template.templateID = templateIndex.ToString();
            te.Template.templateName = "template name";
            te.Template.Discriminator = "Warrant";
            te.Template.addresseeRequired = true;
            Mock<HttpPostedFileBase> uploadFile = new Mock<HttpPostedFileBase>();
            uploadFile
            .Setup(f => f.ContentLength)
            .Returns(10);

            uploadFile
                .Setup(f => f.FileName)
                .Returns("testtemplate.xml");

            uploadFile
                .Setup(f => f.InputStream)
                .Returns(new System.IO.MemoryStream(Encoding.UTF8.GetBytes("<?xml version=\"1.0\" encoding=\"UTF - 8\" standalone=\"yes\"?><note><to>Tove</to></note>")));

            te.uploadFile = uploadFile.Object;

            _guidGenerator.Setup(x => x.GenerateTimeBasedGuid()).Returns(templateIndex);
            var response = _sub.Create(te);

            tr = new TipstaffRecord()
            {
                Id = trIndex.ToString(),
                Discriminator = "Warrant",
                CaseNumber = "Case number",
                CaseStatusId = 2,
                DivisionId = 3,
                DateCirculated = DateTime.Now,
                NextReviewDate = DateTime.Now.AddMonths(1),
                CreatedBy = "Carlos",
                CreatedOn = DateTime.Now,
                RespondentName = "Respondant name"
            };
            _tipstaffRecordRepository.Add(tr);
            System.Web.Mvc.FileContentResult resp = (System.Web.Mvc.FileContentResult)_subMain.Create(trIndex.ToString(), templateIndex.ToString());

            Assert.AreEqual("application/msword", resp.ContentType);
        }

        [TearDown]
        public void TearDown()
        {
            if (template != null) _templateRepository.Delete(template);
            if (tr != null) _tipstaffRecordRepository.Delete(tr);
        }
    }
}
