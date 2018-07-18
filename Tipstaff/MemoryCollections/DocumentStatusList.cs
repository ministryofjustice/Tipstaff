using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tipstaff.MemoryCollections
{
    public class DocumentStatus
    {
        public int DocumentStatusID { get; set; }

        public string Detail { get; set; }

        public int Active { get; set; }
   }

    public class DocumentStatusList
    {
        public static List<DocumentStatus> GetDocumentStatusList()
        {
            return new List<DocumentStatus>()
            {
                new DocumentStatus() { DocumentStatusID=1, Detail="Generated", Active=1},
                new DocumentStatus() { DocumentStatusID=2, Detail="Stored in Tipstaff's Safe", Active=1},
                new DocumentStatus() { DocumentStatusID=3, Detail="Disposed of", Active=1},
                new DocumentStatus() { DocumentStatusID=4, Detail="Returned to Owner", Active=1},
                new DocumentStatus() { DocumentStatusID=5, Detail="Stored on shared drive", Active=1},
                new DocumentStatus() { DocumentStatusID=6, Detail="E-Mail Police Force Reference Number", Active=0}
            };
        }

        public static DocumentStatus GetDocumentStatusByDetail(string c)
        {
            return GetDocumentStatusList().FirstOrDefault(x => x.Detail == c);
        }

        public static DocumentStatus GetDocumentStatusByID(int id)
        {
            return GetDocumentStatusList().FirstOrDefault(x => x.DocumentStatusID == id);
        }
    }
}