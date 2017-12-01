﻿using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("Tipstaff_Contacts")]
    public class Contact : DynamoTable
    {
        public int SalutationId{ get; set; }
       
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
   
        public string AddressLine1 { get; set; }
       
        public string AddressLine2 { get; set; }
      
        public string AddressLine3 { get; set; }
      
        public string Town { get; set; }
       
        public string County { get; set; }
       
        public string Postcode { get; set; }
      
        public string DX { get; set; }
       
        public string PhoneHome { get; set; }
       
        public string PhoneMobile { get; set; }
      
        public string Email { get; set; }
       
        public string Notes { get; set; }
     
        public string ContactType { get; set; }
    }
}
