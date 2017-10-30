using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Controllers;
using Tipstaff.Infrastructure.Services;

namespace Tipstaff.Tests.Integration
{
    [TestFixture]
    public class ChildAbductionControllerTests : BaseController
    {
        private ChildAbductionController _sub;
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
            _sub = new ChildAbductionController(_childAbductionPresenter, _tipstaffRecordPresenter);
            _id = Guid.NewGuid();
            _record = Guid.NewGuid().ToString();
            _rangeKey = Guid.NewGuid().ToString();
        }

        [Test]
        public void Create_With_Model_Param_Should_Add_New_ChildAbduction_Entry()
        {

        }

        [Test]
        public void EnterResult_With_Model_Param_Should_Update_TippstaffRecord()
        {

        }

        public void Edit_With_Model_Param_Should_Update_ChildAbduction()
        {
        }
    }
}
