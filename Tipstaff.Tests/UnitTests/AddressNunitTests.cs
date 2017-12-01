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
    public class AddressNunitTests
    {
        private IAddressRepository _addressRepository;
        private ITipstaffRecordRepository _tipstaffRepository;
        private IDynamoAPI<Address> _dynamoAPI;
        private IDynamoAPI<TipstaffRecord> _tipstaffDynamoAPI;
        string addressIndex = string.Empty;
        string tipstaffIndex = string.Empty;
        Address address;
        TipstaffRecord tr;

        [SetUp]
        public void SetUp()
        {

            _dynamoAPI = new DynamoAPI<Address>();
            _tipstaffDynamoAPI = new DynamoAPI<TipstaffRecord>();
            _addressRepository = new AddressRepository(_dynamoAPI);
            _tipstaffRepository = new TipstaffRecordRepository(_tipstaffDynamoAPI);
            addressIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
            tipstaffIndex = new GuidGenerator().GenerateTimeBasedGuid().ToString();
        }

        [Test]
        public void Create_Should_Add_New_Address()
        {
            _tipstaffRepository.Add(new TipstaffRecord()
            {
                Id = tipstaffIndex
            });

            _addressRepository.AddAddress(new Address() {
                Id = addressIndex,
                //TipstaffRecordId = tipstaffIndex,
                AddresseeName = "Addressee",
                AddressLine1 = "Line 1",
                AddressLine2 = "Line 2",
                AddressLine3 = "Line 3",
                County = "County name",
                Phone = "any phone",
                PostCode = "postcode London",
                Town = "London"

            });

            address = _addressRepository.GetAddress(addressIndex);

            //Assert.AreEqual(tipstaffIndex, address.TipstaffRecordId);
            Assert.AreEqual("Addressee", address.AddresseeName);
            Assert.AreEqual("Line 1", address.AddressLine1);
            Assert.AreEqual("Line 2", address.AddressLine2);
            Assert.AreEqual("Line 3", address.AddressLine3);
            Assert.AreEqual("County name", address.County);
            Assert.AreEqual("any phone", address.Phone);
            Assert.AreEqual("postcode London", address.PostCode);
            Assert.AreEqual("London", address.Town);

        }

        [Test]
        public void Update_Should_Update_Address()
        {
            _tipstaffRepository.Add(new TipstaffRecord()
            {
                Id = tipstaffIndex
            });
            _addressRepository.AddAddress(new Address()
            {
                Id = addressIndex,
                //TipstaffRecordId = tipstaffIndex,
                AddresseeName = "Addressee",
                AddressLine1 = "Line 1",
                AddressLine2 = "Line 2",
                AddressLine3 = "Line 3",
                County = "County name",
                Phone = "any phone",
                PostCode = "postcode London",
                Town = "London"

            });

            _addressRepository.UpdateRepository(new Address()
            {
                Id = addressIndex,
                //TipstaffRecordId = tipstaffIndex,
                AddresseeName = "Addressee modified",
                AddressLine1 = "Line 1 modified",
                AddressLine2 = "Line 2 modified",
                AddressLine3 = "Line 3 modified",
                County = "County name modified",
                Phone = "any phon modifiede",
                PostCode = "postcode London modified",
                Town = "London modified"

            });

            address = _addressRepository.GetAddress(addressIndex);
            
            //Assert.AreEqual(tipstaffIndex, address.TipstaffRecordId);
            Assert.AreEqual("Addressee modified", address.AddresseeName);
            Assert.AreEqual("Line 1 modified", address.AddressLine1);
            Assert.AreNotEqual("Line 2", address.AddressLine2);
            Assert.AreEqual("Line 3 modified", address.AddressLine3);
            Assert.AreEqual("County name modified", address.County);
            Assert.AreNotEqual("any phone", address.Phone);
            Assert.AreEqual("postcode London modified", address.PostCode);
            Assert.AreNotEqual("London", address.Town);

        }

        [TearDown]
        public void TearDown()
        {
            _addressRepository.DeleteAddress(address);
            _tipstaffRepository.Delete(tr);
        }
    }
}
