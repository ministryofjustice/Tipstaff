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
        private ITipstaffRecordRepository _tipstaffRepository;
        private IDynamoAPI<Document> _dynamoAPI;
        private IDynamoAPI<TipstaffRecord> _tipstaffDynamoAPI;
        string docIndex = string.Empty;
        string tipstaffIndex = string.Empty;
        Document doc;
        TipstaffRecord tr;

        [SetUp]
        public void SetUp()
        {

            _dynamoAPI = new DynamoAPI<Document>();
            _tipstaffDynamoAPI = new DynamoAPI<TipstaffRecord>();
            _docRepository = new DocumentsRepository(_dynamoAPI);
            _tipstaffRepository = new TipstaffRecordRepository(_tipstaffDynamoAPI);
            docIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
            tipstaffIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
        }

        //[Test]
        //public void Create_Should_Add_New_Document()
        //{
        //    _tipstaffRepository.Add(new TipstaffRecord()
        //    {
        //        Id = tipstaffIndex
        //    });

        //    _docRepository.AddDocument(new Document() {
        //        Id = docIndex,
        //        TipstaffRecordID = tipstaffIndex,
        //        Country="UK",
        //        CreatedBy="John Doe",
        //        CreatedOn=DateTime.Now,
        //        DocumentReference="12",
        //        DocumentStatus="Open",
        //        DocumentType="Warning",
        //        FileName="test.docx",
        //        FilePath="any path",
        //        MimeType="word",
        //        Nationality="British",
        //        TemplateID="1234"
        //    });

        //    doc = _docRepository.GetDocument(docIndex);

        //    Assert.AreEqual(tipstaffIndex, doc.TipstaffRecordID);
        //    Assert.AreEqual("word", doc.MimeType);
        //    Assert.AreEqual("Warning", doc.DocumentType);
        //    Assert.AreEqual("12", doc.DocumentReference);
        //}

        [TearDown]
        public void TearDown()
        {
            _docRepository.DeleteDocument(doc);
            _tipstaffRepository.Delete(tr);
        }
    }
}
