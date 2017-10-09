using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.dto
{
    public class Template
    {
        public string TemplateID { get; set; }

        public string Discriminator { get; set; }

        public string TemplateName { get; set; }

        public string FilePath { get; set; }

        public bool AddresseeRequired { get; set; }

        public bool Active { get; set; }

        public DateTime? Deactivated { get; set; }

        public string DeactivatedBy { get; set; }
    }
}
