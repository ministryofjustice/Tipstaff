using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tipstaff.MemoryCollections
{
    public class ProtectiveMarkings
    {
        public int ProtectiveMarkingId { get; set; }

        public string Detail { get; set; }

        public int Active { get; set; }
    }

    public class ProtectiveMarkingsList
    {
        public static List<ProtectiveMarkings> GetProtectiveMarkingsList()
        {
            return new List<ProtectiveMarkings>()
            {
                new ProtectiveMarkings() {  ProtectiveMarkingId = 1 ,  Detail = "Confidential",  Active = 1  },
                new ProtectiveMarkings() {  ProtectiveMarkingId = 2 ,  Detail = "Restricted",    Active = 1  },
                new ProtectiveMarkings() {  ProtectiveMarkingId = 3 ,  Detail = "Protect",       Active = 1  },
                new ProtectiveMarkings() {  ProtectiveMarkingId = 4 ,  Detail = "Unclassified",  Active = 1  }
            };
        }
    }
}