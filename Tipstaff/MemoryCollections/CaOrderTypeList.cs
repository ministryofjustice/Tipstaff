using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tipstaff.MemoryCollections
{
    public class CaOrderType
    {
        public int OrderTypeId { get; set; }

        public string Detail { get; set; }

        public int Active { get; set; }
    }

    public class CaOrderTypeList
    {
        public static List<CaOrderType> GetOrderTypeList()
        {
            return new List<CaOrderType>()
            {
                new CaOrderType() {  OrderTypeId=1 ,  Detail = "Collection",  Active = 1  },
                new CaOrderType() {  OrderTypeId=2 ,  Detail = "Location",  Active = 1  },
                new CaOrderType() {  OrderTypeId=3 ,  Detail = "Passport Seizure",  Active = 1  },
                new CaOrderType() {  OrderTypeId=4 ,  Detail = "Port Alert Only",  Active = 1  }
            };
        }
    }
}