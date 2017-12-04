using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Data;
using System.Data.Entity;
using Tipstaff.Presenters;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class NPOController : Controller
    {
        //private TipstaffDB db = myDBContextHelper.CurrentContext;
        //
        // GET: /NPO/Add
        private readonly IWarrantPresenter _warrantPresenter;
        private readonly IChildAbductionPresenter _childAbductionPresenter;
        private readonly ITipstaffRecordPresenter _tipstaffRecordPresenter;
        private readonly IChildPresenter _childPresenter;

        public NPOController(IWarrantPresenter warrantPresenter, IChildAbductionPresenter childAbductionPresenter, ITipstaffRecordPresenter tipstaffRecordPresenter, IChildPresenter childPresenter)
        {
            _warrantPresenter = warrantPresenter;
            _childAbductionPresenter = childAbductionPresenter;
            _tipstaffRecordPresenter = tipstaffRecordPresenter;
            _childPresenter = childPresenter;
        }


        public ActionResult Add(string id)
        {
            TipstaffNPO model = new TipstaffNPO();
            //////TipstaffRecord ts = db.TipstaffRecord.Find(id);

            TipstaffRecord ts = _tipstaffRecordPresenter.GetTipStaffRecord(id);
            model.tipstaffRecordID = id;
            model.UniqueRecordID = ts.UniqueRecordID;
            model.NPO = ts.NPO;
            return View(model);
        } 

        //
        // POST: /NPO/Add

        [HttpPost]
        public ActionResult Add(TipstaffNPO model)
        {
            if (ModelState.IsValid)
            {
                string controller = genericFunctions.TypeOfTipstaffRecord(model.tipstaffRecordID);
                if (controller == "Warrant")
                {
                    ////Warrant w = db.Warrants.Find(model.tipstaffRecordID);
                    Warrant w = _warrantPresenter.GetWarrant(model.tipstaffRecordID);
                    w.NPO = model.NPO;
                    ////db.Entry(w).State = EntityState.Modified;
                    ////db.SaveChanges();
                    _warrantPresenter.UpdateWarrant(w);
                    return RedirectToAction("Details", "Warrant", new { id = w.tipstaffRecordID });
                }
                else
                {
                    ////ChildAbduction ca = db.ChildAbductions.Find(model.tipstaffRecordID);
                    ChildAbduction ca = _childAbductionPresenter.GetChildAbduction(model.tipstaffRecordID);
                    ca.NPO = model.NPO;
                    ////db.Entry(ca).State = EntityState.Modified;
                    ////db.SaveChanges();
                    _childAbductionPresenter.UpdateChildAbduction(ca);
                    return RedirectToAction("Details", "ChildAbduction", new { id = ca.tipstaffRecordID });
                }
            }
            else
                return View(model);
        }

        //public PartialViewResult ListPNCIDAndNPOByRecord(int id)
        //{
        //    TipstaffRecord w = db.TipstaffRecord.Find(id);
        //    ListPNCIDsNPO model = new ListPNCIDsNPO();
        //    model.npo = new TipstaffNPO();
        //    model.npo.tipstaffRecordID = w.tipstaffRecordID;
        //    model.npo.NPO = w.NPO;
        //    model.Respondents = w.Respondents.Where(r=>r.PNCID != null).ToList();
        //    if (genericFunctions.TypeOfTipstaffRecord(id) != "Warrant")
        //        model.children = db.Children.Where(c => c.tipstaffRecordID == id && c.PNCID != null).ToList();
        //    return PartialView("_ListPNCIDAndNPOByRecord", model);
        //}

        public PartialViewResult ListPNCIDAndNPOByRecord(string id)
        {
            //TipstaffRecord w = db.TipstaffRecord.Find(id);
            TipstaffRecord w = _tipstaffRecordPresenter.GetTipStaffRecord(id);
            ListPNCIDsNPO model = new ListPNCIDsNPO();
            model.npo = new TipstaffNPO();
            model.npo.tipstaffRecordID = w.tipstaffRecordID;
            model.npo.NPO = w.NPO;
            model.Respondents = w.Respondents.Where(r => r.PNCID != null).ToList();
            if (genericFunctions.TypeOfTipstaffRecord(id) != "Warrant")
            {
                /// model.children = db.Children.Where(c => c.tipstaffRecordID == id && c.PNCID != null).ToList();
                model.children = _childPresenter.GetAllChildren().Where(c => c.tipstaffRecordID == id && c.PNCID != null).ToList();
            }
            return PartialView("_ListPNCIDAndNPOByRecord", model);
        }
    }
}
