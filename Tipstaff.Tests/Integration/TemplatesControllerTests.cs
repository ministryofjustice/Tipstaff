using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Areas.Admin.Controllers;
using Tipstaff.Controllers;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Infrastructure.S3API;
using Tipstaff.Infrastructure.Services;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Tests.Integration
{

    [TestFixture]
    public class TemplatesControllerTests
    {
        private TemplatesController _sub;
        private ITemplateRepository _templateRepository;
        private IS3API _s3Repository;
        private IDynamoAPI<Tipstaff.Services.DynamoTables.Template> _dynamoAPI;
        string templateIndex = string.Empty;
        Template template;

        [SetUp]
        public void SetUp()
        {

            _dynamoAPI = new DynamoAPI<Tipstaff.Services.DynamoTables.Template>();
            _templateRepository = new TemplateRepository(_dynamoAPI);
            _s3Repository = new S3API();
            templateIndex = GuidGenerator.GenerateTimeBasedGuid().ToString();
            _sub = new TemplatesController(_templateRepository, _s3Repository);
        }

        [Test]
        public void Create_Should_Add_New_Template()
        {
            Models.TemplateEdit te = new Models.TemplateEdit();
            te.Template.templateID = templateIndex;
            te.Template.templateName = "template name";
            var response = _sub.Create(te); //this will fail as there is no file to be uploaded

            template = _templateRepository.GetTemplate(templateIndex);

            Assert.AreEqual(templateIndex, template.templateID);
        }

        [Test]
        public void Update_Should_Update_Template()
        {
            Models.TemplateEdit te = new Models.TemplateEdit();
            te.Template.templateID = templateIndex;
            te.Template.templateName = "template name";
            var response = _sub.Create(te); //this will fail as there is no file to be uploaded

            te = new Models.TemplateEdit();
            te.Template.templateID = templateIndex;
            te.Template.templateName = "template name modified";
            

            response = _sub.Edit(te);

            template = _templateRepository.GetTemplate(templateIndex);

            Assert.AreEqual("template name modified", template.templateName);

        }

        [TearDown]
        public void TearDown()
        {
            _templateRepository.Delete(template);
        }
    }
}
