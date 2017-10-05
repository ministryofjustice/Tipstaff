using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenter
{
    public interface IPresenterDocument
    {
        Document GetDocument(string id);

        TipstaffRecord GetTipstaffRecord(string id);

        Template GetTemplate(string id);

        IEnumerable<Template> GetTemplatesForRecordType(string type); //db.Templates.Where(t => (t.Discriminator == docType || t.Discriminator == "All") && t.active).OrderBy(t=>t.Discriminator);

        User GetUserByLoginName(string loginName);

        void AddDocument(DocumentUploadModel model);

        void DeleteDocument(DeleteDocument model);

    }
}
