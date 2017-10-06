﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface IAddressRepository
    {
        Address GetAddress(string id);

        IEnumerable<Address> GetAddresses();

        void DeleteAddress(Address address);
    }
}
