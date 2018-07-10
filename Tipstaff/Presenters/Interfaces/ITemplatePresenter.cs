using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public interface ITemplatePresenter
    {
        Template GetTemplate(string id);

        Applicant GetApplicant(string id);

        Solicitor GetSolicitor(string id);

        IEnumerable<Template> GetAllTemplates();

        IEnumerable<Template> GetTemplatesForRecordType(string type);

        void AddTemplate(TemplateEdit model);

        void UpdateTemplate(TemplateEdit model);

    }
}
