using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Tipstaff.Services.Repositories;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Infrastructure.Services;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Tests.UnitTests
{
    [TestFixture]
    public class TemplatesNunitTests
    {
        private ITemplateRepository _templateRepository;
        private IDynamoAPI<Template> _dynamoAPI;
        string templateIndex = string.Empty;
        Template template;

        [SetUp]
        public void SetUp()
        {

            _dynamoAPI = new DynamoAPI<Template>();
            _templateRepository = new TemplateRepository(_dynamoAPI);
            templateIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
        }

        [Test]
        public void Create_Should_Add_New_Template()
        {
            _templateRepository.AddTemplate(new Template() {
                Id = templateIndex,
                TemplateName = "Template name",
                Discriminator = "Warrant",
                AddresseeRequired = true,
                Active = true
            });

            template = _templateRepository.GetTemplate(templateIndex);

            Assert.AreEqual("Template name", template.TemplateName);
            Assert.AreEqual(true, template.AddresseeRequired);
            Assert.AreEqual(true, template.Active);
        }

        [Test]
        public void Update_Should_Update_Template()
        {
            _templateRepository.AddTemplate(new Template()
            {
                Id = templateIndex,
                TemplateName = "Template name",
                Discriminator = "Warrant",
                AddresseeRequired = true,
                Active = true
            });

            _templateRepository.Update(new Template()
            {
                Id = templateIndex,
                TemplateName = "Template name modified",
                Discriminator = "Warrant modified",
                AddresseeRequired = false,
                Active = true
            });

            template = _templateRepository.GetTemplate(templateIndex);

            Assert.AreNotEqual("Template name", template.TemplateName);
            Assert.AreEqual("Warrant modified", template.Discriminator);
            Assert.AreNotEqual(true, template.AddresseeRequired);
            Assert.AreEqual(true, template.Active);
        }

        [TearDown]
        public void TearDown()
        {
            _templateRepository.Delete(template);
        }
    }
}
