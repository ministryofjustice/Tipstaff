using System.Data;
using System.Linq;
using System.Web.Mvc;
using Tipstaff.Models;
using Tipstaff.Services.Repositories;
using Tipstaff.Infrastructure.Services;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class SolicitorFirmController : Controller
    {
        ////private TipstaffDB db = myDBContextHelper.CurrentContext;
        private readonly ISolicitorFirmRepository _solicitorFirmRepository;
        private readonly ITipstaffRecordRepository _tipstaffRecordRepository;
        private readonly IGuidGenerator _guidGenerator;

        public SolicitorFirmController(ISolicitorFirmRepository solicitorFirmRepository, ITipstaffRecordRepository tipstaffRecordRepository, IGuidGenerator guidGenerator)
        {
            _solicitorFirmRepository = solicitorFirmRepository;
            _tipstaffRecordRepository = tipstaffRecordRepository;
            _guidGenerator = guidGenerator;
        }

        //
        // GET: /SolicitorFirm/Details/5

        public ViewResult Details(string solicitorFirmID, string tipstaffRecordID)
        {
            SolicitorFirmByTipstaffRecordViewModel model = new SolicitorFirmByTipstaffRecordViewModel()
            {
                solicitorFirmID = solicitorFirmID,
                tipstaffRecordID = tipstaffRecordID,
                SolicitorFirm = GetSolicitorFirm(solicitorFirmID),
                TipstaffRecord = GetTipstaffRecord(tipstaffRecordID)
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
                _solicitorFirmRepository.AddSolicitorFirm(new Services.DynamoTables.SolicitorFirm()
                {
                    SolicitorFirmID = _guidGenerator.GenerateTimeBasedGuid().ToString(),
                    AddressLine1 = solicitorfirm.addressLine1,
                    AddressLine2 = solicitorfirm.addressLine2,
                    AddressLine3 = solicitorfirm.addressLine3,
                    County = solicitorfirm.county,
                    Email = solicitorfirm.email,
                    DX = solicitorfirm.DX,
                    FirmName = solicitorfirm.firmName,
                    Postcode = solicitorfirm.postcode,
                    PhoneDayTime = solicitorfirm.phoneDayTime,
                    PhoneOutofHours = solicitorfirm.phoneOutofHours,
                    Town = solicitorfirm.town,
                    Active = solicitorfirm.active,
                    Deactivated = solicitorfirm.deactivated,
                    DeactivatedBy = solicitorfirm.deactivatedBy
                });

                if (Request.IsAjaxRequest())
                {
                    
                    var solicitorFirms = _solicitorFirmRepository.GetAllSolicitorFirms();
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
                SolicitorFirm = GetSolicitorFirm(solicitorFirmID)
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
                _solicitorFirmRepository.Update(new Services.DynamoTables.SolicitorFirm()
                {
                    Active = model.SolicitorFirm.active,
                    AddressLine1 = model.SolicitorFirm.addressLine1,
                    AddressLine2 = model.SolicitorFirm.addressLine2,
                    AddressLine3 = model.SolicitorFirm.addressLine3,
                    County = model.SolicitorFirm.county,
                    SolicitorFirmID = model.SolicitorFirm.solicitorFirmID,
                    Deactivated = model.SolicitorFirm.deactivated,
                    DeactivatedBy = model.SolicitorFirm.deactivatedBy,
                    DX = model.SolicitorFirm.DX,
                    Email = model.SolicitorFirm.email,
                    FirmName = model.SolicitorFirm.firmName,
                    PhoneDayTime = model.SolicitorFirm.phoneDayTime,
                    PhoneOutofHours = model.SolicitorFirm.phoneOutofHours,
                    Postcode = model.SolicitorFirm.postcode,
                    Town = model.SolicitorFirm.town,
                });

                return RedirectToAction("Details", "SolicitorFirm", new { solicitorFirmID = model.solicitorFirmID, tipstaffRecordID = model.tipstaffRecordID });
            }
            return View(model);
        }

        //
        // GET: /SolicitorFirm/Delete/5
 
        public ActionResult Delete(string id)
        {
            //////SolicitorFirm solicitorfirm = db.SolicitorsFirms.Find(id);
            var solicitorFirm = GetSolicitorFirm(id);

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
            var firm = _solicitorFirmRepository.GetSolicitorFirm(id);
            _solicitorFirmRepository.Delete(firm);

            return RedirectToAction("Index");
        }
        public ActionResult QuickSearch(string term)
        {
            var solisitorFirms = _solicitorFirmRepository.GetAllSolicitorFirms();

            ////var sols = db.SolicitorsFirms.Where(s => s.firmName.ToLower().Contains(term.ToLower())).ToList().Select(a => new { value = a.firmName });

            var sols = solisitorFirms.Where(s => s.FirmName.ToLower().Contains(term.ToLower())).ToList().Select(a => new { value = a.FirmName });
            return Json(sols, JsonRequestBehavior.AllowGet);
        }

        private SolicitorFirm GetSolicitorFirm(string id)
        {
            var firm = _solicitorFirmRepository.GetSolicitorFirm(id);

            var solicitorFirm = new SolicitorFirm()
            {
                addressLine1 = firm.AddressLine1,
                addressLine2 = firm.AddressLine2,
                addressLine3 = firm.AddressLine3,
                county = firm.County,
                firmName = firm.FirmName,
                DX = firm.DX,
                email = firm.Email,
                phoneDayTime = firm.PhoneDayTime,
                town = firm.Town,
                postcode = firm.Postcode,
                phoneOutofHours = firm.PhoneOutofHours,
                solicitorFirmID = firm.SolicitorFirmID,
                active = firm.Active,
                deactivated = firm.Deactivated,
                deactivatedBy = firm.DeactivatedBy

                ////CHECK SOLICITORS AND PupulatedLINES!!!!!!!!!!!!!!!!!!!!!!!!
            };

            return solicitorFirm;
        }

        //// NEED TO REVISIT THIS AREA
        private TipstaffRecord GetTipstaffRecord(string id)
        {
            var record = _tipstaffRecordRepository.GetEntityByHashKey(id);

            var tipstaffRecord = new TipstaffRecord()
            {
                arrestCount = record.ArrestCount,
                NPO = record.NPO
            };

            return tipstaffRecord;
        }

    }
}