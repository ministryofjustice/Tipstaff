using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tipstaff.MemoryCollections
{
    public class Role
    {
        public int RoleId { get; set; }

        public string Detail { get; set; }

        public int Strength { get; set; }
   }

    public class RolesList
    {
        public static List<Role> GetRolesList()
        {
            return new List<Role>()
            {
                new Role() {  RoleId=1 ,  Detail = "Deactivated",   Strength = 0  },
                new Role() {  RoleId=2 ,  Detail = "User",          Strength = 25  },
                new Role() {  RoleId=3 ,  Detail = "Admin",         Strength = 75  },
                new Role() {  RoleId=4 ,  Detail = "System Admin",  Strength = 100  }
            };
        }

        public static Role GetRoleByDetail(string d)
        {
            return GetRolesList().Where(x => x.Detail == d).FirstOrDefault();
        }

        public static Role GetRoleByStrength(int d)
        {
            return GetRolesList().Where(x => x.Strength == d).FirstOrDefault();
        }
    }
}