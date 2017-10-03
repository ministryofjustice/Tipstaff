using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tipstaff.MemoryCollections
{
    public class SkinColour
    {
        public int SkinColourId { get; set; }

        public string Detail { get; set; }

        public int Active { get; set; }
   }

    public class SkinColourList
    {
        public static List<SkinColour> GetSkinColourList()
        {
            return new List<SkinColour>()
            {
                new SkinColour() {  SkinColourId=-1 ,  Detail = "Not Supplied",                                 Active = 1  },
                new SkinColour() {  SkinColourId=0 ,  Detail = "IC0 - Unknown",                                 Active = 1  },
                new SkinColour() {  SkinColourId=1 ,  Detail = "IC1 - White North - European",                  Active = 1  },
                new SkinColour() {  SkinColourId=2 ,  Detail = "IC2 - White South - European",                  Active = 1  },
                new SkinColour() {  SkinColourId=3 ,  Detail = "IC3 - Black",                                   Active = 1 },
                new SkinColour() {  SkinColourId=4 ,  Detail = "IC4 - Asian",                                   Active = 1 },
                new SkinColour() {  SkinColourId=5 ,  Detail = "IC5 - Chinese, Japanese or South East Asian",   Active = 1 },
                new SkinColour() {  SkinColourId=6 ,  Detail = "IC6 - Middle Eastern",                          Active = 1 }
            };
        }

        public static SkinColour GetSkinColourByDetail(string d)
        {
            return GetSkinColourList().Where(x => x.Detail == d).FirstOrDefault();
        }
    }
}