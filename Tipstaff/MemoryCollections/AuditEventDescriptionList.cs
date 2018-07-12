using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tipstaff.MemoryCollections
{
    public class AuditEventDescription
    {
        public int Id { get; set; }

        public string AuditDescription { get; set; }
    }

    public class AuditEventDescriptionList
    {
        public static List<AuditEventDescription> GetAuditEventDescriptionList()
        {
            return new List<AuditEventDescription>()
            {
                new AuditEventDescription() { Id = 1, AuditDescription = "FAQs" },
                new AuditEventDescription() { Id = 2, AuditDescription = "FAQ added" },
                new AuditEventDescription() { Id = 3, AuditDescription = "FAQ amended" },
                new AuditEventDescription() { Id = 4, AuditDescription = "FAQ deleted" },
                new AuditEventDescription() { Id = 5, AuditDescription = "Warrant Records" },
                new AuditEventDescription() { Id = 6, AuditDescription = "Warrant added" },
                new AuditEventDescription() { Id = 7, AuditDescription = "Warrant amended" },
                new AuditEventDescription() { Id = 8, AuditDescription = "Warrant deleted" },
            };
        }

        public static AuditEventDescription GetAuditEventDescriptionByDetail(string c)
        {
            return GetAuditEventDescriptionList().FirstOrDefault(x => x.AuditDescription == c);
        }

        public static AuditEventDescription GetAuditEventDescriptionByID(int id)
        {
            return GetAuditEventDescriptionList().FirstOrDefault(x => x.Id == id);
        }
    }
}