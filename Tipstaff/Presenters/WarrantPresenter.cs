﻿using System;
using System.Collections.Generic;
using System.Linq;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class WarrantPresenter : IWarrantPresenter
    {
        private readonly ITipstaffRecordRepository _tipstaffRecordRepository;
        private readonly IAddressPresenter _addressPresenter;
        private readonly ICaseReviewPresenter _casereviewPresenter;
        private readonly IRespondentPresenter _respondentPresenter;
        private readonly IAttendanceNotePresenter _attendanceNotePresenter;
        private readonly IDocumentPresenter _docPresenter;
        private readonly ISolicitorPresenter _solicitorPresenter;
        private readonly ITipstaffPoliceForcesPresenter _policeForcesPresenter;

        
        public WarrantPresenter(ITipstaffRecordRepository tipstaffRecordRepository, 
            IAddressPresenter addressPresenter, 
            ICaseReviewPresenter casereviewPresenter, 
            IRespondentPresenter respondentPresenter, 
            IAttendanceNotePresenter attendanceNotePresenter, 
            IDocumentPresenter docPresenter, 
            ITipstaffPoliceForcesPresenter policePresenter, 
            ISolicitorPresenter solicitorPresenter)
        {
            _tipstaffRecordRepository = tipstaffRecordRepository;
            _addressPresenter = addressPresenter;
            _casereviewPresenter = casereviewPresenter;
            _respondentPresenter = respondentPresenter;
            _attendanceNotePresenter = attendanceNotePresenter;
            _docPresenter = docPresenter;
            _solicitorPresenter = solicitorPresenter;
            _policeForcesPresenter = policePresenter;
        }
        
        public void AddWarrant(Warrant warrant)
        {
            var entity = GetDynamoTable(warrant);

            var records = _tipstaffRecordRepository.GetAll();

            var orderedRecords = records.OrderByDescending(x => int.Parse(x.Id));

            var record = orderedRecords.First();

            int nextId = int.Parse(record.Id) + 1;

            entity.Id = nextId.ToString();

            warrant.tipstaffRecordID = entity.Id;

            _tipstaffRecordRepository.Add(entity);
        }

        public IEnumerable<Warrant> GetAllWarrants()
        {
            var records = _tipstaffRecordRepository.GetAllByCondition("Discriminator", "Warrant");

            var warrants = records.Select(x => GetModel(x));

            return warrants;
        }

        public IEnumerable<Warrant> GetAllActiveWarrants()
        {
            var records = _tipstaffRecordRepository.GetAllByCondition("Discriminator", "Warrant").Where(w => w.CaseStatusId == 1 || w.CaseStatusId == 2).OrderByDescending(w => w.CreatedOn);

            var warrants = records.Select(x => GetModel(x));

            return warrants;
        }

        public Services.DynamoTables.TipstaffRecord GetDynamoTable(Warrant model)
        {
            var record = new Services.DynamoTables.TipstaffRecord()
            {
                Id = model.tipstaffRecordID,
                CaseNumber = model.caseNumber,
                ExpiryDate = model.expiryDate,
                RespondentName = model.RespondentName,
                DivisionId = MemoryCollections.DivisionsList.GetDivisionByID(model.Division.DivisionId)?.DivisionId,
                Discriminator = model.Discriminator,
                DateCirculated = model.DateCirculated,
                CaseStatusId = MemoryCollections.CaseStatusList.GetCaseStatusList().FirstOrDefault(x => x.CaseStatusId == model.caseStatusID)?.CaseStatusId,
                CreatedOn = model.createdOn,
                CreatedBy = model.createdBy,
                ArrestCount = model.arrestCount,
                DateExecuted = model.DateExecuted,
                NextReviewDate = model.nextReviewDate,
                NPO = model.NPO,
                PrisonCount = model.prisonCount,
                ProtectiveMarkingId = MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().FirstOrDefault(x=>x.ProtectiveMarkingId == model.protectiveMarkingID)?.ProtectiveMarkingId,
                ResultDate = model.resultDate,
                ResultEnteredBy = model.resultEnteredBy,
                ResultId = model.resultID,
            };

            return record;
        }
        
        public Warrant GetWarrant(string id, LazyLoader loader)
        {
            var record = _tipstaffRecordRepository.GetEntityByHashKey(id);
            var warrant = GetModel(record, loader);
            return warrant;
        }
        
        public void RemoveWarrant(Warrant warrant)
        {
            var entity = GetDynamoTable(warrant);

            _tipstaffRecordRepository.Delete(entity);
        }



        public Warrant GetModel(Services.DynamoTables.TipstaffRecord table, LazyLoader loader = null)
        {
            var protectiveMArkingId = table.ProtectiveMarkingId.HasValue ? table.ProtectiveMarkingId.Value : 0;
            int? resultId = table.ResultId.HasValue ? table.ResultId.Value : default(int?);
            var caseStatusId = table.CaseStatusId.HasValue ? table.CaseStatusId.Value : 0;
            var divisionId = table.DivisionId.HasValue ? table.DivisionId.Value : 0;

            if (loader == null)
                loader = new LazyLoader();

            var respondents = loader.LoadRespondents ? _respondentPresenter.GetAllById(table.Id) : null;
            var addresses = loader.LoadAddresses ? _addressPresenter.GetAddressesByTipstaffRecordId(table.Id) : null;
            var linkedSolicitors = loader.LoadSolicitors ? _solicitorPresenter.GetTipstaffRecordSolicitors(table.Id) : null;
            var attendanceNotes = loader.LoadAttendanceNotes ? _attendanceNotePresenter.GetAllById(table.Id) : null;
            var documents = loader.LoadDocuments ? _docPresenter.GetAllDocumentsByTipstaffRecordID(table.Id) : null;
            var caseReviews = loader.LoadCaseReviews ? _casereviewPresenter.GetAllById(table.Id) : null;
            var policeForces = loader.LoadPoliceForces ? _policeForcesPresenter.GetAllTipstaffPoliceForcesByTipstaffRecordID(table.Id) : null;

            var model = new Warrant()
            {
                Discriminator = table.Discriminator,
                tipstaffRecordID = table.Id,
                Division = MemoryCollections.DivisionsList.GetDivisionByID(divisionId),
                caseNumber = table.CaseNumber,
                expiryDate = table.ExpiryDate,
                RespondentName = table.RespondentName,
                DateCirculated = table.DateCirculated,
                addresses = addresses,
                caseReviews = caseReviews,
                Respondents = respondents,
                AttendanceNotes = attendanceNotes,
                LinkedSolicitors = linkedSolicitors,
                policeForces = policeForces,
                Documents = documents,
                caseStatus = MemoryCollections.CaseStatusList.GetCaseStatusByID(caseStatusId),
                createdBy = table.CreatedBy,
                createdOn = table.CreatedOn,
                arrestCount = table.ArrestCount,
                DateExecuted = table.DateExecuted,
                nextReviewDate = table.NextReviewDate,
                NPO = table.NPO,
                protectiveMarking = MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().FirstOrDefault(x=> x.ProtectiveMarkingId == protectiveMArkingId),
                prisonCount = table.PrisonCount,
                resultDate = table.ResultDate,
                resultEnteredBy = table.ResultEnteredBy,
                caseStatusID = caseStatusId,
                protectiveMarkingID = protectiveMArkingId,
                result = MemoryCollections.ResultsList.GetResultList().FirstOrDefault(x => x.ResultId == resultId),
                resultID = resultId,
                RespondentsCount = loader.LoadRespondents ? respondents.Count() : 0
            };

            return model;
        }
        
        public void UpdateWarrant(Warrant warrant)
        {
            var entity = GetDynamoTable(warrant);

            _tipstaffRecordRepository.Update(entity);
        }

        public IEnumerable<Warrant> GetAllClosedWarrants(DateTime start, DateTime end)
        {
            var records = _tipstaffRecordRepository.GetAllByCondition("Discriminator", "Warrant").Where(c => c.CaseStatusId == 3 && c.ResultDate >= start && c.ResultDate <= end).OrderBy(c1 => c1.ResultDate);

            var warrants = records.Select(x => GetModel(x));

            return warrants;
        }
    }
}
