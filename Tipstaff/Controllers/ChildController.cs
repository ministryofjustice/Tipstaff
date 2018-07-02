using System;
using System.Linq;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Web.UI;
using System.Data.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using Tipstaff.Presenters;
using TPLibrary.GuidGenerator;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class ChildController : Controller
    {
        private readonly IChildPresenter _childPresenter;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IChildAbductionPresenter _childAbductionPresenter;

        public ChildController(IChildPresenter childPresenter, IGuidGenerator guidGenerator, IChildAbductionPresenter childAbductionPresenter)
        {
            _childPresenter = childPresenter;
            _guidGenerator = guidGenerator;
            _childAbductionPresenter = childAbductionPresenter;
        }
        //
        // GET: /Child/

        public ActionResult Details(string id)
        {
            Child model = _childPresenter.GetChild(id);
            return View(model);
        }
        //
        // GET: /Child/Edit/5
        public ActionResult Edit(string id)
        {
            ChildCreationModel model = new ChildCreationModel();
            model.child = _childPresenter.GetChild(id);
            TipstaffRecord tipstaff = _childPresenter.GetTipstaffRecord(model.child.tipstaffRecordID);

            if (tipstaff.caseStatus.Detail == "File Closed" || tipstaff.caseStatus.Detail == "File Archived")
            {
                TempData["UID"] = tipstaff.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }
            return View(model);
        }

        //
        // POST: /Child/Edit/5
        [HttpPost]
        public ActionResult Edit(ChildCreationModel model)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(model.child).State = EntityState.Modified;
                //db.SaveChanges();
                _childPresenter.UpdateChild(model);
                return RedirectToAction("Details", "ChildAbduction", new { id = model.child.tipstaffRecordID });
            }
            return View(model);
        }

        public ActionResult Create(string id, bool initial=false)
        {
            ChildCreationModel model = new ChildCreationModel();
            model.tipstaffRecord = _childPresenter.GetTipstaffRecord(id);
            model.tipstaffRecordID = id;
            model.initial = initial;

            if (model.tipstaffRecord?.caseStatus?.Detail == "File Closed" || model.tipstaffRecord?.caseStatus?.Detail == "File Archived")
            {
                TempData["UID"] =  model.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ChildCreationModel model, string submitButton)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                ChildAbduction ca = _childAbductionPresenter.GetChildAbduction(model.tipstaffRecordID);
                Child eldestChild = _childPresenter.GetAllChildrenByTipstaffRecordID(model.tipstaffRecordID).OrderBy(c => c.dateOfBirth).FirstOrDefault();

                string newSurname = model.child.nameLast; //by default set to new childs name
                
                if (eldestChild != null)
                {
                    if (model.child.dateOfBirth > eldestChild.dateOfBirth)
                    {
                        newSurname = eldestChild.nameLast;
                    }
                }
                ca.EldestChild = newSurname;
                model.child.childID = _guidGenerator.GenerateTimeBasedGuid().ToString();
                _childAbductionPresenter.UpdateChildAbduction(ca);
                _childPresenter.AddChild(model);


                if (Request!=null && Request.IsAjaxRequest())
                {
                    string url = string.Format("window.location='{0}';", Url.Action("Details", "ChildAbduction", new { id = model.tipstaffRecordID }));
                    return JavaScript(url);
                }
                else
                {
                    switch (submitButton)
                    {
                        case "Save and add new Child":
                            return RedirectToAction("Create", "Child", new { id = model.tipstaffRecordID, initial = model.initial });
                        case "Save,add new Respondent":
                            return RedirectToAction("Create", "Respondent", new { id = model.tipstaffRecordID, initial = model.initial });
                        case null:
                        default:
                            return RedirectToAction("Details", "ChildAbduction", new { id = model.tipstaffRecordID });
                    }
                }
            }
            catch (DbUpdateException)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_createChildRecordForRecord", model);
                }
                else
                {
                    return View(model);
                }
            }
            catch (ValidationException ex)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = genericFunctions.GetLowestError(ex);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", errModel ?? null);
            }
            catch (DbEntityValidationException ex)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = genericFunctions.GetLowestError(ex);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", errModel ?? null);
            }
            catch (Exception ex)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = genericFunctions.GetLowestError(ex);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", errModel ?? null);
            }
        }

        [OutputCache(Location = OutputCacheLocation.Server, Duration = 10)]
        public PartialViewResult ListChildrenByRecord(string id, int? page)
        {
            ListChildrenByTipstaffRecord model = new ListChildrenByTipstaffRecord();
            try
            {
                var ca = _childAbductionPresenter.GetChildAbduction(id);
                model.tipstaffRecordID = ca.tipstaffRecordID;
                model.TipstaffRecordClosed = (ca.caseStatus.Detail == "File Closed" || ca.caseStatus.Detail == "File Archived");
                model.Children = ca.children.ToXPagedList<Child>(page ?? 1, 8);
            }
            catch
            {
                //do nothing!  Return empty model
            }

            return PartialView("_ListChildrenByRecord", model);
        }

        public PartialViewResult _Create(string id)
        {
            ChildCreationModel model = new ChildCreationModel();
            model.tipstaffRecord = _childPresenter.GetTipstaffRecord(id);
            return PartialView("_createChildRecordForRecord",model);
        }

        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            DeleteChild model = new DeleteChild();
            model.Child = _childPresenter.GetChild(id);
            model.DeleteModelID = id;

            if (model.Child == null)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("Child record: {0} has been deleted, please raise a help desk call if you think this has been deleted in error.", id);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            if (model.Child.childAbduction.caseStatus.Sequence > 3)
            {
                TempData["UID"] = model.Child.childAbduction.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }

            return View(model);
        }

        //
        // POST: /ChildRelationship/Delete/5

        [HttpPost, ActionName("Delete"), AuthorizeRedirect(Roles = "Admin")]
        public ActionResult DeleteConfirmed(DeleteChild model)
        {
            //model.Child = db.Children.Find(model.DeleteModelID);
            //string tipstaffRecordID = model.Child.tipstaffRecordID;
            //db.Children.Remove(model.Child);
            //db.SaveChanges();
            ////get the Audit Event we just created 
            //string recDeleted = model.DeleteModelID.ToString();
            //AuditEvent AE = db.AuditEvents.Where(a => a.auditEventDescription.AuditDescription == "Child deleted" && a.RecordChanged == recDeleted).OrderByDescending(a => a.EventDate).Take(1).Single();
            ////add a deleted reason
            //AE.DeletedReasonID = model.DeletedReasonID;
            ////and save again
            //db.SaveChanges();

            string tipstaffRecordID = model.Child.tipstaffRecordID;
            string controller = genericFunctions.TypeOfTipstaffRecord(tipstaffRecordID);
            _childPresenter.DeleteChild(model);
            //Audit the deletion??
            return RedirectToAction("Details", controller, new { id = tipstaffRecordID });
        }

    }
}
