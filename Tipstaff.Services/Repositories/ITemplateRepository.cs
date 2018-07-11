using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface ITemplateRepository
    {
        void AddTemplate(Template template);

        Template GetTemplate(string id);

        IEnumerable<Template> GetAllTemplates();

        void Update(Template template);

        void Delete(Template template);

        IEnumerable<Template> GetTemplatesForRecordType(string type);

    }
}
