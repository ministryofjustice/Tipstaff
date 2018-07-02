using Moq;
using NUnit.Framework;
using System;
using Tipstaff.Controllers;
using TPLibrary.GuidGenerator;
using System.Linq;
using Tipstaff.Models;
using System.Web;
using Tipstaff.Tests.Helpers;
using TPLibrary.Logger;

namespace Tipstaff.Tests.Integration
{
    [TestFixture]
    public class ChildAbductionArea : BaseController
    {
        private ChildAbductionController _sub;
        private ChildController _childController;
        public Mock<IGuidGenerator> _guidGenerator;
        private Guid _id;
        private Guid _record;
        private ChildAbduction _childAbduction;
        private ChildCreationModel _childCreationModel;
        private RespondentController _respondentController;
        private Mock<ICloudWatchLogger> _cloudWatchLogger = new Mock<ICloudWatchLogger>();

        [SetUp]
        public void Setup()
        {
            _guidGenerator = new Mock<IGuidGenerator>();
            _sub = new ChildAbductionController(_childAbductionPresenter, _tipstaffRecordPresenter);
            _childController = new ChildController(_childPresenter, _guidGenerator.Object, _childAbductionPresenter);
            _respondentController = new RespondentController(_cloudWatchLogger.Object, _respondentPresenter, _tipstaffRecordPresenter, _guidGenerator.Object);

            _id = Guid.NewGuid();
            _record = Guid.NewGuid();

            _childAbduction = new Tipstaff.Models.ChildAbduction()
            {
                orderDated = DateTime.Now,
                orderReceived = DateTime.Now,
                sentSCD26 = DateTime.Now,
                officerDealing = "Nick Wilson",
                caOrderType = MemoryCollections.CaOrderTypeList.GetOrderTypeList().First(),
                caseStatus = MemoryCollections.CaseStatusList.GetCaseStatusByID(1),
                protectiveMarking = MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().First(),
                
            };

            _childCreationModel = new ChildCreationModel()
            {
                tipstaffRecordID = _childAbduction.tipstaffRecordID,
                child = new Child()
                {
                    childID = _record.ToString(),
                    nameFirst = "Anna",
                    nameLast = "Wilson",
                    dateOfBirth = DateTime.Now.AddYears(-10),
                    gender = MemoryCollections.GenderList.GetGenderById(2),
                    height = "1.60cm",
                    build = "Medium",
                    hairColour = "blonde",
                    tipstaffRecordID = _childAbduction.tipstaffRecordID,
                    
                }
            };
        }

        [Test]
        public void Create_ChildAbducationCase_Save_New_Child()
        {
            _sub.Create(_childAbduction);

            _guidGenerator.Setup(x => x.GenerateTimeBasedGuid()).Returns(_record);
            
            _childController.Create(_childCreationModel, "Save and add new Child");

            //ASSERT
            var ca = _childAbductionPresenter.GetChildAbduction(_childAbduction.tipstaffRecordID);
            var child = _childPresenter.GetChild(_record.ToString());
            Assert.That(ca.children.Contains(child, new ChildComparer()));
        }

        [Test]
        public void Create_ChildAbducationCase_Save_Child_And_Add_New_Respondent()
        {
            var id = Guid.NewGuid();

            _sub.Create(_childAbduction);

            _guidGenerator.Setup(x => x.GenerateTimeBasedGuid()).Returns(id);

            _childController.Create(_childCreationModel, "Save and add new Child");

            var model = new RespondentCreationModel()
            {
                tipstaffRecordID = _childAbduction.tipstaffRecordID,
                respondent = new Respondent()
                {
                    nameFirst = "Kate",
                    nameLast = "Jackson",
                    build = "Medium",
                    country = MemoryCollections.CountryList.GetCountryList().First(),
                    eyeColour = "Brown",
                    dateOfBirth = DateTime.Now.AddYears(-10),
                    gender = MemoryCollections.GenderList.GetGenderList().First(),
                    nationality = MemoryCollections.NationalityList.GetNationalityList().First(),
                    height = "1.60",
                    riskOfDrugs = "High",
                    riskOfViolence = "High",
                    skinColour = MemoryCollections.SkinColourList.GetSkinColourList().First(),
                    specialfeatures = "Red Spot",
                    PNCID = "Test",
                    respondentID = id.ToString(),
                    tipstaffRecordID = _childAbduction.tipstaffRecordID,
                    childRelationship = MemoryCollections.ChildRelationshipList.GetChildRelationshipList().First(),
                    hairColour = "Black"
                 }
            };

            _respondentController.Create(model, "Save,add new Respondent");

            var respondent = _respondentPresenter.GetRespondentByKeys(_id.ToString(), _childAbduction.tipstaffRecordID);
            Assert.AreEqual(respondent.respondentID, id);
            Assert.AreEqual(respondent.tipstaffRecordID, _childAbduction.tipstaffRecordID);
        }
    }
}
