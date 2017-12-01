using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class AddressPresenter : IAddressPresenter, IMapper<Models.Address, Services.DynamoTables.Address>, IMapperCollections<Models.Address, Services.DynamoTables.Address>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ITipstaffRecordPresenter _tipstaffPresenter;

        public AddressPresenter(IAddressRepository addressRepository, ITipstaffRecordPresenter tipstaffPresenter)
        {
            _addressRepository = addressRepository;
            _tipstaffPresenter = tipstaffPresenter;
        }
        
        public void AddAddress(Models.Address model)
        {
            var address = GetDynamoTable(model);
            _addressRepository.AddAddress(address);
        }

        public Models.Address GetAddress(string id)
        {
            var address = _addressRepository.GetAddress(id);
            return GetModel(address);
        }

        public IEnumerable<Models.Address> GetAddressesByTipstaffRecordId(string id)
        {
            var records = _addressRepository.GetAllByCondition("TipstaffRecordID", id);
            return GetAll(records);
        }

        public Models.TipstaffRecord GetTipstaffRecord(string id)
        {
            var tipstaff = _tipstaffPresenter.GetTipStaffRecord(id);
            return tipstaff;
        }

        public void RemoveAddress(Models.Address address)
        {
            var add = GetDynamoTable(address);
            _addressRepository.DeleteAddress(add);
        }

        public void UpdateAddress(Models.Address address)
        {
            var add= GetDynamoTable(address);
            _addressRepository.UpdateRepository(add);
        }

        public Models.Address GetModel(Services.DynamoTables.Address table)
        {
            var add = new Models.Address()
            {
                addresseeName = table.AddresseeName,
                addressLine1 = table.AddressLine1,
                addressLine2 = table.AddressLine2,
                addressLine3 = table.AddressLine3,
                county = table.County,
                addressID = table.Id,
                phone = table.Phone,
                postcode = table.PostCode,
                tipstaffRecordID = table.TipstaffRecordID,
                town = table.Town,
                TipstaffRecord = GetTipstaffRecord(table.TipstaffRecordID)
            };

            return add;
        }

        public Services.DynamoTables.Address GetDynamoTable(Models.Address model)
        {
            var entity = new Services.DynamoTables.Address()
            {
                AddresseeName = model.addresseeName,
                AddressLine1 = model.addressLine1,
                AddressLine2 = model.addressLine2,
                AddressLine3 = model.addressLine3,
                County = model.county,
                Id = model.addressID,
                Phone = model.phone,
                PostCode = model.postcode,
                TipstaffRecordID = model.tipstaffRecordID.ToString(),
                Town = model.town
            };
            return entity;
        }

        public IEnumerable<Models.Address> GetAll(IEnumerable<Services.DynamoTables.Address> entities)
        {
            return entities.Select(x => GetModel(x));
        }

        public IEnumerable<Services.DynamoTables.Address> GetAll(IEnumerable<Models.Address> entities)
        {
            return entities.Select(x => GetDynamoTable(x));
        }
    }

    
}