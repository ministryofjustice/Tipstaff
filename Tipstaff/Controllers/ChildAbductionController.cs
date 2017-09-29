﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using PagedList;

using Tipstaff.Models;
using System.Xml.Linq;
using System.IO;
using Tipstaff.Logger;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class ChildAbductionController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;

        private readonly ICloudWatchLogger _logger;
        private readonly ITipstaffRecordRepository _tipstaffRecordRepository;

        public ChildAbductionController(ITipstaffRecordRepository tipstaffRecordRepository)
        {
            _tipstaffRecordRepository = tipstaffRecordRepository;
          ///  _logger = telemetryLogger;
          ///  _
        }
        //
        // GET: /ChildAbduction/

        public ViewResult Index(ChildAbductionListViewModel model)
        {
            int pageSize = Int32.Parse(ConfigurationManager.AppSettings["pageSize"]);
            IQueryable<ChildAbduction> TRs = myDBContextHelper.CurrentContext.ChildAbductions.Include(c => c.protectiveMarking).Include(c => c.result).Include(c => c.caseStatus);
            model.TotalRecordCount = TRs.Count();
            if (!model.includeFinal)
            {
                TRs = TRs.Where(x => x.resultID == null);
            }
            if (model.caseStatusID > -1)
            {
                TRs = TRs.Where(w => w.caseStatusID == model.caseStatusID);
            }
            if (model.caOrderTypeID > -1)
            {
                TRs = TRs.Where(w => w.caOrderTypeID == model.caOrderTypeID);
            }
            if (!string.IsNullOrEmpty(model.childNameContains))
            {
                //TRs = TRs.Where(w=>w.children.OrderByDescending(c => c.dateOfBirth).ThenBy(c => c.childID).FirstOrDefault().nameLast.ToUpper().Contains(model.childNameContains.ToUpper()));#
                TRs = TRs.Where(w => w.EldestChild.Contains(model.childNameContains.ToUpper()));
            }
            model.FilteredRecordCount = TRs.Count();

            //IOrderedQueryable<ChildAbduction> TRs = ((IOrderedQueryable<ChildAbduction>)CAs);
            switch (model.sortOrder)
            {
                case "reviewDate asc":
                    TRs = TRs.OrderBy(a => a.nextReviewDate).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "reviewDate desc":
                    TRs = TRs.OrderByDescending(a => a.nextReviewDate).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "sentSCD asc":
                    TRs = TRs.OrderBy(a => a.sentSCD26).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "sentSCD desc":
                    TRs = TRs.OrderByDescending(a => a.sentSCD26).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "orderDated asc":
                    TRs = TRs.OrderBy(a => a.orderDated).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "orderDated desc":
                    TRs = TRs.OrderByDescending(a => a.orderDated).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "orderRecd asc":
                    TRs = TRs.OrderBy(a => a.orderReceived).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "orderRecd desc":
                    TRs = TRs.OrderByDescending(a => a.orderReceived).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "officer asc":
                    TRs = TRs.OrderBy(a => a.officerDealing).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "officer desc":
                    TRs = TRs.OrderByDescending(a => a.officerDealing).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "orderType asc":
                    TRs = TRs.OrderBy(a => a.caOrderType.Detail).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "orderType desc":
                    TRs = TRs.OrderByDescending(a => a.caOrderType.Detail).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "eldestChild asc":
                    TRs = TRs.OrderBy(a => a.EldestChild).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "eldestChild desc":
                    TRs = TRs.OrderByDescending(a => a.EldestChild).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "respCount asc":
                    TRs = TRs.OrderBy(a => a.Respondents.Count()).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "respCount desc":
                    TRs = TRs.OrderByDescending(a => a.Respondents.Count()).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "uniqueid asc":
                    TRs = TRs.OrderBy(a => a.tipstaffRecordID).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "uniqueid desc":
                    TRs = TRs.OrderByDescending(a => a.tipstaffRecordID).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "created asc":
                    TRs = TRs.OrderBy(a => a.createdOn).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "created desc":
                    TRs = TRs.OrderByDescending(a => a.createdOn).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "protMark asc":
                    TRs = TRs.OrderBy(a => a.protectiveMarking.Detail).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "protMark desc":
                    TRs = TRs.OrderByDescending(a => a.protectiveMarking.Detail).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "caseStatus asc":
                    TRs = TRs.OrderBy(a => a.caseStatus.Detail).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "caseStatus desc":
                    TRs = TRs.OrderByDescending(a => a.caseStatus.Detail).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "result asc":
                    TRs = TRs.OrderBy(a => a.result.Detail).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "result desc":
                    TRs = TRs.OrderByDescending(a => a.result.Detail).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "resultEnter asc":
                    TRs = TRs.OrderBy(a => a.resultEnteredBy).ThenBy(b => b.tipstaffRecordID);
                    break;
                case "resultEnter desc":
                    TRs = TRs.OrderByDescending(a => a.resultEnteredBy).ThenBy(b => b.tipstaffRecordID);
                    break;
                default:
                    TRs = TRs.OrderBy(a => a.tipstaffRecordID).ThenBy(b => b.tipstaffRecordID);
                    break;
            }
            model.ChildAbductions = TRs.ToPagedList(model.page, pageSize);
            return View(model);
        }

        //
        // GET: /ChildAbduction/Details/5

        public ViewResult Details(int id)
        {
            ChildAbduction childabduction = db.ChildAbductions.Find(id);
            return View(childabduction);
        }

        //
        // GET: /ChildAbduction/Create

        public ActionResult Create()
        {
            //ViewBag.protectiveMarkingList = new SelectList(db.ProtectiveMarkings.Where(x=>x.active==true).ToList(), "protectiveMarkingID", "Detail");
            //ViewBag.childAbductionCaseStatusList = new SelectList(db.CaseStatuses.Where(x => x.active == true).OrderBy(x=>x.sequence), "caseStatusID", "Detail");
            //ViewBag.caOrderTypeID = new SelectList(db.CAOrderTypes.Where(x => x.active == true), "caOrderTypeID", "Detail");
            ViewBag.caOrderTypeID = new SelectList(MemoryCollections.CaOrderTypeList.GetOrderTypeList().Where(x => x.Active == 1), "CAOrderTypeID", "Detail");
            ViewBag.childAbductionCaseStatusList = new SelectList(MemoryCollections.CaseStatusList.GetCaseStatusList().Where(x => x.Active == 1).OrderBy(x => x.Sequence), "CaseStatusID", "Detail");
            ViewBag.protectiveMarkingList = new SelectList(MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().Where(x => x.Active == 1).ToList(), "ProtectiveMarkingID", "Detail");

            ChildAbduction model = new ChildAbduction();
            model.nextReviewDate = DateTime.Today.AddMonths(1);
            System.Security.Principal.IIdentity userIdentity = User.Identity;
            Tipstaff.CPrincipal thisUser = new CPrincipal(userIdentity);
            model.createdBy = thisUser.User.DisplayName;
            return View(model);
        } 

        //
        // POST: /ChildAbduction/Create

        [HttpPost]
        public ActionResult Create(ChildAbduction childabduction)
        {
            if (ModelState.IsValid)
            {
                ////db.TipstaffRecord.Add(childabduction);
                ////db.SaveChanges();
                _tipstaffRecordRepository.Add(new Services.DynamoTables.TipstaffRecord()
                {
                       ArrestCount = childabduction.arrestCount,
                       EldestChild = childabduction.EldestChild,
                       Discriminator = "ChildAbduction",
                       CreatedBy = childabduction.createdBy,
                       OfficerDealing = childabduction.officerDealing,
                       NPO = childabduction.NPO,
                       PrisonCount = childabduction.prisonCount,
                       SentSCD26 = childabduction.sentSCD26,
                       ResultDate = childabduction.resultDate.Value,
                       ResultEnteredBy = childabduction.resultEnteredBy,
                       OrderReceived = childabduction.orderReceived,
                       OrderDated = childabduction.orderDated
                });
                

                return RedirectToAction("Create","Child", new { id = childabduction.tipstaffRecordID , initial=true });   
            }

            //ViewBag.protectiveMarkingID = new SelectList(db.ProtectiveMarkings.Where(x => x.active == true), "protectiveMarkingID", "Detail", childabduction.protectiveMarkingID);
           // ViewBag.caseStatusID = new SelectList(db.CaseStatuses.Where(x => x.active == true), "caseStatusID", "Detail", childabduction.caseStatusID);
            //ViewBag.caOrderTypeID = new SelectList(db.CAOrderTypes.Where(x => x.active == true), "caOrderTypeID", "Detail", childabduction.caOrderTypeID);
            ViewBag.caOrderTypeID = new SelectList(MemoryCollections.CaOrderTypeList.GetOrderTypeList().Where(x => x.Active == 1), "CAOrderTypeID", "Detail", childabduction.caOrderTypeID);
            ViewBag.caseStatusID = new SelectList(MemoryCollections.CaseStatusList.GetCaseStatusList().Where(x => x.Active == 1), "CaseStatusID", "Detail", childabduction.caseStatusID);
            ViewBag.protectiveMarkingID = new SelectList(MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().Where(x => x.Active == 1), "ProtectiveMarkingID", "Detail", childabduction.protectiveMarkingID);
            return View(childabduction);
        }
        
        //
        // GET: /ChildAbduction/Edit/5
 
        public ActionResult Edit(int id)
        {
            ChildAbduction childabduction = db.ChildAbductions.Find(id);
            if (childabduction.caseStatus.Sequence > 3)  //.caseStatus.sequence > 3)
            {
                TempData["UID"] = childabduction.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }
            //ViewBag.protectiveMarkingID = new SelectList(db.ProtectiveMarkings.Where(x => x.active == true), "protectiveMarkingID", "Detail", childabduction.protectiveMarkingID);
            //ViewBag.caseStatusID = new SelectList(db.CaseStatuses.Where(x => x.active == true), "caseStatusID", "Detail", childabduction.caseStatusID);
            //ViewBag.caOrderTypeID = new SelectList(db.CAOrderTypes.Where(x => x.active == true), "caOrderTypeID", "Detail", childabduction.caOrderTypeID);
            ViewBag.caOrderTypeID = new SelectList(MemoryCollections.CaOrderTypeList.GetOrderTypeList().Where(x => x.Active == 1), "CAOrderTypeID", "Detail", childabduction.caOrderTypeID);
            ViewBag.caseStatusID = new SelectList(MemoryCollections.CaseStatusList.GetCaseStatusList().Where(x => x.Active == 1), "CaseStatusID", "Detail", childabduction.caseStatusID);
            ViewBag.protectiveMarkingID = new SelectList(MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().Where(x => x.Active == 1), "ProtectiveMarkingID", "Detail", childabduction.protectiveMarkingID);
            return View(childabduction);
        }

        //
        // POST: /ChildAbduction/Edit/5

        [HttpPost]
        public ActionResult Edit(ChildAbduction childabduction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(childabduction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "ChildAbduction", new { id = childabduction.tipstaffRecordID });
            }
            //ViewBag.protectiveMarkingID = new SelectList(db.ProtectiveMarkings.Where(x => x.active == true), "protectiveMarkingID", "Detail", childabduction.protectiveMarkingID);
            //ViewBag.caseStatusID = new SelectList(db.CaseStatuses.Where(x => x.active == true), "caseStatusID", "Detail", childabduction.caseStatusID);
            //ViewBag.caOrderTypeID = new SelectList(db.CAOrderTypes.Where(x => x.active == true), "caOrderTypeID", "Detail", childabduction.caOrderTypeID);
            ViewBag.caseStatusID = new SelectList(MemoryCollections.CaseStatusList.GetCaseStatusList().Where(x => x.Active == 1), "CaseStatusID", "Detail", childabduction.caseStatusID);
            ViewBag.caOrderTypeID = new SelectList(MemoryCollections.CaOrderTypeList.GetOrderTypeList().Where(x => x.Active == 1), "CAOrderTypeID", "Detail", childabduction.caOrderTypeID);
            ViewBag.protectiveMarkingID = new SelectList(MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingsList().Where(x => x.Active == 1), "ProtectiveMarkingID", "Detail", childabduction.protectiveMarkingID);

            return View(childabduction);
        }

        public ActionResult EnterResult(int id)
        {
            TipstaffRecordResolutionModel model = new TipstaffRecordResolutionModel(id);
            if (model.tipstaffRecord.caseStatusID > 2 &&  model.tipstaffRecord.resultID!=null)
            {
                TempData["UID"] = model.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }
            model.tipstaffRecordID = id;
            if (model.tipstaffRecord == null)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("Record for Child Abduction {0} cannot be loaded", id);
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
                    model.tipstaffRecord = db.TipstaffRecord.Find(model.tipstaffRecordID);
                    model.tipstaffRecord.nextReviewDate = DateTime.Today.AddDays(1);
                    model.tipstaffRecord.resultDate = DateTime.Now;
                    model.tipstaffRecord.DateExecuted = model.DateExecuted;
                    model.tipstaffRecord.resultID = model.resultID;
                    model.tipstaffRecord.resultEnteredBy = User.Identity.Name;
                    model.tipstaffRecord.prisonCount = model.pCount;
                    model.tipstaffRecord.arrestCount = model.aCount;
                    model.tipstaffRecord.caseStatusID = 3;
                    db.Entry(model.tipstaffRecord).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", "ChildAbduction", new { id = model.tipstaffRecordID });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Exception in ChildAbductionController in EnterResult method, for user {((CPrincipal)User).UserID}");
                    ErrorModel errModel = new ErrorModel(2);
                    errModel.ErrorMessage = genericFunctions.GetLowestError(ex);
                    TempData["ErrorModel"] = errModel;
                    return RedirectToAction("IndexByModel", "Error", errModel ?? null);
                }
            }
            return View(model);
        }

        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            DeleteChildAbductionViewModel model = new DeleteChildAbductionViewModel();
            model.ChildAbduction = db.ChildAbductions.Find(id);
            model.deletedTipstaffRecord.TipstaffRecordID=id;
            if (model.ChildAbduction == null)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("Child Abduction {0} has been deleted, please raise a help desk call if you think this has been deleted in error.", id);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }

        //
        // POST: /Warrant/Delete/5

        [HttpPost, ActionName("Delete"), AuthorizeRedirect(Roles = "Admin")]
        public ActionResult DeleteConfirmed(DeleteChildAbductionViewModel model)
        {
            model.ChildAbduction = db.ChildAbductions.Find(model.deletedTipstaffRecord.TipstaffRecordID);
            model.deletedTipstaffRecord.UniqueRecordID = model.ChildAbduction.UniqueRecordID;
            db.ChildAbductions.Remove(model.ChildAbduction);
            db.DeletedTipstaffRecords.Add(model.deletedTipstaffRecord);
            db.SaveChanges();
            return RedirectToAction("Index", "ChildAbduction");
        }

    }
}