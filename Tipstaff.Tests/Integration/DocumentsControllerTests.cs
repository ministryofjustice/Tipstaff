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


    }

    
}
