using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public class DocumentPresenter : IDocumentPresenter
    {
        public void AddDocument(DocumentUploadModel model)
        {
            throw new NotImplementedException();
        }

        public void DeleteDocument(DeleteDocument model)
        {
            throw new NotImplementedException();
        }

        public Document GetDocument(string id)
        {
            throw new NotImplementedException();
        }

        public Template GetTemplate(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Template> GetTemplatesForRecordType(string type)
        {
            throw new NotImplementedException();
        }

        public TipstaffRecord GetTipstaffRecord(string id)
        {
            throw new NotImplementedException();
        }

        public User GetUserByLoginName(string loginName)
        {
            throw new NotImplementedException();
        }
    }
}