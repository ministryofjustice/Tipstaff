using System;
using NUnit.Framework;
using Tipstaff.Services.Repositories;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Services.DynamoTables;
using TPLibrary.DynamoAPI;
using TPLibrary.GuidGenerator;

namespace Tipstaff.Tests.UnitTests
{
    [TestFixture]
    public class DocumentNunitTests
    {
        private IDocumentsRepository _docRepository;
        private IDynamoAPI<Document> _dynamoAPI;
        string docIndex = string.Empty;
        string tipstaffIndex = string.Empty;
        Document doc;

        [SetUp]
        public void SetUp()
        {

            _dynamoAPI = new DynamoAPI<Document>();
            _docRepository = new DocumentsRepository(_dynamoAPI);
            docIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
            tipstaffIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
        }

        [Test]
        public void Create_Should_Add_New_Document()
        {
            _docRepository.AddDocument(new Document()
            {
                Id = docIndex,
                TipstaffRecordID = tipstaffIndex,
                Country = "UK",
                CreatedBy = "John Doe",
                CreatedOn = DateTime.Now,
                DocumentReference = "12",
                DocumentStatus = "Open",
                DocumentType = "Warning",
                FileName = "test.docx",
                FilePath = "any path",
                MimeType = "word",
                Nationality = "British",
                TemplateID = "1234"
            });

            doc = _docRepository.GetDocumentByIdAndRange(docIndex, tipstaffIndex);

            Assert.AreEqual(tipstaffIndex, doc.TipstaffRecordID);
            Assert.AreEqual("word", doc.MimeType);
            Assert.AreEqual("Warning", doc.DocumentType);
            Assert.AreEqual("12", doc.DocumentReference);
        }

        [TearDown]
        public void TearDown()
        {
            _docRepository.DeleteDocument(doc);
        }
    }
}
