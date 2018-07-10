using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Models;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class DocumentPresenter : IDocumentPresenter
    {
        private readonly IDocumentsRepository _docRepository;
        private readonly ISolicitorPresenter _solicitorPresenter;

        public DocumentPresenter(IDocumentsRepository docRepo, ISolicitorPresenter solicitorPresenter)
        {
            _docRepository = docRepo;
            _solicitorPresenter = solicitorPresenter;
        }

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

        public User GetUserByLoginName(string loginName)
        {
            throw new NotImplementedException();
        }
    }
}