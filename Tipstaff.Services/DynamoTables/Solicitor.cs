using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.DynamoTables
{
    [DynamoDBTable("Tipstaff_Solicitors")]
    public class Solicitor : DynamoTable
    {
        public string SolicitorFirmID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Salutation { get; set; }

        public string PhoneDayTime { get; set; }

        public string PhoneOutOfHours { get; set; }

        public string Email { get; set; }

        public bool Active { get; set; }

        public DateTime? Dectivated { get; set; }

        public string DectivatedBy { get; set; }

        public string SolicitorName { get; set; }

        public string FirmName { get; set; }
    }
}
