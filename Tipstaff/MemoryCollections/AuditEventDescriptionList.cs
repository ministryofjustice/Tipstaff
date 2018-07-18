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
                new AuditEventDescription() { Id = 5, AuditDescription = "ChildAbduction Records" },
                new AuditEventDescription() { Id = 6, AuditDescription = "ChildAbduction added" },
                new AuditEventDescription() { Id = 7, AuditDescription = "ChildAbduction amended" },
                new AuditEventDescription() { Id = 8, AuditDescription = "ChildAbduction deleted" },
                new AuditEventDescription() { Id = 9, AuditDescription = "Police Forces Records" },
                new AuditEventDescription() { Id = 10, AuditDescription = "Police Forces added" },
                new AuditEventDescription() { Id = 11, AuditDescription = "Police Forces amended" },
                new AuditEventDescription() { Id = 12, AuditDescription = "Police Forces deleted" },
                new AuditEventDescription() { Id = 13, AuditDescription = "Solicitor Firm Records" },
                new AuditEventDescription() { Id = 14, AuditDescription = "Solicitor Firm added" },
                new AuditEventDescription() { Id = 15, AuditDescription = "Solicitor Firm amended" },
                new AuditEventDescription() { Id = 16, AuditDescription = "Solicitor Firm deleted" },
                new AuditEventDescription() { Id = 17, AuditDescription = "Solicitor Records" },
                new AuditEventDescription() { Id = 18, AuditDescription = "Solicitor added" },
                new AuditEventDescription() { Id = 19, AuditDescription = "Solicitor amended" },
                new AuditEventDescription() { Id = 20, AuditDescription = "Solicitor deleted" },
                new AuditEventDescription() { Id = 21, AuditDescription = "Templates Records" },
                new AuditEventDescription() { Id = 22, AuditDescription = "Templates added" },
                new AuditEventDescription() { Id = 23, AuditDescription = "Templates amended" },
                new AuditEventDescription() { Id = 24, AuditDescription = "Templates deleted" }
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