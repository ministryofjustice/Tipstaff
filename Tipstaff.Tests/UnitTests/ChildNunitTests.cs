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
    public class ChildNunitTests
    {
        private IChildRepository _childRepository;
        private ITipstaffRecordRepository _tipstaffRepository;
        private IDynamoAPI<Child> _dynamoAPI;
        private IDynamoAPI<TipstaffRecord> _tipstaffDynamoAPI;
        string childIndex = string.Empty;
        string tipstaffIndex = string.Empty;
        Child child;
        TipstaffRecord tr;

        [SetUp]
        public void SetUp()
        {

            _dynamoAPI = new DynamoAPI<Child>();
            _tipstaffDynamoAPI = new DynamoAPI<TipstaffRecord>();
            _childRepository = new ChildRepository(_dynamoAPI);
            _tipstaffRepository = new TipstaffRecordRepository(_tipstaffDynamoAPI);
            childIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
            tipstaffIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
        }

        [Test]
        public void Create_Should_Add_New_Child()
        {
            _tipstaffRepository.Add(new TipstaffRecord()
            {
                Id = tipstaffIndex
            });

            _childRepository.AddChild(new Child() {
                Id = childIndex,
                TipstaffRecordID = tipstaffIndex,
                Build = "average",
                Country = "Spain",
                DateOfBirth = DateTime.Now.AddYears(-10),
                EyeColour = "Brown",
                Gender = "Male",
                HairColour="Black",
                Height="150",
                NameFirst="Juanito",
                NameLast="Nadie",
                NameMiddle="",
                Nationality="Spanish",
                PNCID="12345",
                SkinColour="white",
                Specialfeatures="none"
            });

            child = _childRepository.GetChild(childIndex);

            Assert.AreEqual(tipstaffIndex, child.TipstaffRecordID);
            Assert.AreEqual("Nadie", child.NameLast);
            Assert.AreEqual("12345", child.PNCID);
            Assert.AreEqual("none", child.Specialfeatures);


        }

        [Test]
        public void Update_Should_Update_Child()
        {
            _tipstaffRepository.Add(new TipstaffRecord()
            {
                Id = tipstaffIndex
            });

            _childRepository.AddChild(new Child()
            {
                Id = childIndex,
                TipstaffRecordID = tipstaffIndex,
                Build = "average",
                Country = "Spain",
                DateOfBirth = DateTime.Now.AddYears(-10),
                EyeColour = "Brown",
                Gender = "Male",
                HairColour = "Black",
                Height = "150",
                NameFirst = "Juanito",
                NameLast = "Nadie",
                NameMiddle = "",
                Nationality = "Spanish",
                PNCID = "12345",
                SkinColour = "white",
                Specialfeatures = "none"
            });

            _childRepository.Update(new Child()
            {
                Id = childIndex,
                TipstaffRecordID = tipstaffIndex,
                Build = "average modified",
                Country = "Spain",
                DateOfBirth = DateTime.Now.AddYears(-10),
                EyeColour = "Brown",
                Gender = "Male",
                HairColour = "Black",
                Height = "150",
                NameFirst = "Juanito",
                NameLast = "Nadie modified",
                NameMiddle = "",
                Nationality = "Spanish",
                PNCID = "12345 modified",
                SkinColour = "white",
                Specialfeatures = "none"
            });

            child = _childRepository.GetChild(childIndex);

            Assert.AreEqual(tipstaffIndex, child.TipstaffRecordID);
            Assert.AreEqual("Nadie modified", child.NameLast);
            Assert.AreNotEqual("12345", child.PNCID);
            Assert.AreNotEqual("none", child.Specialfeatures);

        }

        [TearDown]
        public void TearDown()
        {
            _childRepository.Delete(child);
            _tipstaffRepository.Delete(tr);
        }
    }
}
