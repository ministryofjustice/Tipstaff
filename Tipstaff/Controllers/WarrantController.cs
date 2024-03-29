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
using System.Web.Security;
using System.IO;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class WarrantController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;

        public ViewResult Index(WarrantListViewModel model)
        {
            IQueryable<Warrant> TRs = myDBContextHelper.CurrentContext.Warrants;
            model.TotalRecordCount = TRs.Count();
            if (!model.includeFinal)
            {
                TRs = TRs.Where(x => x.resultID == null);
            }
            if (model.caseStatusID > -1)
            {
                TRs = TRs.Where(w => w.caseStatusID == model.caseStatusID);
            }
            if (model.divisionID > -1)
            {
                TRs = TRs.Where(w => w.divisionID == model.divisionID);
            }
            if (!string.IsNullOrEmpty(model.caseNumberContains))
            {
                TRs = TRs.Where(w => w.caseNumber.Contains(model.caseNumberContains.ToUpper()));
            }
            if (!string.IsNullOrEmpty(model.respondentNameContains))
            {
                //TRs = TRs.Where(w=>w.Respondents.OrderByDescending(c => c.dateOfBirth).ThenBy(c => c.respondentID).FirstOrDefault().nameLast.ToUpper().Contains(model.childNameContains.ToUpper()));#
                TRs = TRs.Where(w => w.RespondentName.Contains(model.respondentNameContains.ToUpper()));
            }
            switch (model.sortOrder)
            {
                case "created asc":
                    model.Warrants = TRs.OrderBy(a => a.createdOn).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "created desc":
                    model.Warrants = TRs.OrderByDescending(a => a.createdOn).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "uniqueid asc":
                    model.Warrants = TRs.OrderBy(a => a.tipstaffRecordID).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "uniqueid desc":
                    model.Warrants = TRs.OrderByDescending(a => a.tipstaffRecordID).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "casenumber asc":
                    model.Warrants = TRs.OrderBy(a => a.caseNumber).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "casenumber desc":
                    model.Warrants = TRs.OrderByDescending(a => a.caseNumber).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "division asc":
                    model.Warrants = TRs.OrderBy(a => a.division.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "division desc":
                    model.Warrants = TRs.OrderByDescending(a => a.division.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "reviewDate asc":
                    model.Warrants = TRs.OrderBy(a => a.nextReviewDate).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "reviewDate desc":
                    model.Warrants = TRs.OrderByDescending(a => a.nextReviewDate).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "displayName asc":
                    model.Warrants = TRs.OrderBy(a => a.RespondentName).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    /*
                     * This cannot work in SQL 2000 - SQL>LINQ and LINQ>Entities build SQl queries with APPLY operators
                     * with APPLY operators these do not work in SQL 2000 - restore if backend moved to 2005 or newer
                     */
                    //model.Warrants = TRs.OrderBy(a => a.Respondents.FirstOrDefault().nameLast).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "displayName desc":
                    model.Warrants = TRs.OrderByDescending(a => a.RespondentName).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    /*
                    * This cannot work in SQL 2000 - SQL>LINQ and LINQ>Entities build SQl queries with APPLY operators
                    * with APPLY operators these do not work in SQL 2000 - restore if backend moved to 2005 or newer
                    */
                    //model.Warrants = TRs.OrderByDescending(a => a.Respondents.OrderBy(b=>b.nameLast).Take(1)).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "expiryDate asc":
                    model.Warrants = TRs.OrderBy(a => a.expiryDate).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "expiryDate desc":
                    model.Warrants = TRs.OrderByDescending(a => a.expiryDate).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "protMark asc":
                    model.Warrants = TRs.OrderBy(a => a.protectiveMarking.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "protMark desc":
                    model.Warrants = TRs.OrderByDescending(a => a.protectiveMarking.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "caseStatus asc":
                    model.Warrants = TRs.OrderBy(a => a.caseStatus.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "caseStatus desc":
                    model.Warrants = TRs.OrderByDescending(a => a.caseStatus.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "result asc":
                    model.Warrants = TRs.OrderBy(a => a.result.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "result desc":
                    model.Warrants = TRs.OrderByDescending(a => a.result.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "resultEnter asc":
                    model.Warrants = TRs.OrderBy(a => a.resultEnteredBy).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                case "resultEnter desc":
                    model.Warrants = TRs.OrderByDescending(a => a.resultEnteredBy).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;

                default:
                    model.Warrants = TRs.OrderBy(a => a.tipstaffRecordID).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
                    break;
            }
            return View(model);
        }

        //
        // GET: /Warrant/Details/5
        public ViewResult Details(int id)
        {
            Warrant warrant = db.Warrants.Find(id);
            return View(warrant);
        }

        //
        // GET: /Warrant/Create
        public ActionResult Create()
        {
            ViewBag.protectiveMarkings = new SelectList(db.ProtectiveMarkings.Where(x => x.active == true), "protectiveMarkingID", "Detail");
            ViewBag.divisions = new SelectList(db.Divisions.Where(x => x.active == true), "divisionID", "Detail");
            ViewBag.resultID = new SelectList(db.Results.Where(x => x.active == true), "resultID", "Detail");
            ViewBag.caseStatusID = new SelectList(db.CaseStatuses.Where(x => x.active == true), "caseStatusID", "Detail");
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
            if (warrant.DateCirculated == null)
            {
                ViewBag.protectiveMarkings = new SelectList(db.ProtectiveMarkings.Where(x => x.active == true), "protectiveMarkingID", "Detail", warrant.protectiveMarkingID);
                ViewBag.divisions = new SelectList(db.Divisions.Where(x => x.active == true), "divisionID", "Detail", warrant.divisionID);
                ViewBag.resultID = new SelectList(db.Results.Where(x => x.active == true), "resultID", "Detail", warrant.resultID);
                ViewBag.caseStatusID = new SelectList(db.CaseStatuses.Where(x => x.active == true), "caseStatusID", "Detail", warrant.caseStatusID);
                ViewBag.noDateCirculated = "Please enter the Date Circulated";
                return View(warrant);
            }
            else
            {
                warrant.createdOn = DateTime.Now;
                if (ModelState.IsValid)
                {
                    db.Warrants.Add(warrant);
                    db.SaveChanges();
                    return RedirectToAction("Create", "Respondent", new { id = warrant.tipstaffRecordID });
                }

                ViewBag.protectiveMarkings = new SelectList(db.ProtectiveMarkings.Where(x => x.active == true), "protectiveMarkingID", "Detail", warrant.protectiveMarkingID);
                ViewBag.divisions = new SelectList(db.Divisions.Where(x => x.active == true), "divisionID", "Detail", warrant.divisionID);
                ViewBag.resultID = new SelectList(db.Results.Where(x => x.active == true), "resultID", "Detail", warrant.resultID);
                ViewBag.caseStatusID = new SelectList(db.CaseStatuses.Where(x => x.active == true), "caseStatusID", "Detail", warrant.caseStatusID);
                return View(warrant);
            }
        }

        //
        // GET: /Warrant/Edit/5
        public ActionResult Edit(int id)
        {
            Warrant warrant = db.Warrants.Find(id);
            if (warrant.caseStatus.sequence > 3)
            {
                TempData["UID"] = warrant.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }
            ViewBag.protectiveMarkings = new SelectList(db.ProtectiveMarkings.Where(x => x.active == true), "protectiveMarkingID", "Detail", warrant.protectiveMarkingID);
            ViewBag.divisions = new SelectList(db.Divisions.Where(x => x.active == true), "divisionID", "Detail", warrant.divisionID);
            ViewBag.caseStatusID = new SelectList(db.CaseStatuses.Where(x => x.active == true), "caseStatusID", "Detail", warrant.caseStatusID);
            return View(warrant);
        }

        //
        // POST: /Warrant/Edit/5

        [HttpPost]
        public ActionResult Edit(Warrant warrant)
        {
            warrant.division = db.Divisions.Find(warrant.divisionID);
            warrant.Respondents = db.Respondents.Where(r => r.tipstaffRecordID == warrant.tipstaffRecordID).ToList();
            if (ModelState.IsValid)
            {
                db.Entry(warrant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Warrant", new { id = warrant.tipstaffRecordID });
            }
            ViewBag.protectiveMarkings = new SelectList(db.ProtectiveMarkings.Where(x => x.active == true), "protectiveMarkingID", "Detail", warrant.protectiveMarkingID);
            ViewBag.divisions = new SelectList(db.Divisions.Where(x => x.active == true), "divisionID", "Detail", warrant.divisionID);
            ViewBag.caseStatusID = new SelectList(db.CaseStatuses.Where(x => x.active == true), "caseStatusID", "Detail", warrant.caseStatusID);
            return View(warrant);
        }

        public ActionResult EnterResult(int id)
        {
            TipstaffRecordResolutionModel model = new TipstaffRecordResolutionModel(id);
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
                    model.tipstaffRecord = db.TipstaffRecord.Find(model.tipstaffRecordID);
                    model.tipstaffRecord.resultDate = DateTime.Now;
                    model.tipstaffRecord.DateExecuted = model.DateExecuted;
                    model.tipstaffRecord.resultID = model.resultID;
                    model.tipstaffRecord.resultEnteredBy = User.Identity.Name;
                    model.tipstaffRecord.prisonCount = model.pCount;
                    model.tipstaffRecord.arrestCount = model.aCount;
                    model.tipstaffRecord.caseStatusID = 3;
                    db.Entry(model.tipstaffRecord).State = EntityState.Modified;
                    db.SaveChanges();
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
        public ActionResult Delete(int id)
        {
            DeleteWarrantViewModel model = new DeleteWarrantViewModel();
            model.Warrant = db.Warrants.Find(id);
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
            //model.Warrant = db.Warrants.Find(model.deletedTipstaffRecord.TipstaffRecordID);
            //db.Warrants.Remove(model.Warrant);

            var w = db.Warrants.Find(model.deletedTipstaffRecord.TipstaffRecordID);

            foreach (var r in w.Respondents.ToList())
            {
                db.Entry(r).State = EntityState.Deleted;
            }
            foreach (var a in w.addresses.ToList())
            {
                db.Entry(a).State = EntityState.Deleted;
            }
            foreach (var an in w.AttendanceNotes.ToList())
            {
                db.Entry(an).State = EntityState.Deleted;
            }
            foreach (var cr in w.caseReviews.ToList())
            {
                db.Entry(cr).State = EntityState.Deleted;
            }
            foreach (var d in w.Documents.ToList())
            {
                db.Entry(d).State = EntityState.Deleted;
            }
            foreach (var pass in w.Passports.ToList())
            {
                db.Entry(pass).State = EntityState.Deleted;
            }
            foreach (var trs in w.LinkedSolicitors.ToList())
            {
                db.Entry(trs).State = EntityState.Deleted;
            }
            foreach (var p in w.policeForces.ToList())
            {
                db.Entry(p).State = EntityState.Deleted;
            }
            db.Entry(w).State = EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("Index", "Warrant");
        }
    }
}