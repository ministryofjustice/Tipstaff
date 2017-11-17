﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.MemoryCollections;
using Tipstaff.Models;

namespace Tipstaff.Presenters.Interfaces
{
    public interface IUsersPresenter
    {
        IEnumerable<User> GetAllUsers();

        void Update(User user);

        void Add(User user);

        User GetUserByID(string id);

        IEnumerable<Role> GetAllRoles();
    }
}