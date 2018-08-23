using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Tipstaff.Cache;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class TipstaffRecordPresenter : ITipstaffRecordPresenter
    {
        private readonly ITipstaffRecordRepository _tipstaffRecordRepository;
        private readonly IRespondentPresenter _respondentPresenter;
        private readonly ICaseReviewPresenter _caseReviewPresenter;
        private readonly IAddressPresenter _addressPresenter;
        private readonly ISolicitorPresenter _solicitorPresenter;
        private readonly ICacheRepository _cacheRepository;
        private readonly EasyCache _cache;
      

        public TipstaffRecordPresenter(ITipstaffRecordRepository tipstaffRecordRepository, 
                                       IRespondentPresenter respondentPresenter, 
                                       ICaseReviewPresenter caseReviewPresenter, 
                                       IAddressPresenter addressPresenter, 
                                       ISolicitorPresenter solicitorPresenter, ICacheRepository cacheRepository)
        {
            _tipstaffRecordRepository = tipstaffRecordRepository;
            _respondentPresenter = respondentPresenter;
            _caseReviewPresenter = caseReviewPresenter;
            _addressPresenter = addressPresenter;
            _solicitorPresenter = solicitorPresenter;
            _cacheRepository = cacheRepository;
            _cache = new EasyCache();
        }
        
        public IEnumerable<Models.TipstaffRecord> GetAll()
        {
            //var entities = _tipstaffRecordRepository.GetAllByCondition<int>("CaseStatusId", 2);

            var conditions = new Dictionary<string, object>();
            conditions.Add("CaseStatusId", 2);

            IEnumerable<Tipstaff.Services.DynamoTables.TipstaffRecord> entities = new List<Tipstaff.Services.DynamoTables.TipstaffRecord>();

            IEnumerable<Tipstaff.Services.DynamoTables.TipstaffRecord> recs = _cache.GetItems<Tipstaff.Services.DynamoTables.TipstaffRecord>(CacheItem.TipstaffRecords);

            if (recs == null)
            {
                entities = _tipstaffRecordRepository.GetAllByConditions(conditions);
                _cache.RefreshCache(CacheItem.TipstaffRecords, entities, new DateTimeOffset(DateTime.Now.AddMinutes(30)));
            }
            else
            {
                entities = recs;
                _cacheRepository.Add(new Services.DynamoTables.CacheStore() { Context = "GetAllTipstaffRecords", DateTime = DateTime.Now });
            }


            //            var records = entities.Select(x => GetModel(x));
            //var records = new List<Models.TipstaffRecord>();
            //var r = Parallel.ForEach(entities, new ParallelOptions() { MaxDegreeOfParallelism = 50 }, rec => 
            //{
            //    records.Add(GetModel(rec));
            //});
            return entities.Select(x=> GetModel(x));
        }
        
        public Models.TipstaffRecord GetModel(Services.DynamoTables.TipstaffRecord table, LazyLoader loader = null)
        {
            if (loader == null)
                loader = new LazyLoader();

            var respondents = loader.LoadRespondents ? _respondentPresenter.GetAllById(table.Id) : null;
            var addresses = loader.LoadAddresses ? _addressPresenter.GetAddressesByTipstaffRecordId(table.Id) : null;
            var caseReviews = loader.LoadAttendanceNotes ? _caseReviewPresenter.GetAllById(table.Id) : null;
            var linkedSolicitors = loader.LoadSolicitors ? _solicitorPresenter.GetTipstaffRecordSolicitors(table.Id) : null;
            
            var model = new Models.TipstaffRecord()
            {
                arrestCount = table.ArrestCount,
                createdBy = table.CreatedBy,
                createdOn = table.CreatedOn,
                nextReviewDate = table.NextReviewDate,
                NPO = table.NPO,
                DateExecuted = table.DateExecuted,
                prisonCount = table.PrisonCount,
                resultDate = table.ResultDate,
                tipstaffRecordID = table.Id,
                resultEnteredBy = table.ResultEnteredBy,
               
                addresses = addresses,
                LinkedSolicitors = linkedSolicitors,
                caseReviews = caseReviews,
                Respondents = respondents,

                Discriminator = table.Discriminator,
                result = MemoryCollections.ResultsList.GetResultList().FirstOrDefault(x=>x.ResultId == table.ResultId),
                caseStatus = MemoryCollections.CaseStatusList.GetCaseStatusList().FirstOrDefault(x=>x.CaseStatusId == table.CaseStatusId),
                protectiveMarking = MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().FirstOrDefault(x=>x.ProtectiveMarkingId == table.ProtectiveMarkingId)
            };

            return model;
        }

        public Models.TipstaffRecord GetTipStaffRecord(string id, LazyLoader loader = null)
        {
            var record = _tipstaffRecordRepository.GetEntityByHashKey(id);
           
             if (loader == null)
                loader = new LazyLoader();

            if (record != null)
            {
                var model = GetModel(record,loader);
                return model;
            }
            return new Models.TipstaffRecord();
        }

        public ChildAbduction GetChildAbduction(string id)
        {
            var record = _tipstaffRecordRepository.GetEntityByHashKey(id);
            if (record != null)
            {
                var childAbduction = new ChildAbduction()
                {
                    arrestCount = record.ArrestCount,
                    createdBy = record.CreatedBy,
                    createdOn = record.CreatedOn,
                    nextReviewDate = record.NextReviewDate,
                    NPO = record.NPO,
                    DateExecuted = record.DateExecuted,
                    prisonCount = record.PrisonCount,
                    resultDate = record.ResultDate,
                    tipstaffRecordID = record.Id,
                    resultEnteredBy = record.ResultEnteredBy,
                    Discriminator = record.Discriminator,
                    result = MemoryCollections.ResultsList.GetResultList().FirstOrDefault(x => x.ResultId == record.ResultId),
                    caseStatus = MemoryCollections.CaseStatusList.GetCaseStatusList().FirstOrDefault(x => x.CaseStatusId == record.CaseStatusId),
                    protectiveMarking = MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().FirstOrDefault(x => x.ProtectiveMarkingId == record.ProtectiveMarkingId)
                };

                return childAbduction;
            }
            return null;
        }

        public void UpdateTipstaffRecord(Models.TipstaffRecord record)
        {
            var entity = new Services.DynamoTables.TipstaffRecord()
            {
                ArrestCount = record.arrestCount,
                Discriminator = record.Discriminator,
                DateExecuted = record.DateExecuted,
                CreatedBy = record.createdBy,
                NPO = record.NPO,
                Id = record.tipstaffRecordID,
                NextReviewDate = record.nextReviewDate,
                PrisonCount = record.prisonCount,
                ResultDate = record.resultDate,
                ProtectiveMarkingId = record.protectiveMarking?.ProtectiveMarkingId,
                CaseStatusId = record.caseStatus.CaseStatusId,
                CreatedOn = record.createdOn,
                ResultId = record.resultID,
                ResultEnteredBy = record.resultEnteredBy
             };
            
            _tipstaffRecordRepository.PartialUpdate(entity,record.Discriminator);
        }
    }
}