using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using PagedList;

using Tipstaff.Models;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class SolicitorFirmController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;

        //
        // GET: /SolicitorFirm/Details/5

        public ViewResult Details(int solicitorFirmID, int tipstaffRecordID)
        {
            SolicitorFirmByTipstaffRecordViewModel model = new SolicitorFirmByTipstaffRecordViewModel(solicitorFirmID, tipstaffRecordID);
            return View(model);
        }

        //
        // POST: /SolicitorFirm/Create

        [HttpPost]
        public ActionResult Create(SolicitorFirm solicitorfirm)
        {
            if (ModelState.IsValid)
            {
                solicitorfirm.active = true;
                db.SolicitorsFirms.Add(solicitorfirm);
                db.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    ViewBag.solicitorFirmID = new SelectList(db.SolicitorsFirms, "solicitorFirmID", "firmName", solicitorfirm.solicitorFirmID);
                    return PartialView("_createSolicitorFirmSuccess", solicitorfirm);
                }
            }
            return View(solicitorfirm);
        }
        
        //
        // GET: /SolicitorFirm/Edit/5
 
        public ActionResult Edit(int solicitorFirmID, int tipstaffRecordID)
        {
            SolicitorFirmByTipstaffRecordViewModel model = new SolicitorFirmByTipstaffRecordViewModel(solicitorFirmID, tipstaffRecordID);
            return View(model);
        }

        //
        // POST: /SolicitorFirm/Edit/5

        [HttpPost]
        public ActionResult Edit(SolicitorFirmByTipstaffRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model.SolicitorFirm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "SolicitorFirm", new { solicitorFirmID = model.solicitorFirmID, tipstaffRecordID = model.tipstaffRecordID });
            }
            return View(model);
        }

        //
        // GET: /SolicitorFirm/Delete/5
 
        public ActionResult Delete(int id)
        {
            SolicitorFirm solicitorfirm = db.SolicitorsFirms.Find(id);
            return View(solicitorfirm);
        }

        //
        // POST: /SolicitorFirm/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            SolicitorFirm solicitorfirm = db.SolicitorsFirms.Find(id);
            db.SolicitorsFirms.Remove(solicitorfirm);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult QuickSearch(string term)
        {
            //var sols = db.SolicitorsFirms.Where(s => s.firmName.ToLower().Contains(term.ToLower())).ToList().Select(a => new { value = a.firmName });
            var sols = db.SolicitorsFirms.Where(s => s.firmName.ToLower().Contains(term.ToLower())).ToList().Select(a => new { value = a.firmName, a.solicitorFirmID});
            return Json(sols, JsonRequestBehavior.AllowGet);
        }

    }
}