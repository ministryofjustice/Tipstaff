using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Models;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class AddressPresenter : IAddressPresenter
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ITipstaffRecordRepository _tipstaffRepository;

        public AddressPresenter(IAddressRepository addressRepository, ITipstaffRecordRepository tipstaffRepository)
        {
            _addressRepository = addressRepository;
            _tipstaffRepository = tipstaffRepository;
        }
        
        public void AddAddress(Address addressMdl)
        {
            var address = GetDTAddress(addressMdl);

            _addressRepository.AddAddress(address);
        }

        public Address GetAddress(string id)
        {
            var address = _addressRepository.GetAddress(id);
            var addressMdl = GetMdlAddress(address);
            return addressMdl;
        }

        public IEnumerable<Address> GetAddressByTipstaffRecordId(string id)
        {
            var records = _addressRepository.GetAddresses().Where(x => x.TipstaffRecordId == id);

            var addresses = GetAddresses(records);

            return addresses;
        }

        public TipstaffRecord GetTipstaffRecord(string id)
        {
            var record = _tipstaffRepository.GetEntityByHashKey(id);
            return GetMdlTipstaffRecord(record);
        }

        public void RemoveAddress(Address address)
        {
            var add = GetDTAddress(address);
            _addressRepository.DeleteAddress(add);
        }

        public void UpdateAddress(Address address)
        {
            var add= GetDTAddress(address);
            _addressRepository.UpdateRepository(add);
        }

        private Tipstaff.Services.DynamoTables.Address GetDTAddress(Address addressModel)
        {
            return new Services.DynamoTables.Address()
            {
                AddresseeName = addressModel.addresseeName,
                AddressLine1 = addressModel.addressLine1,
                AddressLine2 = addressModel.addressLine2,
                AddressLine3 = addressModel.addressLine3,
                County = addressModel.county,
                Id = addressModel.addressID.ToString(),
                Phone = addressModel.phone,
                PostCode = addressModel.postcode,
                TipstaffRecordId = addressModel.tipstaffRecordID.ToString(),
                Town = addressModel.town
            };
        }

        private Address GetMdlAddress(Services.DynamoTables.Address address)
        {
            var tipstaffRecord = _tipstaffRepository.GetEntityByHashKey(address.TipstaffRecordId);

            var add = new Address()
            {
                addresseeName = address.AddresseeName,
                addressLine1 = address.AddressLine1,
                addressLine2 = address.AddressLine2,
                addressLine3 = address.AddressLine3,
                county = address.County,
                addressID = int.Parse(address.Id),
                phone = address.Phone,
                postcode = address.PostCode,
                tipstaffRecordID = int.Parse(address.TipstaffRecordId),
                town = address.Town,
                TipstaffRecord = new TipstaffRecord()
                {
                    prisonCount = tipstaffRecord.PrisonCount,
                    arrestCount = tipstaffRecord.ArrestCount,
                    tipstaffRecordID = tipstaffRecord.Id,
                    createdBy = tipstaffRecord.CreatedBy,
                    createdOn = tipstaffRecord.CreatedOn,
                    Descriminator = tipstaffRecord.Discriminator,
                    DateExecuted = tipstaffRecord.DateExecuted,
                    nextReviewDate = tipstaffRecord.NextReviewDate,
                    NPO = tipstaffRecord.NPO,
                    resultDate = tipstaffRecord.ResultDate,
                    resultEnteredBy = tipstaffRecord.ResultEnteredBy,
                    caseStatusID = MemoryCollections.CaseStatusList.GetCaseStatusList().FirstOrDefault(x=>x.Detail == tipstaffRecord.CaseStatus).CaseStatusId,
                    caseStatus = MemoryCollections.CaseStatusList.GetCaseStatusList().FirstOrDefault(x => x.Detail == tipstaffRecord.CaseStatus),
                    protectiveMarkingID = MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().FirstOrDefault(x => x.Detail == tipstaffRecord.CaseStatus).ProtectiveMarkingId, 
                    protectiveMarking = MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().FirstOrDefault(x => x.Detail == tipstaffRecord.CaseStatus),
                    resultID = MemoryCollections.ResultsList.GetResultList().FirstOrDefault(x => x.Detail == tipstaffRecord.Result).ResultId,
                    result = MemoryCollections.ResultsList.GetResultList().FirstOrDefault(x => x.Detail == tipstaffRecord.Result),
                    
                 }
            };

            return add;
        }

        private IEnumerable<Address> GetAddresses(IEnumerable<Services.DynamoTables.Address> addresses)
        {
            return addresses.Select(x => new Address()
            {
                addresseeName = x.AddresseeName,
                addressLine1 = x.AddressLine1,
                addressLine2 = x.AddressLine2,
                addressLine3 = x.AddressLine3,
                county = x.County,
                addressID = int.Parse(x.Id),
                phone = x.Phone,
                postcode = x.PostCode,
                tipstaffRecordID = int.Parse(x.TipstaffRecordId),
                town = x.Town,
                
            });
        }

        private TipstaffRecord GetMdlTipstaffRecord(Tipstaff.Services.DynamoTables.TipstaffRecord rec)
        {
            var addressList = _addressRepository.GetAddresses();

            var record = new TipstaffRecord()
            {
                Descriminator = rec.Discriminator,
                arrestCount = rec.ArrestCount,
                DateExecuted = rec.DateExecuted,
                NPO = rec.NPO,
                createdBy = rec.CreatedBy,
                createdOn = rec.CreatedOn,
                nextReviewDate = rec.NextReviewDate,
                prisonCount = rec.PrisonCount,
                result = MemoryCollections.ResultsList.GetResultByDetail(rec.Result),
                caseStatus = MemoryCollections.CaseStatusList.GetCaseStatusByDetail(rec.CaseStatus),
                addresses = GetAddresses(addressList) as ICollection<Address>,
                resultDate = rec.ResultDate,
                resultEnteredBy = rec.ResultEnteredBy,
                protectiveMarking = MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingByDetail(rec.ProtectiveMarking),
                tipstaffRecordID = rec.Id,
            };

            return record;
        }
    }

    
}