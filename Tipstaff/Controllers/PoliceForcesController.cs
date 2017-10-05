using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Data.Entity;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Controllers
{
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class PoliceForcesController : Controller
    {
        private readonly IPoliceForcesRepository _policeforcesRepository;

        public PoliceForcesController(IPoliceForcesRepository policeforcesRepository)
        {
            _policeforcesRepository = policeforcesRepository;
        }
        

        [AllowAnonymous]
        public ActionResult Index()
        {
            System.Security.Principal.IIdentity userIdentity = User.Identity;
            Tipstaff.CPrincipal thisUser = new CPrincipal(userIdentity);
            if (thisUser.IsInRole("Admin"))
            {
                var policeforces = _policeforcesRepository.GetAllPoliceForces();
                return View(policeforces.ToList());
            }
            else
            {
                var policeforces = _policeforcesRepository.GetAllPoliceForces().Where(x=>x.LoggedInUser == User.Identity.IsAuthenticated);
                //var policeforces = db.PoliceForces.Where(f => f.loggedInUser == User.Identity.IsAuthenticated);
                return View(policeforces.ToList());
            }
        }
        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            var policeforces = _policeforcesRepository.GetPoliceForces(id);
            
            return View(new PoliceForces()
            {
                policeForceID = policeforces.PoliceForceId,
                deactivated  = policeforces.Deactivated,
                deactivatedBy = policeforces.DeactivatedBy, 
                active = policeforces.Active,
                loggedInUser = policeforces.LoggedInUser
            });
        }

        [AuthorizeRedirect(Roles = "Admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(PoliceForces policeforce)
        {
            if (ModelState.IsValid)
            {
                
                _policeforcesRepository.Update(new Services.DynamoTables.PoliceForces()
                {
                    PoliceForceId = policeforce.policeForceID,
                    Active = policeforce.active,
                    
                    ////Deactivated = policeforce.deactivated,
                    DeactivatedBy = policeforce.deactivatedBy,
                    PoliceForceName = policeforce.policeForceName,
                    PoliceForceEMail = policeforce.policeForceEmail,
                    LoggedInUser = policeforce.loggedInUser
                });

                return RedirectToAction("Index");
            }
            return View(policeforce);
        }
        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BusinessArea/Create

        [AuthorizeRedirect(Roles = "Admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(PoliceForces policeforces)
        {
            if (ModelState.IsValid)
            {
                _policeforcesRepository.AddPoliceForces(new Services.DynamoTables.PoliceForces()
                {

                    PoliceForceId = policeforces.policeForceID,
                    PoliceForceName = policeforces.policeForceName,
                    PoliceForceEMail = policeforces.policeForceEmail,
                    Active = policeforces.active,
                    //Deactivated = policeforces.deactivated,
                    DeactivatedBy = policeforces.deactivatedBy,
                    LoggedInUser = policeforces.loggedInUser
                });

                return RedirectToAction("Index");
            }

            return View(policeforces);
        }

    }
}
