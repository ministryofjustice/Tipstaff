using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.dto;

namespace Tipstaff.Services.Services
{
    public interface ITemplateServices
    {
        Template GetTemplate(string id);

        List<Template> GetAllTemplates();

        void AddTemplate(Template model);

        void UpdateTemplate(Template model);

        dto.Tipstaff GetTipstaffRecord(string id);

        Applicant GetApplicant(string id);
    }
}
