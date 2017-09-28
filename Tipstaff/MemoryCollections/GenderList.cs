using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tipstaff.MemoryCollections
{
    public class Gender
    {
        public int GenderId { get; set; }

        public string Detail { get; set; }

        public int Active { get; set; }
   }

    public class GenderList
    {
        public static List<Gender> GetGenderList()
        {
            return new List<Gender>()
            {
                new Gender() {  GenderId=1 ,  Detail = "Male",            Active = 1  },
                new Gender() {  GenderId=2 ,  Detail = "Female",          Active = 1  },
                new Gender() {  GenderId=3 ,  Detail = "N/A",             Active = 1 }
            };
        }

        public static Gender GetGenderByDetail(string d)
        {
            return GetGenderList().Where(x => x.Detail == d).FirstOrDefault();
        }
    }
}