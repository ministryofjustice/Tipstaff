using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tipstaff.Models;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Web.UI;
using System.Data.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class ChildController : Controller
    {
        private TipstaffDB db = myDBContextHelper.CurrentContext;
        //
        // GET: /Child/

        public ActionResult Details(int id)
        {
            Child model = db.Children.Find(id);
            return View(model);
        }
        //
        // GET: /Child/Edit/5
        public ActionResult Edit(int id)
        {
            ChildCreationModel model = new ChildCreationModel();
            model.child = db.Children.Find(id);
            if (model.child.childAbduction.caseStatus.sequence > 3)
            {
                TempData["UID"] = model.child.childAbduction.UniqueRecordID;
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
                db.Entry(model.child).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "ChildAbduction", new { id = model.child.tipstaffRecordID });
            }
            return View(model);
        }

        public ActionResult Create(int id, bool initial=false)
        {
            ChildCreationModel model = new ChildCreationModel(id);
            if (model.tipstaffRecord.caseStatus.sequence > 3)
            {
                TempData["UID"] = model.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }
            model.initial = initial;
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
                ChildAbduction ca = db.ChildAbductions.Find(model.tipstaffRecordID);
                /* if 
                 *  EldestChild is null or
                 *  new child is eldest 
                 * Add the surname of this child to the CA record
                 */
                Child curEldest = ca.children.OrderBy(c => c.dateOfBirth).ThenBy(c => c.childID).FirstOrDefault();
                string newSurname = model.child.nameLast; //by default set to new childs name
                if (curEldest != null)
                {
                    if (model.child.dateOfBirth > curEldest.dateOfBirth)
                    {
                        newSurname = curEldest.nameLast;
                    }
                }
                ca.EldestChild = newSurname;
                //Add new child
                ca.children.Add(model.child);

                //Now save the changes
                db.SaveChanges();

                if (Request.IsAjaxRequest())
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
        
        public PartialViewResult ListChildrenByRecord(int id, int? page)
        {
            ListChildrenByTipstaffRecord model = new ListChildrenByTipstaffRecord();
            try
            {
                ChildAbduction ca = db.ChildAbductions.Find(id);
                model.tipstaffRecordID = ca.tipstaffRecordID;
                model.TipstaffRecordClosed = ca.caseStatus.sequence > 3;
                model.Children = ca.children.ToXPagedList<Child>(page ?? 1, 8);
            }
            catch
            {
                //do nothing!  Return empty model
            }
            return PartialView("_ListChildrenByRecord", model);
        }
        public PartialViewResult _Create(int id)
        {
            ChildCreationModel model = new ChildCreationModel(id);
            return PartialView("_createChildRecordForRecord",model);
        }

        [AuthorizeRedirect(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            DeleteChild model = new DeleteChild(id);
            if (model.Child == null)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("Child record: {0} has been deleted, please raise a help desk call if you think this has been deleted in error.", id);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            if (model.Child.childAbduction.caseStatus.sequence > 3)
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
            model.Child = db.Children.Find(model.DeleteModelID);
            int tipstaffRecordID = model.Child.tipstaffRecordID;
            string controller = genericFunctions.TypeOfTipstaffRecord(tipstaffRecordID);
            db.Children.Remove(model.Child);
            db.SaveChanges();
            //get the Audit Event we just created 
            string recDeleted = model.DeleteModelID.ToString();
            AuditEvent AE = db.AuditEvents.Where(a => a.auditEventDescription.AuditDescription == "Child deleted" && a.RecordChanged == recDeleted).OrderByDescending(a => a.EventDate).Take(1).Single();
            //add a deleted reason
            AE.DeletedReasonID = model.DeletedReasonID;
            //and save again
            db.SaveChanges();
            return RedirectToAction("Details", controller, new { id = tipstaffRecordID });
        }

    }
}
