using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Cache;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class LazyLoader
    {
        public bool LoadCaseReviews { get; set; }
        public bool LoadAddresses { get; set; }
        public bool LoadRespondents { get; set; }
        public bool LoadSolicitors { get; set; }
        public bool LoadAttendanceNotes { get; set; }
        public bool LoadDocuments { get; set; }
        public bool LoadApplicants { get; set; }
        public bool LoadChildren { get; set; }
        public bool LoadPoliceForces { get; set; }
    }
        
    public class ChildAbductionPresenter : IChildAbductionPresenter
    {
        private readonly ITipstaffRecordRepository _tipstaffRecordRepository;
        private readonly IDeletedTipstaffRecordRepository _deletedTipstaffRecordRepository;
        private readonly ICaseReviewPresenter _caseReviewsPresenter;
        private readonly IRespondentPresenter _respondentPresenter;
        private readonly IChildPresenter _childPresenter;
        private readonly IAddressPresenter _addressPresenter;
        private readonly IApplicantPresenter _applicantPresenter;
        private readonly ISolicitorPresenter _solicitorPresenter;
        private readonly IAttendanceNotePresenter _attendanceNotePresenter;
        private readonly IDocumentPresenter _documentPresenter;
        private readonly EasyCache _cache;

        public ChildAbductionPresenter(ITipstaffRecordRepository tipstaffRecordRepository, 
            IDeletedTipstaffRecordRepository deletedTipstaffRecordRepository, 
            ICaseReviewPresenter caseReviewsPresenter, 
            IRespondentPresenter respondentPresenter, 
            IChildPresenter childPresenter, 
            IAddressPresenter addressPresenter, 
            IApplicantPresenter applicantPresenter, 
            ISolicitorPresenter solicitorPresenter, IAttendanceNotePresenter attendanceNotePresenter, 
            IDocumentPresenter documentPresenter)
        {
            _tipstaffRecordRepository = tipstaffRecordRepository;
            _deletedTipstaffRecordRepository = deletedTipstaffRecordRepository;
            _caseReviewsPresenter = caseReviewsPresenter;
            _respondentPresenter = respondentPresenter;
            _childPresenter = childPresenter;
            _addressPresenter = addressPresenter;
            _applicantPresenter = applicantPresenter;
            _solicitorPresenter = solicitorPresenter;
            _attendanceNotePresenter = attendanceNotePresenter;
            _documentPresenter = documentPresenter;
            _cache = new EasyCache();
        }

        public void AddDeletedTipstaffRecord(Models.DeletedTipstaffRecord record)
        {
            var entity = new Services.DynamoTables.DeletedTipstaffRecord()
            {
                Id = record.TipstaffRecordID,
                DeletedReasonId = MemoryCollections.DeletedReasonList.GetDeletedReasonList().FirstOrDefault(x=>x.Detail == record.deletedReason.Detail).DeletedReasonID,
                UniqueRecordID = record.UniqueRecordID,
                Discriminator = record.discriminator
            };

            _deletedTipstaffRecordRepository.Add(entity);
        }

        public void AddTipstaffRecord(ChildAbduction childabduction)
        {
           var entity = GetDynamoTable(childabduction);

            var records = _tipstaffRecordRepository.GetAll();

            var orderedRecords = records.OrderByDescending(x => int.Parse(x.Id));

            var record = orderedRecords.First();

            int nextId = int.Parse(record.Id) + 1;

            entity.Id = nextId.ToString();

            childabduction.tipstaffRecordID = entity.Id;

            _tipstaffRecordRepository.Add(entity);
        }

        public void DeletedTipstaffRecords(Models.DeletedTipstaffRecord record)
        {
            var entity = new Services.DynamoTables.DeletedTipstaffRecord()
            {
                Id = record.TipstaffRecordID,
                DeletedReasonId = MemoryCollections.DeletedReasonList.GetDeletedReasonList().FirstOrDefault(x => x.Detail == record.deletedReason.Detail).DeletedReasonID,
                UniqueRecordID = record.UniqueRecordID,
                Discriminator = record.discriminator
            };

            _deletedTipstaffRecordRepository.Remove(entity);
        }

        public IEnumerable<ChildAbduction> GetAllChildAbductions()
        {
            var conditions = new Dictionary<string, object>();
            conditions.Add("Discriminator", "ChildAbduction");
            //conditions.Add("CaseStatusId", 1);
            IEnumerable<Tipstaff.Services.DynamoTables.TipstaffRecord> records = new List<Tipstaff.Services.DynamoTables.TipstaffRecord>();

            IEnumerable<Tipstaff.Services.DynamoTables.TipstaffRecord> recs = _cache.GetItems<Tipstaff.Services.DynamoTables.TipstaffRecord>(CacheItem.CA);

            if (recs == null)
            {
                records = _tipstaffRecordRepository.GetAllByConditions(conditions);
                _cache.RefreshCache(CacheItem.CA, records, new DateTimeOffset(DateTime.Now.AddMinutes(30)));
            }
            else
                records = recs;

            var childAbductions = records.Select(x=> GetModel(x,new LazyLoader() { LoadRespondents = true,LoadChildren =true }));

            return childAbductions;
        }
        

        public ChildAbduction GetChildAbduction(string id)
        {
            var record = _tipstaffRecordRepository.GetEntityByHashKey(id);

            var lazyLoader = new LazyLoader()
            {
                LoadAddresses = true,
                LoadApplicants = true,
                LoadAttendanceNotes = true,
                LoadCaseReviews = true,
                LoadChildren = true,
                LoadDocuments = true,
                LoadRespondents = true,
                LoadSolicitors = true
            };

            var childAbduction = GetModel(record, lazyLoader);

            return childAbduction;
        }

        public void RemoveChildAbduction(ChildAbduction childAbduction)
        {
            var entity = GetDynamoTable(childAbduction);

            _tipstaffRecordRepository.Delete(entity);
        }

        public void UpdateChildAbduction(ChildAbduction childAbduction)
        {
            var entity = GetDynamoTable(childAbduction);

            _tipstaffRecordRepository.Update(entity);
        }

        public Services.DynamoTables.TipstaffRecord GetDynamoTable(ChildAbduction model)
        {
            var record = new Services.DynamoTables.TipstaffRecord()
            {
                Id = model.tipstaffRecordID,
                SentSCD26 = model.sentSCD26,
                OfficerDealing = model.officerDealing,
                OrderDated = model.orderDated,
                OrderReceived = model.orderReceived,
                EldestChild = model.EldestChild,
                CAOrderTypeId = (model.caOrderType != null) ? model.caOrderType.CAOrderTypeId : model.CAOrderTypeId,
                Discriminator = model.Discriminator,
                NextReviewDate = model.nextReviewDate,
                CaseStatusId = (model.caseStatus!=null) ? model.caseStatus.CaseStatusId : model.caseStatusID,
                CreatedBy = model.createdBy,
                CreatedOn = model.createdOn,
                NPO = model.NPO,
                ResultId = model.resultID,
                ProtectiveMarkingId = (model.protectiveMarking != null) ? model.protectiveMarking.ProtectiveMarkingId : model.protectiveMarkingID,
            };

            return record;
        }

        public Models.ChildAbduction GetModel(Services.DynamoTables.TipstaffRecord table, LazyLoader loader=null)
        {
            if (loader == null)
                loader = new LazyLoader();

            var Respondents = loader.LoadRespondents ? _respondentPresenter.GetAllById(table.Id) : null;
            var children = loader.LoadChildren ? _childPresenter.GetAllChildrenByTipstaffRecordID(table.Id) : null;
            var caseReviews = loader.LoadCaseReviews ? _caseReviewsPresenter.GetAllById(table.Id) : null;
            var addresses = loader.LoadAddresses ? _addressPresenter.GetAddressesByTipstaffRecordId(table.Id) : null;
            var applicants = loader.LoadAddresses ? _applicantPresenter.GetAllApplicantsByTipstaffRecordID(table.Id) : null;
            var linkedSolicitors = loader.LoadSolicitors ? _solicitorPresenter.GetTipstaffRecordSolicitors(table.Id) : null;
            var attendanceNotes = loader.LoadAttendanceNotes ? _attendanceNotePresenter.GetAllById(table.Id) : null;
            var documents = loader.LoadDocuments ? _documentPresenter.GetAllDocumentsByTipstaffRecordID(table.Id) : null;

            var model = new Models.ChildAbduction()
            {
                sentSCD26 = table.SentSCD26,
                Discriminator = table.Discriminator,
                orderDated = table.OrderDated,
                orderReceived = table.OrderReceived,
                officerDealing = table.OfficerDealing,
                EldestChild = table.EldestChild,
                caOrderType = MemoryCollections.CaOrderTypeList.GetOrderTypeList().FirstOrDefault(x => x.CAOrderTypeId == table.CAOrderTypeId),
                tipstaffRecordID = table.Id,
                caseStatus = MemoryCollections.CaseStatusList.GetCaseStatusList().FirstOrDefault(x => x.CaseStatusId == table.CaseStatusId),
                caseStatusID = table.CaseStatusId.HasValue ? table.CaseStatusId.Value : 0,
                createdBy = table.CreatedBy,
                createdOn = table.CreatedOn,
                Respondents = Respondents,
                children = children,
                caseReviews = caseReviews,
                addresses = addresses,
                Applicants = applicants,
                LinkedSolicitors = linkedSolicitors,
                AttendanceNotes = attendanceNotes,
                Documents = documents,
                NPO = table.NPO,
                result = MemoryCollections.ResultsList.GetResultList().FirstOrDefault(x=>x.ResultId==table.ResultId),
                resultDate = table.ResultDate,
                resultEnteredBy = table.ResultEnteredBy,
                arrestCount = table.ArrestCount,
                DateExecuted = table.DateExecuted,
                nextReviewDate = table.NextReviewDate,
                prisonCount = table.PrisonCount,
                protectiveMarking = MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().FirstOrDefault(x=>x.ProtectiveMarkingId==table.ProtectiveMarkingId),
                protectiveMarkingID = table.ProtectiveMarkingId.HasValue ? table.ProtectiveMarkingId.Value:0,
                CAOrderTypeId = table.CAOrderTypeId.HasValue ? table.CAOrderTypeId.Value :0,
                resultID = table.ResultId
            };

            return model;
        }
        

        public IEnumerable<ChildAbduction> GetAllClosedChildAbductions(DateTime start, DateTime end)
        {
            var conditions = new Dictionary<string, object>();
            conditions.Add("Discriminator", "ChildAbduction");
            conditions.Add("CaseStatusId", 3);

            IEnumerable<Tipstaff.Services.DynamoTables.TipstaffRecord> recs = _cache.GetItems<Tipstaff.Services.DynamoTables.TipstaffRecord>(CacheItem.CA);
            IEnumerable<Tipstaff.Services.DynamoTables.TipstaffRecord> records = new List<Tipstaff.Services.DynamoTables.TipstaffRecord>();


            if (recs == null)
            {
                records = _tipstaffRecordRepository.GetAllByConditions(conditions);
                _cache.RefreshCache(CacheItem.CA, records, new DateTimeOffset(DateTime.Now.AddMinutes(60)));
            }
            else
                records = recs.Where(x => x.CaseStatusId == 3);


            var recordswithFilter = records.Where(c => c.ResultDate >= start && c.ResultDate <= end).OrderBy(c1 => c1.ResultDate);
            var cas = records.Select(x => GetModel(x,new LazyLoader() { LoadRespondents = true, LoadChildren = true }));

            return cas;
        }

        public IEnumerable<ChildAbduction> GetActiveChildAbductions()
        {
            var conditions = new Dictionary<string, object>();
            conditions.Add("Discriminator", "ChildAbduction");
            conditions.Add("CaseStatusId", 2);
            //var records = _tipstaffRecordRepository.GetAllByConditions(conditions);

            IEnumerable<Tipstaff.Services.DynamoTables.TipstaffRecord> recs = _cache.GetItems<Tipstaff.Services.DynamoTables.TipstaffRecord>(CacheItem.CA);
            IEnumerable<Tipstaff.Services.DynamoTables.TipstaffRecord> records = new List<Tipstaff.Services.DynamoTables.TipstaffRecord>();


            if (recs == null)
            {
                records = _tipstaffRecordRepository.GetAllByConditions(conditions);
                _cache.RefreshCache(CacheItem.CA, records, new DateTimeOffset(DateTime.Now.AddMinutes(60)));
            }
            else
                records = recs.Where(x=>x.CaseStatusId==2);


            var childAbductions = records.Select(x => GetModel(x, new LazyLoader() { LoadRespondents = true, LoadChildren = true }));

            return childAbductions;
        }
    }
}
