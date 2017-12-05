using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tipstaff.Infrastructure.Services;
using Tipstaff.Models;
using Tipstaff.Presenters;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class CaseReviewController : Controller
    {
        private readonly ITipstaffRecordPresenter _tipstaffRecordPresenter;
        private readonly ICaseReviewPresenter _caseReviewPresenter;
        private readonly IGuidGenerator _guidGenerator;

        public CaseReviewController(ITipstaffRecordPresenter tipstaffRecordPresenter, 
                                    ICaseReviewPresenter caseReviewPresenter, IGuidGenerator guidGenerator)
        {
            _tipstaffRecordPresenter = tipstaffRecordPresenter;
            _caseReviewPresenter = caseReviewPresenter;
            _guidGenerator = guidGenerator;
        }
        
        // GET: /CaseReview/
        public ActionResult Create(string id)
        {
            CaseReviewCreation model = new CaseReviewCreation();
            ////model.CaseReview.tipstaffRecord = db.TipstaffRecord.Find(id);
            model.CaseReview.tipstaffRecord = _tipstaffRecordPresenter.GetTipStaffRecord(id);
            if (model.CaseReview.tipstaffRecord.caseStatus.Sequence > 3)
            {
                TempData["UID"] = model.CaseReview.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }
            model.CaseReview.tipstaffRecordID = id;
            model.CaseReview.reviewDate = DateTime.Today;
            model.CaseReview.nextReviewDate = DateTime.Today.AddMonths(1);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CaseReviewCreation model)
        {
            //do stuff to save CaseReview
            //Add event?
            if (ModelState.IsValid)
            {
                //if (genericFunctions.isTipstaffRecordChildAbduction){
                ////TipstaffRecord tr = db.TipstaffRecord.Find(model.CaseReview.tipstaffRecordID);
                TipstaffRecord tr = _tipstaffRecordPresenter.GetTipStaffRecord(model.CaseReview.tipstaffRecordID);
                ////tr.caseReviews.Add(model.CaseReview);
                model.CaseReview.tipstaffRecordID = tr.tipstaffRecordID;
                model.CaseReview.caseReviewID = _guidGenerator.GenerateTimeBasedGuid().ToString();
               
                _caseReviewPresenter.Add(model.CaseReview);


                if (model.CaseReview.caseReviewStatus.CaseReviewStatusId == 2 || model.CaseReview.caseReviewStatus.CaseReviewStatusId == 3)
                {
                    tr.caseStatusID = model.CaseReview.caseReviewStatus.CaseReviewStatusId + 1;
                }
                else
                {
                    tr.caseStatusID = model.CaseStatusID;
                }
                if (model.CaseReview.nextReviewDate != null)
                {
                    tr.nextReviewDate = model.CaseReview.nextReviewDate;
                }
                //////db.SaveChanges();
                _tipstaffRecordPresenter.UpdateTipstaffRecord(tr);

                if (model.CaseReview?.caseReviewStatus?.CaseReviewStatusId == 2)
                {
                    //user picked file closed, so get reasons...
                    return RedirectToAction("EnterResult", genericFunctions.TypeOfTipstaffRecord(tr), new { id = model.CaseReview.tipstaffRecordID });
                }
                return RedirectToAction("Details", genericFunctions.TypeOfTipstaffRecord(tr), new { id = model.CaseReview.tipstaffRecordID });
            }
            return View(model);
        }
        //public ActionResult Create(int id)
        //{
        //    CaseReview caseReview = new CaseReview();
        //    caseReview.tipstaffRecord= db.TipstaffRecord.Find(id);
        //    if (caseReview.tipstaffRecord.caseStatus.sequence > 3)
        //    {
        //        TempData["UID"] = caseReview.tipstaffRecord.UniqueRecordID;
        //        return RedirectToAction("ClosedFile", "Error");
        //    }
        //    caseReview.tipstaffRecordID = id;
        //    caseReview.reviewDate = DateTime.Today;
        //    caseReview.nextReviewDate = DateTime.Today.AddMonths(1);
        //    ViewBag.caseReviewStatusID = new SelectList(db.CaseReviewStatuses.Where(c => c.active == true), "caseReviewStatusID", "Detail");
        //    return View(caseReview);
        //}
        //[HttpPost]
        //public ActionResult Create(CaseReview caseReview)
        //{
        //    //do stuff to save CaseReview
        //    //Add event?
        //    if (ModelState.IsValid)
        //    {
        //        //if (genericFunctions.isTipstaffRecordChildAbduction){
        //        TipstaffRecord tr = db.TipstaffRecord.Find(caseReview.tipstaffRecordID);
        //        tr.caseReviews.Add(caseReview);
        //        tr.caseStatusID = caseReview.caseReviewStatusID + 1;
        //        if (caseReview.nextReviewDate!=null) {
        //            tr.nextReviewDate = caseReview.nextReviewDate;
        //        }
        //        db.SaveChanges();
        //        if (caseReview.caseReviewStatusID == 2)
        //        {
        //            //user picked file closed, so get reasons...
        //            return RedirectToAction("EnterResult", genericFunctions.TypeOfTipstaffRecord(tr), new { id = caseReview.tipstaffRecordID });
        //        }
        //        return RedirectToAction("Details", genericFunctions.TypeOfTipstaffRecord(tr), new { id = caseReview.tipstaffRecordID });
        //    }
        //    caseReview.tipstaffRecord= db.TipstaffRecord.Find(caseReview.tipstaffRecordID);
        //    caseReview.tipstaffRecordID = caseReview.tipstaffRecordID;
        //    ViewBag.caseReviewStatusID = new SelectList(db.CaseReviewStatuses.Where(c => c.active == true), "caseReviewStatusID", "Detail");
        //    return View(caseReview);
        //}

        public PartialViewResult Outstanding()
        {
            OutstandingCaseReviewViewModel model = new OutstandingCaseReviewViewModel();
            DateTime WeekAway = DateTime.Today.AddDays(7);
            var tipstaffRecords = _tipstaffRecordPresenter.GetAll();
            //////model.DueWithinWeekCaseReviews = db.TipstaffRecord.Where(w => w.result==null && w.nextReviewDate <= WeekAway && w.nextReviewDate > DateTime.Today).OrderBy(w => w.nextReviewDate).ThenBy(y => y.tipstaffRecordID).ToList();
            //////model.OverdueCaseReviews = db.TipstaffRecord.Where(w => w.result==null && w.nextReviewDate < DateTime.Today).OrderBy(w => w.nextReviewDate).ThenBy(y=>y.tipstaffRecordID).ToList();
            //////model.DueTodayCaseReviews = db.TipstaffRecord.Where(w => w.result==null && w.nextReviewDate == DateTime.Today).OrderBy(w => w.nextReviewDate).ThenBy(y => y.tipstaffRecordID).ToList();
            model.DueWithinWeekCaseReviews = tipstaffRecords.Where(w => w.result == null && w.nextReviewDate <= WeekAway && w.nextReviewDate > DateTime.Today).OrderBy(w => w.nextReviewDate).ThenBy(y => y.tipstaffRecordID).ToList();
            model.OverdueCaseReviews = tipstaffRecords.Where(w => w.result == null && w.nextReviewDate < DateTime.Today).OrderBy(w => w.nextReviewDate).ThenBy(y => y.tipstaffRecordID).ToList();
            model.DueTodayCaseReviews = tipstaffRecords.Where(w => w.result == null && w.nextReviewDate == DateTime.Today).OrderBy(w => w.nextReviewDate).ThenBy(y => y.tipstaffRecordID).ToList();


            return PartialView("_OutstandingCaseReviews",model);
        }
        public PartialViewResult ListCaseReviewsByRecord(string id, int? page)
        {
            ////TipstaffRecord w = db.TipstaffRecord.Find(id);

            TipstaffRecord w = _tipstaffRecordPresenter.GetTipStaffRecord(id);

            ListCaseReviewsByTipstaffRecord model = new ListCaseReviewsByTipstaffRecord();
            model.tipstaffRecordID = w.tipstaffRecordID;
            model.CaseReviews = w.caseReviews.OrderByDescending(d => d.reviewDate).ToXPagedList<CaseReview>(page ?? 1, 8);
            return PartialView("_ListCaseReviewsByRecord", model);
        }
    }
}
