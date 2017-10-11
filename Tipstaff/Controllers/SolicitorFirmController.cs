using System.Data;
using System.Linq;
using System.Web.Mvc;
using Tipstaff.Models;
using Tipstaff.Presenters;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class SolicitorFirmController : Controller
    {
        private readonly ISolicitorFirmsPresenter _solicitorFirmsPresenter;
        private readonly ITipstaffRecordPresenter _tipstaffRecordPresenter;

        public SolicitorFirmController(ISolicitorFirmsPresenter solicitorFirmsPresenter, ITipstaffRecordPresenter tipstaffRecordPresenter)
        {
            _solicitorFirmsPresenter = solicitorFirmsPresenter;
            _tipstaffRecordPresenter = tipstaffRecordPresenter;
        }

        public ViewResult Details(string solicitorFirmID, string tipstaffRecordID)
        {
            SolicitorFirmByTipstaffRecordViewModel model = new SolicitorFirmByTipstaffRecordViewModel()
            {
                solicitorFirmID = solicitorFirmID,
                tipstaffRecordID = tipstaffRecordID,
                SolicitorFirm = _solicitorFirmsPresenter.GetSolicitorFirm(solicitorFirmID),
                TipstaffRecord = _tipstaffRecordPresenter.GetTipStaffRecord(tipstaffRecordID)
            };

            return View(model);
        }

        //
        // POST: /SolicitorFirm/Create

        [HttpPost]
        public ActionResult Create(SolicitorFirm solicitorfirm)
        {
            if (ModelState.IsValid)
            {

                ////////db.SolicitorsFirms.Add(solicitorfirm);
                ////////db.SaveChanges();
                _solicitorFirmsPresenter.AddSolicitorFirm(solicitorfirm);


                if (Request.IsAjaxRequest())
                {
                    
                    var solicitorFirms = _solicitorFirmsPresenter.GetAllSolicitorFirms();
                    //////ViewBag.solicitorFirmID = new SelectList(db.SolicitorsFirms, "solicitorFirmID", "firmName", solicitorfirm.solicitorFirmID);
                    ViewBag.solicitorFirmID = new SelectList(solicitorFirms, "solicitorFirmID", "firmName", solicitorfirm.solicitorFirmID);

                    return PartialView("_createSolicitorFirmSuccess", solicitorfirm);
                }
            }
            return View(solicitorfirm);
        }
        
        //
        // GET: /SolicitorFirm/Edit/5
 
        public ActionResult Edit(string solicitorFirmID, string tipstaffRecordID)
        {
            SolicitorFirmByTipstaffRecordViewModel model = new SolicitorFirmByTipstaffRecordViewModel()
            {
                solicitorFirmID = solicitorFirmID,
                tipstaffRecordID = tipstaffRecordID,
                SolicitorFirm = _solicitorFirmsPresenter.GetSolicitorFirm(solicitorFirmID)
            };

           return View(model);
        }

        //
        // POST: /SolicitorFirm/Edit/5

        [HttpPost]
        public ActionResult Edit(SolicitorFirmByTipstaffRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //////db.Entry(model.SolicitorFirm).State = EntityState.Modified;
                //////db.SaveChanges();
                _solicitorFirmsPresenter.Update(model.SolicitorFirm);

                return RedirectToAction("Details", "SolicitorFirm", new { solicitorFirmID = model.solicitorFirmID, tipstaffRecordID = model.tipstaffRecordID });
            }
            return View(model);
        }

        //
        // GET: /SolicitorFirm/Delete/5
 
        public ActionResult Delete(string id)
        {
            //////SolicitorFirm solicitorfirm = db.SolicitorsFirms.Find(id);
            var solicitorFirm = _solicitorFirmsPresenter.GetSolicitorFirm(id);

            return View(solicitorFirm);
        }

        //
        // POST: /SolicitorFirm/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            //////SolicitorFirm solicitorfirm = db.SolicitorsFirms.Find(id);
            //////db.SolicitorsFirms.Remove(solicitorfirm);
            //////db.SaveChanges();
            var firm = _solicitorFirmsPresenter.GetSolicitorFirm(id);
            _solicitorFirmsPresenter.Delete(firm);

            return RedirectToAction("Index");
        }
        public ActionResult QuickSearch(string term)
        {
            var solicitorFirms = _solicitorFirmsPresenter.GetAllSolicitorFirms();

            ////var sols = db.SolicitorsFirms.Where(s => s.firmName.ToLower().Contains(term.ToLower())).ToList().Select(a => new { value = a.firmName });

            var sols = solicitorFirms.Where(s => s.firmName.ToLower().Contains(term.ToLower())).ToList().Select(a => new { value = a.firmName });
            return Json(sols, JsonRequestBehavior.AllowGet);
        }
        

       

    }
}