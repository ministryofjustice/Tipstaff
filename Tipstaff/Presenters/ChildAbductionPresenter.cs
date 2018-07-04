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

        private Object _lock = new Object();

        public ChildAbductionPresenter(ITipstaffRecordRepository tipstaffRecordRepository, 
            IDeletedTipstaffRecordRepository deletedTipstaffRecordRepository, 
            ICaseReviewPresenter caseReviewsPresenter, 
            IRespondentPresenter respondentPresenter, 
            IChildPresenter childPresenter, 
            IAddressPresenter addressPresenter, 
            IApplicantPresenter applicantPresenter, 
            ISolicitorPresenter solicitorPresenter, IAttendanceNotePresenter attendanceNotePresenter)
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
            lock (_lock)
            {
                var entity = GetDynamoTable(childabduction);

                var count = _tipstaffRecordRepository.GetAll().Count();

                entity.Id = $"{count++}";

                childabduction.tipstaffRecordID = entity.Id;

                _tipstaffRecordRepository.Add(entity);
            }
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
            var records = _tipstaffRecordRepository.GetAllByCondition("Discriminator", "ChildAbduction");

            var childAbductions = records.Select(x=> GetModel(x));

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
                CAOrderTypeId = MemoryCollections.CaOrderTypeList.GetOrderTypeList().FirstOrDefault(x => x.CAOrderTypeId == model?.caOrderType.CAOrderTypeId)?.CAOrderTypeId,
                Discriminator = model.Discriminator,
                NextReviewDate = model.nextReviewDate,
                CaseStatusId = MemoryCollections.CaseStatusList.GetCaseStatusList().FirstOrDefault(x=>x.CaseStatusId == model?.caseStatus.CaseStatusId)?.CaseStatusId,
                CreatedBy = model.createdBy,
                CreatedOn = model.createdOn,
                NPO = model.NPO
                
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
                caseReviews = _caseReviewsPresenter.GetAllById(table.Id),
                createdBy = table.CreatedBy,
                createdOn = table.CreatedOn,
                Respondents = _respondentPresenter.GetAllById(table.Id),
                children = _childPresenter.GetAllChildrenByTipstaffRecordID(table.Id),
                addresses = _addressPresenter.GetAddressesByTipstaffRecordId(table.Id),
                Applicants = _applicantPresenter.GetAllApplicantsByTipstaffRecordID(table.Id),
                AttendanceNotes = _attendanceNotePresenter.GetAllById(table.Id),
                NPO = table.NPO,
                
                
                
            };

            return model;
        }
    }
}


//<li><a href = "@Url.Action("ListChildrenByRecord", "Child", new { id = Model.tipstaffRecordID })">Children(@Model.children.Count())</a></li>
//		<li><a href = "@Url.Action("ListRespondentsByRecord", "Respondent", new { id = Model.tipstaffRecordID })">Respondents(@Model.Respondents.Count())</a></li>
//		<li><a href = "@Url.Action("ListAddressesByRecord", "Address", new { id = Model.tipstaffRecordID })">Addresses(@Model.addresses.Count())</a></li>
//		<li><a href = "@Url.Action("ListAttendanceNotesByRecord", "AttendanceNote", new { id = Model.tipstaffRecordID })">Attendance notes(@Model.AttendanceNotes.Count())</a></li>
//		<li><a href = "@Url.Action("ListDocumentsByRecord", "Document", new { id = Model.tipstaffRecordID })">Documents(@Model.Documents.Count())</a></li>
//		<li><a href = "@Url.Action("ListSolicitorsByRecord", "Solicitor", new { id = Model.tipstaffRecordID })">Solicitors(@Model.LinkedSolicitors.Count())</a></li>
//		<li><a href = "@Url.Action("ListApplicantsByRecord", "Applicant", new { id = Model.tipstaffRecordID })">Applicants(@Model.Applicants.Count())</a></li>
//		<li><a href = "@Url.Action("ListCaseReviewsByRecord", "CaseReview", new { id = Model.tipstaffRecordID })">Case Reviews(@Model.caseReviews.Count())</a></li>
//        <li><a href = "@Url.Action("ListPNCIDAndNPOByRecord", "NPO", new { id = Model.tipstaffRecordID })">PNCIDs & NPO</a></li>
//        <li><a href = "@Url.Action("ListPoliceForcesByRecord", "PoliceForces", new { id = Model.tipstaffRecordID, Area="Admin" })">Police Forces</a></li>