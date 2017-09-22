using NUnit.Framework;
using Tipstaff.Areas.Admin.Controllers;
using Tipstaff.Controllers;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Infrastructure.Repositories;
using Tipstaff.Infrastructure.Services;
using Tipstaff.Logger;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Tests.Integration
{
    [TestFixture]
    public class ApplicantControllerTests
    {
        private ApplicantController _sub;
        private IApplicantRepository _applicantRepository;
        private ICloudWatchLogger _logger;
        private ITipstaffRecordRepository _tipstaffRepository;
        private IDynamoAPI<Tipstaff.Services.DynamoTables.Applicant> _dynamoAPI;
        private IDynamoAPI<Tipstaff.Services.DynamoTables.TipstaffRecord> _dynamoAPITipstaff;
        string applicantIndex = string.Empty;
        string tipstaffRecordIndex = string.Empty;
        Applicant applicant;

        [SetUp]
        public void SetUp()
        {

            _dynamoAPI = new DynamoAPI<Tipstaff.Services.DynamoTables.Applicant>();
            _applicantRepository = new ApplicantRepository(_dynamoAPI);
            _tipstaffRepository = new TipstaffRecordRepository(_dynamoAPITipstaff);
            applicantIndex = GuidGenerator.GenerateTimeBasedGuid().ToString();
            tipstaffRecordIndex = GuidGenerator.GenerateTimeBasedGuid().ToString();
            _sub = new ApplicantController(_logger, _applicantRepository, _tipstaffRepository);
        }

        [Test]
        public void Create_Should_Add_New_Applicant()
        {
            Models.Applicant a = new Models.Applicant()
            {
                ApplicantID = applicantIndex,
                nameLast = "Last Name",
                nameFirst = "First Name",
                addressLine1 = "Address Line 1",
                addressLine2 = "Address Line 2",
                addressLine3 = "Address Line 3",
                town = "Town",
                county = "County",
                postcode = "Postcode",
                phone = "Phone",
                salutation = new Models.Salutation() { salutationID = 1, active = true, Detail = "Mr" },
                salutationID = 1,
                childAbduction = new Models.ChildAbduction(),
                tipstaffRecordID = tipstaffRecordIndex
            };
            Models.ApplicantCreationModel acm = new Models.ApplicantCreationModel()
            {
                applicant = a,
                tipstaffRecordID = tipstaffRecordIndex
            };

            var response = _sub.Create(acm);
            applicant = _applicantRepository.GetApplicant(applicantIndex);

            Assert.AreEqual(applicantIndex, applicant.ApplicantID);
        }

        [Test]
        public void Update_Should_Update_Applicant()
        {
            var response = _sub.Create(new Models.ApplicantCreationModel()
            {
                applicant = new Models.Applicant()
                {
                    ApplicantID = applicantIndex,
                    nameLast = "Last Name",
                    nameFirst = "First Name",
                    addressLine1 = "Address Line 1",
                    addressLine2 = "Address Line 2",
                    addressLine3 = "Address Line 3",
                    town = "Town",
                    county = "County",
                    postcode = "Postcode",
                    phone = "Phone",
                    salutation = new Models.Salutation() { salutationID = 1, active = true, Detail = "Mr" },
                    tipstaffRecordID = tipstaffRecordIndex
                },
                tipstaffRecordID = tipstaffRecordIndex
            });

            response = _sub.Edit(new Models.ApplicantEditModel()
            {
                applicant = new Models.Applicant()
                {
                    ApplicantID = applicantIndex,
                    nameLast = "Last Name modified",
                    nameFirst = "First Name modified",
                    addressLine1 = "Address Line 1",
                    addressLine2 = "Address Line 2 modified",
                    addressLine3 = "Address Line 3",
                    town = "Town",
                    county = "County",
                    postcode = "Postcode",
                    phone = "Phone",
                    salutation = new Models.Salutation() { salutationID = 1, active = true, Detail = "Mr" },
                    tipstaffRecordID = tipstaffRecordIndex
                }
            });

            applicant = _applicantRepository.GetApplicant(applicantIndex);

            Assert.AreEqual("Address Line 2 modified", applicant.AddressLine2);

        }

        [TearDown]
        public void TearDown()
        {
            _applicantRepository.Delete(applicant);
        }
    }
}
