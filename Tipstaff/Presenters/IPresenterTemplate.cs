using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public interface IPresenterTemplate
    {
        Template GetTemplate(string id);

        TipstaffRecord GetTipstaffRecord(string id);

        Applicant GetApplicant(string id);

        Solicitor GetSolicitor(string id);

        IEnumerable<Template> GetAllTemplates();

        void AddTemplate(TemplateEdit model);

        void UpdateTemplate(TemplateEdit model);

    }
}
