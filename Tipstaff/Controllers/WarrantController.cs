﻿using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using PagedList;

using Tipstaff.Models;
using Tipstaff.Presenters;
using System.Collections.Generic;
using TPLibrary.Logger;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class WarrantController : Controller
    {
        //private TipstaffDB db = myDBContextHelper.CurrentContext;
        private readonly IWarrantPresenter _warrantPresenter;
        private readonly ITipstaffRecordPresenter _tipstaffRecordPresenter;
        private readonly IRespondentPresenter _respondentPresenter;
        private readonly ICloudWatchLogger _logger;

        public WarrantController(IWarrantPresenter warrantPresenter, 
                                 ITipstaffRecordPresenter tipstaffRecordPresenter, 
                                 IRespondentPresenter respondentPresenter, ICloudWatchLogger logger)
        {
            _warrantPresenter = warrantPresenter;
            _tipstaffRecordPresenter = tipstaffRecordPresenter;
            _respondentPresenter = respondentPresenter;
            _logger = logger;
        }

        public ViewResult Index(WarrantListViewModel model)
        {
            //////IQueryable<Warrant> TRs = myDBContextHelper.CurrentContext.Warrants;
            var TRs = _warrantPresenter.GetAllWarrants();

            model.TotalRecordCount = TRs.Count();

            if (!model.includeFinal)
            {
                TRs = TRs.Where(x => x.resultID == null);
            }
            if (model.caseStatusID > -1)
            {
                TRs = TRs.Where(w => w.caseStatus?.CaseStatusId == model.caseStatusID);

            }
            if (model.divisionID > -1)
            {
                TRs = TRs.Where(w => w.Division?.DivisionId == model.divisionID);

            }
            if (!string.IsNullOrEmpty(model.caseNumberContains))
            {
                TRs = TRs.Where(w => !string.IsNullOrEmpty(w.caseNumber) && w.caseNumber.Contains(model.caseNumberContains.ToUpper()));
            }
            if (!string.IsNullOrEmpty(model.respondentNameContains))
            {
                TRs = TRs.Where(w => string.IsNullOrEmpty(w.RespondentName)?false:w.RespondentName.Contains(model.respondentNameContains.ToUpper()));
            }
            switch (model.sortOrder)
            {
                case "created asc":
                    TRs = TRs.OrderBy(a => a.createdOn).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                case "created desc":
                    TRs = TRs.OrderByDescending(a => a.createdOn).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                case "uniqueid asc":
                    TRs = TRs.OrderBy(a => int.Parse(a.tipstaffRecordID));
                    break;
                case "uniqueid desc":
                    TRs = TRs.OrderByDescending(a => int.Parse(a.tipstaffRecordID));
                    break;
                case "casenumber asc":
                    TRs = TRs.OrderBy(a => a.caseNumber).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                case "casenumber desc":
                    TRs = TRs.OrderByDescending(a => a.caseNumber).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                case "division asc":
                    TRs = TRs.OrderBy(a => a.Division?.Detail).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                case "division desc":
                    TRs = TRs.OrderByDescending(a => a.Division?.Detail).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                case "reviewDate asc":
                    TRs = TRs.OrderBy(a => a.nextReviewDate).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                case "reviewDate desc":
                    TRs = TRs.OrderByDescending(a => a.nextReviewDate).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                case "displayName asc":
                    TRs = TRs.OrderBy(a => a.RespondentName).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                case "displayName desc":
                    TRs = TRs.OrderByDescending(a => a.RespondentName).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                case "expiryDate asc":
                    TRs = TRs.OrderBy(a => a.expiryDate).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                case "expiryDate desc":
                    TRs = TRs.OrderByDescending(a => a.expiryDate).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                case "protMark asc":
                    TRs = TRs.OrderBy(a => a.protectiveMarking?.Detail).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                case "protMark desc":
                    TRs = TRs.OrderByDescending(a => a.protectiveMarking?.Detail).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                case "caseStatus asc":
                    TRs = TRs.OrderBy(a => a.caseStatus?.Detail).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                case "caseStatus desc":
                    TRs = TRs.OrderByDescending(a => a.caseStatus?.Detail).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                case "result asc":
                    TRs = TRs.OrderBy(a => a.result?.Detail).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                case "result desc":
                    TRs = TRs.OrderByDescending(a => a.result?.Detail).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                case "resultEnter asc":
                    TRs = TRs.OrderBy(a => a.resultEnteredBy).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                case "resultEnter desc":
                    TRs = TRs.OrderByDescending(a => a.resultEnteredBy).ThenBy(b => int.Parse(b.tipstaffRecordID));
                    break;
                default:
                    TRs = TRs.OrderBy(a => int.Parse(a.tipstaffRecordID));
                    break;
            }
            model.Warrants = TRs.ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }
        //
        // GET: /Warrant/Details/5
        public ActionResult Details(string id)
        {
            try
            {
                var loader = new LazyLoader()
                {
                    LoadAddresses = true,
                    LoadAttendanceNotes = true,
                    LoadCaseReviews = true,
                    LoadDocuments = true,
                    LoadRespondents = true,
                    LoadSolicitors = true,
                    LoadPoliceForces = true
                };

                var warrant = _warrantPresenter.GetWarrant(id, loader);

                return View(warrant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Warrant in Details method, for user {((CPrincipal)User).UserID}");
                ErrorModel model = new ErrorModel(2);
                model.ErrorMessage = ex.Message;
                TempData["ErrorModel"] = model;
                return RedirectToAction("IndexByModel", "Error", model ?? null);
            }

        }

        //
        // GET: /Warrant/Create
        public ActionResult Create()
        {
            ViewBag.caseStatusID = new SelectList(MemoryCollections.CaseStatusList.GetCaseStatusList().Where(x => x.Active == 1), "CaseStatusID", "Detail");
            ViewBag.divisions = new SelectList(MemoryCollections.DivisionsList.GetResultList().Where(x => x.Active == 1), "DivisionID", "Detail");
            ViewBag.protectiveMarkings = new SelectList(MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().Where(x => x.Active == 1), "ProtectiveMarkingID", "Detail");
            ViewBag.resultID = new SelectList(MemoryCollections.ResultsList.GetResultList().Where(x => x.Active == 1), "ResultID", "Detail");

            Warrant model = new Warrant();
            model.nextReviewDate = DateTime.Today.AddMonths(1);
            System.Security.Principal.IIdentity userIdentity = User.Identity;
            Tipstaff.CPrincipal thisUser = new CPrincipal(userIdentity);
            model.createdBy = thisUser.User.DisplayName;
            model.caseStatusID = 1;
            return View(model);
        }

        //
        // POST: /Warrant/Create

        [HttpPost]
        public ActionResult Create(Warrant warrant)
        {
            try
            {
                if (warrant.DateCirculated == null)
                {
                    ViewBag.caseStatusID = new SelectList(MemoryCollections.CaseStatusList.GetCaseStatusList().Where(x => x.Active == 1), "CaseStatusID", "Detail", warrant.caseStatusID);
                    ViewBag.divisions = new SelectList(MemoryCollections.DivisionsList.GetResultList().Where(x => x.Active == 1), "DivisionID", "Detail", warrant.Division.DivisionId);
                    ViewBag.protectiveMarkings = new SelectList(MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().Where(x => x.Active == 1), "ProtectiveMarkingID", "Detail", warrant.protectiveMarkingID);
                    ViewBag.resultID = new SelectList(MemoryCollections.ResultsList.GetResultList().Where(x => x.Active == 1), "ResultID", "Detail", warrant.resultID);

                    ViewBag.noDateCirculated = "Please enter the Date Circulated";
                    return View(warrant);
                }
                else
                {
                    warrant.createdOn = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        warrant.Discriminator = "Warrant";
                        _warrantPresenter.AddWarrant(warrant);
                        return RedirectToAction("Create", "Respondent", new { id = warrant.tipstaffRecordID });
                    }

                    ViewBag.caseStatusID = new SelectList(MemoryCollections.CaseStatusList.GetCaseStatusList().Where(x => x.Active == 1), "CaseStatusID", "Detail", warrant.caseStatusID);
                    ViewBag.divisions = new SelectList(MemoryCollections.DivisionsList.GetResultList().Where(x => x.Active == 1), "DivisionID", "Detail", warrant.Division.DivisionId);
                    ViewBag.protectiveMarkings = new SelectList(MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().Where(x => x.Active == 1), "ProtectiveMarkingID", "Detail", warrant.protectiveMarkingID);
                    ViewBag.resultID = new SelectList(MemoryCollections.ResultsList.GetResultList().Where(x => x.Active == 1), "ResultID", "Detail", warrant.resultID);
                    return View(warrant);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Warrant in Create with Model method, for user {((CPrincipal)User).UserID}");
                ErrorModel model = new ErrorModel(2);
                model.ErrorMessage = ex.Message;
                TempData["ErrorModel"] = model;
                return RedirectToAction("IndexByModel", "Error", model ?? null);
            }
        }

        //
        // GET: /Warrant/Edit/5
        public ActionResult Edit(string id)
        {
            try
            {
                Warrant warrant = _warrantPresenter.GetWarrant(id);
                if (warrant.caseStatus.Sequence > 3)
                {
                    TempData["UID"] = warrant.UniqueRecordID;
                    return RedirectToAction("ClosedFile", "Error");
                }
                ViewBag.caseStatusID = new SelectList(MemoryCollections.CaseStatusList.GetCaseStatusList().Where(x => x.Active == 1), "CaseStatusID", "Detail", warrant.caseStatusID);
                ViewBag.divisions = new SelectList(MemoryCollections.DivisionsList.GetResultList().Where(x => x.Active == 1), "DivisionID", "Detail", warrant.Division.DivisionId);
                ViewBag.protectiveMarkings = new SelectList(MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().Where(x => x.Active == 1), "ProtectiveMarkingID", "Detail", warrant.protectiveMarkingID);
                return View(warrant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Warrant in Edit method, for user {((CPrincipal)User).UserID}");
                ErrorModel model = new ErrorModel(2);
                model.ErrorMessage = ex.Message;
                TempData["ErrorModel"] = model;
                return RedirectToAction("IndexByModel", "Error", model ?? null);
            }
        }

        //
        // POST: /Warrant/Edit/5
        [HttpPost]
        public ActionResult Edit(Warrant warrant)
        {
            try
            {
                warrant.Division = MemoryCollections.DivisionsList.GetDivisionByID(warrant.Division.DivisionId);
                warrant.Respondents = _respondentPresenter.GetAllById(warrant.tipstaffRecordID);
                if (ModelState.IsValid)
                {
                    _warrantPresenter.UpdateWarrant(warrant);
                    return RedirectToAction("Details", "Warrant", new { id = warrant.tipstaffRecordID });
                }

                ViewBag.caseStatusID = new SelectList(MemoryCollections.CaseStatusList.GetCaseStatusList().Where(x => x.Active == 1), "CaseStatusID", "Detail", warrant.caseStatusID);
                ViewBag.divisions = new SelectList(MemoryCollections.DivisionsList.GetResultList().Where(x => x.Active == 1), "DivisionID", "Detail", warrant.Division?.DivisionId);
                ViewBag.protectiveMarkings = new SelectList(MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().Where(x => x.Active == 1), "ProtectiveMarkingID", "Detail", warrant.protectiveMarkingID);
                return View(warrant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Warrant in Edit with method, for user {((CPrincipal)User).UserID}");
                ErrorModel model = new ErrorModel(2);
                model.ErrorMessage = ex.Message;
                TempData["ErrorModel"] = model;
                return RedirectToAction("IndexByModel", "Error", model ?? null);
            }
        }

        public ActionResult EnterResult(string id)
        {
            TipstaffRecordResolutionModel model = new TipstaffRecordResolutionModel();
            model.tipstaffRecord = _tipstaffRecordPresenter.GetTipStaffRecord(id, new LazyLoader() { LoadRespondents = true });
            model.tipstaffRecordID = id;

            int respCount = model.tipstaffRecord.Respondents.Count();

            Dictionary<int, string> resp = new Dictionary<int, string>();
            for (int i = 0; i <= respCount; i++)
            {
                resp.Add(i, i.ToString());
            }
            model.prisonDict = resp;
            model.arrestDict = resp;

            if (model.tipstaffRecord.caseStatusID > 2 && model.tipstaffRecord.resultID != null)
            {
                TempData["UID"] = model.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }
            model.tipstaffRecordID = id;
            if (model.tipstaffRecord == null)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("Record for Warrant {0} cannot be loaded", id);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", errModel ?? null);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EnterResult(TipstaffRecordResolutionModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var tipstaffRecord = _tipstaffRecordPresenter.GetTipStaffRecord(model.tipstaffRecordID.ToString());
                    //////model.tipstaffRecord = db.TipstaffRecord.Find(model.tipstaffRecordID);
                    model.tipstaffRecord = tipstaffRecord;
                    model.tipstaffRecord.resultDate = DateTime.Now;
                    model.tipstaffRecord.DateExecuted = model.DateExecuted;
                    model.tipstaffRecord.resultID = model.resultID;
                    model.tipstaffRecord.resultEnteredBy = User.Identity.Name;
                    model.tipstaffRecord.prisonCount = model.pCount;
                    model.tipstaffRecord.arrestCount = model.aCount;
                    model.tipstaffRecord.caseStatusID = 3;
                    ////db.Entry(model.tipstaffRecord).State = EntityState.Modified;
                    ////db.SaveChanges();
                    _tipstaffRecordPresenter.UpdateTipstaffRecord(model.tipstaffRecord);
                    return RedirectToAction("Details", "Warrant", new { id = model.tipstaffRecordID });
                }
                catch (Exception ex)
                {
                    ErrorModel errModel = new ErrorModel(2);
                    errModel.ErrorMessage = genericFunctions.GetLowestError(ex);
                    TempData["ErrorModel"] = errModel;
                    return RedirectToAction("IndexByModel", "Error", errModel ?? null);
                }
            }
            return View(model);
        }
        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            DeleteWarrantViewModel model = new DeleteWarrantViewModel();
            //////model.Warrant = db.Warrants.Find(id);
            model.Warrant = _warrantPresenter.GetWarrant(id);
            model.deletedTipstaffRecord.TipstaffRecordID = id;
            if (model == null)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("Warrant {0} has been deleted, please raise a help desk call if you think this has been deleted in error.", id);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }

        //
        // POST: /Warrant/Delete/5

        [HttpPost, ActionName("Delete"), AuthorizeRedirect(Roles = "Admin")]
        public ActionResult DeleteConfirmed(DeleteWarrantViewModel model)
        {
            //////model.Warrant = db.Warrants.Find(model.deletedTipstaffRecord.TipstaffRecordID);
            model.Warrant = _warrantPresenter.GetWarrant(model.deletedTipstaffRecord.TipstaffRecordID.ToString());
            model.deletedTipstaffRecord.UniqueRecordID = model.Warrant.UniqueRecordID;
            //////db.Warrants.Remove(model.Warrant);

            _warrantPresenter.RemoveWarrant(model.Warrant);
            //////db.DeletedTipstaffRecords.Add(model.deletedTipstaffRecord);
            //////db.SaveChanges();
            //////_tipstaffRecordPresenter.AddDeletedTipstaffRecord(model.deletedTipstaffRecord);
            return RedirectToAction("Index", "Warrant");
        }

    }
}