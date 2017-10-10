using System.Linq;
using System.Data;
using System.Web.Mvc;
using Tipstaff.Models;
using Tipstaff.Presenters;

namespace Tipstaff.Controllers
{
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class PoliceForcesController : Controller
    {
        private readonly IPoliceForcesPresenter _policeForcesPresenter;

        public PoliceForcesController(IPoliceForcesPresenter policeForcesPresenter)
        {
            _policeForcesPresenter = policeForcesPresenter;
        }
        

        [AllowAnonymous]
        public ActionResult Index()
        {
            System.Security.Principal.IIdentity userIdentity = User.Identity;
            Tipstaff.CPrincipal thisUser = new CPrincipal(userIdentity);
            if (thisUser.IsInRole("Admin"))
            {
                var policeforces = _policeForcesPresenter.GetAllPoliceForces();
                return View(policeforces.ToList());
            }
            else
            {
                var policeforces = _policeForcesPresenter.GetAllPoliceForces().Where(x=>x.loggedInUser == User.Identity.IsAuthenticated);
                //var policeforces = db.PoliceForces.Where(f => f.loggedInUser == User.Identity.IsAuthenticated);
                return View(policeforces.ToList());
            }
        }
        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            var policeforce = _policeForcesPresenter.GetPoliceForces(id);
            
            return View(policeforce);
        }

        [AuthorizeRedirect(Roles = "Admin")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(PoliceForces policeforce)
        {
            if (ModelState.IsValid)
            {
                _policeForcesPresenter.Update(policeforce);

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
                _policeForcesPresenter.AddPoliceForces(policeforces);

                return RedirectToAction("Index");
            }

            return View(policeforces);
        }

    }
}
