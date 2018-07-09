using Moq;
using NUnit.Framework;
using System;
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
        private IS3API _s3Repository;
        private IDynamoAPI<Template> _dynamoAPI;
        public Mock<IGuidGenerator> _guidGenerator;
        Guid templateIndex;
        private Mock<ICloudWatchLogger> _cloudWatchLogger = new Mock<ICloudWatchLogger>();
        Template template;


        [SetUp]
        public void SetUp()
        {

            _dynamoAPI = new DynamoAPI<Template>();
            _templateRepository = new TemplateRepository(_dynamoAPI);
            _s3Repository = new S3API();
            _guidGenerator = new Mock<IGuidGenerator>();
            templateIndex = Guid.NewGuid();
            _sub = new TemplatesController(_templatePresenter, _s3Repository, _guidGenerator.Object, _cloudWatchLogger.Object);
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
                .Returns(new System.IO.MemoryStream(Encoding.UTF8.GetBytes("test file")));

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

        [TearDown]
        public void TearDown()
        {
            _templateRepository.Delete(template);
        }
    }
}
