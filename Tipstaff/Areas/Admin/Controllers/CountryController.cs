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
using Tipstaff.Services.Repositories;

namespace Tipstaff.Areas.Admin.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Admin)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class CountryController : Controller
    {
        //private TipstaffDB db = myDBContextHelper.CurrentContext;

        private readonly ICountryRepository _countryRepository;

        public CountryController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
        //
        // GET: /Admin/Country/

        public ViewResult Index(CountryListView model)
        {
            if (model.page < 1)
            {
                model.page = 1;
            }

            //IEnumerable<Country> Countries = db.IssuingCountries;
            var Countries = _countryRepository.GetAllCountries();

            if (model.onlyActive == true)
            {
                Countries = Countries.Where(c => c.Active == true);
            }
            if (model.detailContains != "" && model.detailContains!=null)
            {
                Countries= Countries.Where(c=>c.Detail.ToLower().Contains(model.detailContains.ToLower().ToString()));
            }
            model.Countries = Countries.OrderBy(c => c.Detail).ToPagedList(model.page, Int32.Parse(ConfigurationManager.AppSettings["pageSize"]));
            return View(model);
        }

        //
        // GET: /Admin/Country/Details/5

        public ActionResult Details(string id)
        {
            //Country country = db.IssuingCountries.Find(id);
            var country = _countryRepository.GetCountry(id);
            if (country.Active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot view {0} as it has been deactivated, please raise a help desk call to re-activate it.", country.Detail);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(country);
        }

        //
        // GET: /Admin/Country/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Admin/Country/Create

        [HttpPost]
        public ActionResult Create(Country country)
        {
            if (ModelState.IsValid)
            {
                country.active = true;
                //db.IssuingCountries.Add(country);
                //db.SaveChanges();
                _countryRepository.AddCountry(new Services.DynamoTables.Country()
                {
                    CountryId = country.countryID,
                    Detail = country.Detail
                });
                return RedirectToAction("Index");  
            }

            return View(country);
        }
        
        //
        // GET: /Admin/Country/Edit/5
 
        public ActionResult Edit(string id)
        {
            //Country country = db.IssuingCountries.Find(id);
            var country = _countryRepository.GetCountry(id);
            if (country.Active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot edit {0} as it has been deactivated, please raise a help desk call to re-activate it.", country.Detail);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area="", model=errModel ?? null});
            }
            return View(country);
        }

        //
        // POST: /Admin/Country/Edit/5

        [HttpPost]
        public ActionResult Edit(Country country)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(country).State = EntityState.Modified;
                //db.SaveChanges();
                _countryRepository.Update(new Services.DynamoTables.Country()
                {
                    CountryId = country.countryID,
                    Detail = country.Detail,
                    Active = country.active,
                    Deactivated = country.deactivated,
                    DeactivatedBy = country.deactivatedBy
                });
                return RedirectToAction("Index");
            }
            return View(country);
        }

        //
        // GET: /Admin/Country/Delete/5

        public ActionResult Deactivate(string id)
        {
            //Country country = db.IssuingCountries.Find(id);
            var country = _countryRepository.GetCountry(id);
            if (country.Active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot deactivate {0} as it has already been deactivated", country.Detail);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(country);
        }

        //
        // POST: /Admin/Country/Delete/5

        [HttpPost, ActionName("Deactivate")]
        public ActionResult DeleteConfirmed(string id)
        {
            //Country country = db.IssuingCountries.Find(id);
            var country = _countryRepository.GetCountry(id);

            //country.active = false;
            //country.deactivated = DateTime.Now;
            //country.deactivatedBy = User.Identity.Name;
            //db.Entry(country).State = EntityState.Modified;
            //db.SaveChanges();

            _countryRepository.Update(new Services.DynamoTables.Country()
            {
                CountryId = country.CountryId,
                Detail = country.Detail,
                Active = false,
                Deactivated = DateTime.Now,
                DeactivatedBy = User.Identity.Name
            });
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            //base.Dispose(disposing);
        }
    }
}