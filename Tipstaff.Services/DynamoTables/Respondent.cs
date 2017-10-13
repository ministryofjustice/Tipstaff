using Amazon.DynamoDBv2.DataModel;
using System;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("Tipstaff_Respondents")]
    public class Respondent : DynamoTable
    {
        [DynamoDBRangeKey]
        public string TipstaffRecordID { get; set; }

        public string NameLast { get; set; }
        
        public string NameFirst { get; set; }
       
        public string NameMiddle { get; set; }

        public DateTime? DateOfBirth { get; set; }
       
        public string Gender { get; set; }
        
        public string ChildRelationship { get; set; }
        
        public string HairColour { get; set; }
        
        public string EyeColour { get; set; }
        
        public string SkinColour { get; set; }
        
        public string Height { get; set; }
        
        public string Build { get; set; }
        
        public string Specialfeatures { get; set; }
        
        public string Country { get; set; }
        
        public string Nationality { get; set; }
        
        public string RiskOfViolence { get; set; }
        
        public string RiskOfDrugs { get; set; }
        
        public string PNCID { get; set; }
    }
}
