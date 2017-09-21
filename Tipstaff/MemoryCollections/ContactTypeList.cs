using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tipstaff.MemoryCollections
{
    public class ContactType
    {
        public int ContactTypeId { get; set; }

        public string Detail { get; set; }

        public int Active { get; set; }
    }

    public class ContactTypeList
    {
        public static List<ContactType> GetContactTypeList()
        {
            return new List<ContactType>()
            {
                new ContactType() {  ContactTypeId=1 ,   Detail = "Judicial",               Active = 1   },
                new ContactType() {  ContactTypeId=2 ,   Detail = "Judges Clerk",           Active = 1   },
                new ContactType() {  ContactTypeId=3 ,   Detail = "Solicitor",              Active = 1   },
                new ContactType() {  ContactTypeId=4 ,   Detail = "Met Police",             Active = 1   },
                new ContactType() {  ContactTypeId=5 ,   Detail = "Police",                 Active = 1   },
                new ContactType() {  ContactTypeId=6 ,   Detail = "Government Department",  Active = 1  },
                new ContactType() {  ContactTypeId=7 ,   Detail = "Other",                  Active = 1   },
                new ContactType() {  ContactTypeId=8 ,   Detail = "NPO, SB & Airports",     Active = 1   },
                new ContactType() {  ContactTypeId=9 ,   Detail = "Official Receiver",      Active = 1   },
            };
        }
    }
}