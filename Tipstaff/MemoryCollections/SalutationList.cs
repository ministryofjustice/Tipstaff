using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tipstaff.MemoryCollections
{
    public class Salutation
    {
        public int SalutationId { get; set; }

        public string Detail { get; set; }

        public int Active { get; set; }
   }

    public class SalutationList
    {
        public static List<Salutation> GetSalutationList()
        {
            return new List<Salutation>()
            {
                new Salutation() {  SalutationId=1 ,  Detail = "Mr",            Active = 1  },
                new Salutation() {  SalutationId=2 ,  Detail = "Miss",          Active = 1  },
                new Salutation() {  SalutationId=3 ,  Detail = "Ms",            Active = 1  },
                new Salutation() {  SalutationId=4 ,  Detail = "Mrs",           Active = 1  },
                new Salutation() {  SalutationId=5 ,  Detail = "Madam",         Active = 1  },
                new Salutation() {  SalutationId=6 ,  Detail = "Sir",           Active = 1  },
                new Salutation() {  SalutationId=7 ,  Detail = "Force",         Active = 1  },
                new Salutation() {  SalutationId=8 ,  Detail = "L A",           Active = 1  },
                new Salutation() {  SalutationId=9 ,  Detail = "Heathrow",      Active = 1  },
                new Salutation() {  SalutationId=10 ,  Detail = "Gatwick",      Active = 1  },
                new Salutation() {  SalutationId=11 ,  Detail = "Manchester",   Active = 1  },
                new Salutation() {  SalutationId=12 ,  Detail = "Dover Port",   Active = 1  },
                new Salutation() {  SalutationId=13 ,  Detail = "Luton",        Active = 1  },
                new Salutation() {  SalutationId=14 ,  Detail = "Leeds",        Active = 1  },
                new Salutation() {  SalutationId=15 ,  Detail = "Stansted",     Active = 1  },
                new Salutation() {  SalutationId=16 ,  Detail = "Birmingham",   Active = 1  },
                new Salutation() {  SalutationId=17 ,  Detail = "DC",           Active = 1  },
                new Salutation() {  SalutationId=18 ,  Detail = "DS",           Active = 1  },
                new Salutation() {  SalutationId=19 ,  Detail = "Insp",         Active = 1  },
                new Salutation() {  SalutationId=20 ,  Detail = "PC",           Active = 1  },
                new Salutation() {  SalutationId=21 ,  Detail = "DCI",          Active = 1  },
                new Salutation() {  SalutationId=22 ,  Detail = "Newcastle",    Active = 1  },
            };
        }

        public static Salutation GetSalutationByID(int id)
        {
            return GetSalutationList().Where(x => x.SalutationId == id).FirstOrDefault();
        }
    }
}