﻿using System;
using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly IDynamoAPI<Address> _dynamoAPI;

        public AddressRepository(IDynamoAPI<Address> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }


        public void AddAddress(Address address)
        {
            _dynamoAPI.Save(address);
        }

        public void DeleteAddress(Address address)
        {
            _dynamoAPI.Delete(address);
        }

        public Address GetAddress(string id)
        {
            return _dynamoAPI.GetEntityByHashKey(id);
        }
        

        public void UpdateRepository(Address address)
        {
            var entity = _dynamoAPI.GetEntity(address.Id, address.TipstaffRecordID);

            entity.AddresseeName = address.AddresseeName;
            entity.AddressLine1 = address.AddressLine1;
            entity.AddressLine2 = address.AddressLine2;
            entity.AddressLine3 = address.AddressLine3;
            entity.County = address.County;
            entity.Phone = address.Phone;
            entity.PostCode = address.PostCode;
            entity.Town = address.Town;

            _dynamoAPI.Save(entity);
        }

        public IEnumerable<Address> GetAllByCondition<T>(string name, T value)
        {
            return _dynamoAPI.GetResultsByCondition(name, Amazon.DynamoDBv2.DocumentModel.ScanOperator.GreaterThan, value);
        }

        public Address GetAddressByIDAndRange(string id, string range)
        {
            return _dynamoAPI.GetEntity(id, range);
        }
    }
}
