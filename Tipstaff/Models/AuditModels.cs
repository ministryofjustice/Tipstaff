using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tipstaff.MemoryCollections;

namespace Tipstaff.Models
{
    public class AuditEventViewModel
    {
        public PagedList.IPagedList<AuditEvent> AuditEvents { get; set; }
        public string auditType { get; set; }
        public string itemID { get; set; }
    }

    public class AuditEvent
    {
        [Key]
        public string idAuditEvent { get; set; }

 
        public DateTime EventDate { get; set; }

        public string UserID { get; set; }

        public string RecordChanged { get; set; }
        public string RecordAddedTo { get; set; }

        public AuditEventDescription auditEventDescription { get; set; }

        public DeletedReason DeletedReason { get; set; }

        public string ColumnName { get; set; }

        public string Was { get; set; }

        public string Now { get; set; }

        //public virtual ICollection<AuditEventDataRow> AuditEventDataRows { get; set; }

        //I think the below is the Detail of the MemoryCollection.AuditEventDescription
        //public string EventDescription { get; set; }

    }


    //public class AuditEventDataRow
    //{
    //    [Key]
    //    public int idAuditData { get; set; }
    //    [Required]
    //    public int idAuditEvent { get; set; }
    //    [Required, MaxLength(200)]
    //    public string ColumnName { get; set; }
    //    [Required, MaxLength(200)]
    //    public string Was { get; set; }
    //    [Required, MaxLength(200)]
    //    public string Now { get; set; }

    //    public virtual AuditEvent auditEvent { get; set; }   
    //}
}