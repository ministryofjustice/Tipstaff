using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tipstaff.MemoryCollections
{
    public class DeletedReason
    {
        public int DeletedReasonID { get; set; }

        public string Detail { get; set; }

        public int Active { get; set; }
    }

    public class DeletedReasonList
    {
        public static List<DeletedReason> GetDeletedReasonList()
        {
            return new List<DeletedReason>()
            {
                new DeletedReason() { DeletedReasonID=1, Detail="User error", Active=1},
                new DeletedReason() { DeletedReasonID=2, Detail="Duplicate record 2", Active=1},
                new DeletedReason() { DeletedReasonID=3, Detail="Text error in order", Active=1},
                new DeletedReason() { DeletedReasonID=4, Detail="Solicitor Change", Active=1}
            };
        }
    }
}