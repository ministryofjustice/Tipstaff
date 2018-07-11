using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public interface IDocumentPresenter
    {
        Document GetDocument(string id);

        void AddDocument(DocumentUploadModel model);

        void DeleteDocument(DeleteDocument model);

    }
}
