using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.DynamoTables
{
    [DynamoDBTable("Tipstaff_Documents")]
    public class Document : DynamoTable
    {

        [DynamoDBRangeKey]
        public string TipstaffRecordID { get; set; }

        public string DocumentReference { get; set; }
        
        public string Country { get; set; }
        
        public string Nationality { get; set; }
        
        public string DocumentStatus { get; set; }
        
        public string DocumentType { get; set; }
        public string TemplateID { get; set; }
        
        public DateTime CreatedOn { get; set; }
        
        public string CreatedBy { get; set; }
        
        
        public string FilePath { get; set; }
        
        public string FileName { get; set; }
        
        public string MimeType { get; set; }
    }
}
