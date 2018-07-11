using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using System.Web;
using Tipstaff.Controllers;
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
    public class DocumentsControllerTests:BaseController
    {
        private DocumentController _sub;
        private IS3API _s3Repository;
        private IDynamoAPI<Document> _dynamoAPI;
        public Mock<IGuidGenerator> _guidGenerator;
        Guid docIndex;
        private Mock<ICloudWatchLogger> _cloudWatchLogger = new Mock<ICloudWatchLogger>();
        Document doc;

        [SetUp]
        public void SetUp()
        {
            _dynamoAPI = new DynamoAPI<Document>();
            _docRepository = new DocumentsRepository(_dynamoAPI);
            _s3Repository = new S3API();
            _guidGenerator = new Mock<IGuidGenerator>();
            docIndex = Guid.NewGuid();
            _sub = new DocumentController(_cloudWatchLogger.Object, _s3Repository, 
                _docPresenter, _tipstaffRecordPresenter, _templatePresenter,
                _guidGenerator.Object);
        }

        [Test]
        public void Create_Should_Add_New_Document()
        {
            Models.DocumentUploadModel docUpload = new Models.DocumentUploadModel()
            {
                document = new Models.Document()
                {
                    createdBy = "integration test",
                    createdOn = DateTime.Now,
                    tipstaffRecordID = "1",
                    mimeType = "mime",
                    documentID = docIndex.ToString(),
                    country = MemoryCollections.CountryList.GetCountryByID(1),
                    nationality = MemoryCollections.NationalityList.GetNationalityByID(1),
                    documentReference = "Reference",
                    documentStatus = MemoryCollections.DocumentStatusList.GetDocumentStatusByID(1),
                    documentType = MemoryCollections.DocumentTypeList.GetDocumentTypeByID(1)
                 },
                 tipstaffRecordID = "1"
            };
            Mock<HttpPostedFileBase> uploadFile = new Mock<HttpPostedFileBase>();
            uploadFile
            .Setup(f => f.ContentLength)
            .Returns(10);

            uploadFile
                .Setup(f => f.FileName)
                .Returns("testdocument.txt");

            uploadFile
               .Setup(f => f.ContentType)
               .Returns("mime");

            uploadFile
                .Setup(f => f.InputStream)
                .Returns(new MemoryStream(Encoding.UTF8.GetBytes("test document")));

            docUpload.uploadFile = uploadFile.Object;

            _guidGenerator.Setup(x => x.GenerateTimeBasedGuid()).Returns(docIndex);
            var response = _sub.Upload(docUpload);

            doc = _docRepository.GetDocument(docIndex.ToString());
            Assert.AreEqual("mime", doc.MimeType);
            Assert.AreEqual("1", doc.TipstaffRecordID);
        }

        [Test]
        public void Create_Should_Extract_Document()
        {
            Models.DocumentUploadModel docUpload = new Models.DocumentUploadModel()
            {
                document = new Models.Document()
                {
                    createdBy = "integration test",
                    createdOn = DateTime.Now,
                    tipstaffRecordID = "1",
                    mimeType = "mime",
                    documentID = docIndex.ToString(),
                    country = MemoryCollections.CountryList.GetCountryByID(1),
                    nationality = MemoryCollections.NationalityList.GetNationalityByID(1),
                    documentReference = "Reference",
                    documentStatus = MemoryCollections.DocumentStatusList.GetDocumentStatusByID(1),
                    documentType = MemoryCollections.DocumentTypeList.GetDocumentTypeByID(1)
                },
                tipstaffRecordID = "1"
            };
            Mock<HttpPostedFileBase> uploadFile = new Mock<HttpPostedFileBase>();
            uploadFile
            .Setup(f => f.ContentLength)
            .Returns(10);

            uploadFile
                .Setup(f => f.FileName)
                .Returns("testdocument.txt");

            uploadFile
               .Setup(f => f.ContentType)
               .Returns("mime");

            uploadFile
                .Setup(f => f.InputStream)
                .Returns(new MemoryStream(Encoding.UTF8.GetBytes("test document")));

            docUpload.uploadFile = uploadFile.Object;

            _guidGenerator.Setup(x => x.GenerateTimeBasedGuid()).Returns(docIndex);
            var response = _sub.Upload(docUpload);

            doc = _docRepository.GetDocument(docIndex.ToString());

            System.Web.Mvc.FileStreamResult resp = (System.Web.Mvc.FileStreamResult)_sub.ExtractDocument(docIndex.ToString());

            Assert.AreEqual("application/msword", resp.ContentType);
            Assert.AreEqual(doc.FileName, resp.FileDownloadName);
        }
    }
}
