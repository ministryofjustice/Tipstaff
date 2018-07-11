using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class DocumentPresenter : IDocumentPresenter, IMapper<Models.Document, Services.DynamoTables.Document>, IMapperCollections<Models.Document, Services.DynamoTables.Document>
    {
        private readonly IDocumentsRepository _docRepository;

        public DocumentPresenter(IDocumentsRepository docRepo)
        {
            _docRepository = docRepo;
        }

        public void AddDocument(DocumentUploadModel model)
        {
            var entity = GetDynamoTable(model.document);
            _docRepository.AddDocument(entity);
        }

        public void DeleteDocument(DeleteDocument model)
        {
            var entity = GetDynamoTable(model.Document);

            _docRepository.DeleteDocument(entity);
        }

        public Models.Document GetDocument(string id)
        {
            var entity = _docRepository.GetDocument(id);
            return GetModel(entity);
        }

        public Services.DynamoTables.Document GetDynamoTable(Models.Document model)
        {
            var entity = new Services.DynamoTables.Document()
            {
                Id = model.documentID,
                Country = MemoryCollections.CountryList.GetCountryByID(model.country.CountryID).Detail,
                CreatedBy = model.createdBy,
                CreatedOn = model.createdOn,
                DocumentReference = model.documentReference,
                DocumentStatus = MemoryCollections.DocumentStatusList.GetDocumentStatusByID(model.documentStatus.DocumentStatusID).Detail,
                DocumentType = MemoryCollections.DocumentTypeList.GetDocumentTypeByID(model.documentType.DocumentTypeID).Detail,
                FileName = model.fileName,
                MimeType = model.mimeType,
                Nationality = MemoryCollections.NationalityList.GetNationalityByID(model.nationality.NationalityID).Detail,
                TipstaffRecordID = model.tipstaffRecordID,
                FilePath = model.filePath,
                TemplateID = model.templateID
            };

            return entity;
        }

        public Models.Document GetModel(Services.DynamoTables.Document table)
        {
            var model = new Models.Document()
            {
                documentID = table.Id,
                tipstaffRecordID = table.TipstaffRecordID,
                country = MemoryCollections.CountryList.GetCountryByDetail(table.Country),
                createdBy = table.CreatedBy,
                createdOn = table.CreatedOn,
                documentReference = table.DocumentReference,
                documentStatus = MemoryCollections.DocumentStatusList.GetDocumentStatusByDetail(table.DocumentStatus),
                documentType = MemoryCollections.DocumentTypeList.GetDocumentTypeByDetail(table.DocumentType),
                fileName = table.FileName,
                filePath = table.FilePath,
                mimeType = table.MimeType,
                nationality = MemoryCollections.NationalityList.GetNationalityByDetail(table.Nationality),
                template = null,
                templateID = table.TemplateID,
                tipstaffRecord = null
            };

            return model;
        }

        public IEnumerable<Models.Document> GetAll(IEnumerable<Services.DynamoTables.Document> entities)
        {
            return entities.Select(x => GetModel(x));
        }

        public IEnumerable<Services.DynamoTables.Document> GetAll(IEnumerable<Models.Document> entities)
        {
            return entities.Select(x => GetDynamoTable(x));
        }

        public IEnumerable<Models.Document> GetAllDocumentsByTipstaffRecordID(string id)
        {
            var docs = _docRepository.GetAllDocumentsByTipstaffRecordID(id);
            return docs.Select(x => GetModel(x));
        }
    }
}