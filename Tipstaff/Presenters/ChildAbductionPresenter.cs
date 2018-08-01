using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class ChildAbductionPresenter : IChildAbductionPresenter, IMapper<ChildAbduction, Services.DynamoTables.TipstaffRecord>
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

            var count = _tipstaffRecordRepository.GetAll().Count();

            entity.Id = $"{count++}";

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
            var records = _tipstaffRecordRepository.GetAllByConditions(conditions);
            var childAbductions = records.Select(x=> GetModel(x));

            return childAbductions;
        }

        public IEnumerable<ChildAbduction> GetAllChildAbductionsWithConditions()
        {
            var conditions = new Dictionary<string, object>();
            conditions.Add("Discriminator", "ChildAbduction");
            conditions.Add("CaseStatusId", 1);
            var records = _tipstaffRecordRepository.GetAllByConditions(conditions);
            var childAbductions = records.Select(x => GetModel(x));

            return childAbductions;
        }


        public ChildAbduction GetChildAbduction(string id)
        {
            var record = _tipstaffRecordRepository.GetEntityByHashKey(id);

            var childAbduction = GetModel(record);

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
                CAOrderTypeId = MemoryCollections.CaOrderTypeList.GetOrderTypeList().FirstOrDefault(x => x.CAOrderTypeId == model?.caOrderType?.CAOrderTypeId)?.CAOrderTypeId,
                Discriminator = model.Discriminator,
                NextReviewDate = model.nextReviewDate,
                CaseStatusId = MemoryCollections.CaseStatusList.GetCaseStatusList().FirstOrDefault(x=>x.CaseStatusId == model?.caseStatus?.CaseStatusId)?.CaseStatusId,
                CreatedBy = model.createdBy,
                CreatedOn = model.createdOn,
                NPO = model.NPO,
                ProtectiveMarkingId = MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().FirstOrDefault(x => x.ProtectiveMarkingId == model?.protectiveMarking?.ProtectiveMarkingId)?.ProtectiveMarkingId
            };

            return record;
        }

        public Models.ChildAbduction GetModel(Services.DynamoTables.TipstaffRecord table)
        {
            

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
                caseReviews = _caseReviewsPresenter.GetAllById(table.Id),
                createdBy = table.CreatedBy,
                createdOn = table.CreatedOn,
                Respondents = _respondentPresenter.GetAllById(table.Id),
                children = _childPresenter.GetAllChildrenByTipstaffRecordID(table.Id),
                addresses = _addressPresenter.GetAddressesByTipstaffRecordId(table.Id),
                Applicants = _applicantPresenter.GetAllApplicantsByTipstaffRecordID(table.Id),
                AttendanceNotes = _attendanceNotePresenter.GetAllById(table.Id),
                Documents = _documentPresenter.GetAllDocumentsByTipstaffRecordID(table.Id),
                NPO = table.NPO,
                result = MemoryCollections.ResultsList.GetResultList().FirstOrDefault(x=>x.ResultId==table.ResultId),
                resultDate = table.ResultDate,
                resultEnteredBy = table.ResultEnteredBy,
                arrestCount = table.ArrestCount,
                DateExecuted = table.DateExecuted,
                nextReviewDate = table.NextReviewDate,
                prisonCount = table.PrisonCount,
                protectiveMarking = MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().FirstOrDefault(x=>x.ProtectiveMarkingId==table.ProtectiveMarkingId),
                protectiveMarkingID = table.ProtectiveMarkingId.HasValue?table.ProtectiveMarkingId.Value:0,
                resultID = table.ResultId
            };

            return model;
        }

        public IEnumerable<ChildAbduction> GetAllActiveChildAbductions()
        {
            var records = _tipstaffRecordRepository.GetAllByCondition("Discriminator", "ChildAbduction").Where(w => w.CaseStatusId == 1 || w.CaseStatusId == 2).OrderByDescending(w=>w.CreatedOn);

            var cas = records.Select(x => GetModel(x));

            return cas;
        }

        public IEnumerable<ChildAbduction> GetAllClosedChildAbductions(DateTime start, DateTime end)
        {
            var records = _tipstaffRecordRepository.GetAllByCondition("Discriminator", "ChildAbduction").Where(c => c.CaseStatusId == 3 && c.ResultDate >= start && c.ResultDate <= end).OrderBy(c1 => c1.ResultDate);

            var cas = records.Select(x => GetModel(x));

            return cas;
        }
    }
}
