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
using Tipstaff.Services.Repositories;
using Tipstaff.Infrastructure.Services;

namespace Tipstaff.Controllers
{
    [AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.User)]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class ChildController : Controller
    {
        //private TipstaffDB db = myDBContextHelper.CurrentContext;
        private readonly IChildRepository _childRepository;
        private readonly ITipstaffRecordRepository _tipstaffRecordRepository;

        public ChildController(IChildRepository childRepository, ITipstaffRecordRepository tipstaffRecordRepository)
        {
            _childRepository = childRepository;
            _tipstaffRecordRepository = tipstaffRecordRepository;
        }
        //
        // GET: /Child/

        public ActionResult Details(string id)
        {
            //Child model = db.Children.Find(id);
            var model = _childRepository.GetChild(id);
            Child c = new Child() {
                childID = model.ChildID,
                nameFirst = model.NameFirst,
                nameLast = model.NameLast,
                nameMiddle = model.NameMiddle,
                dateOfBirth = model.DateOfBirth,
                gender = MemoryCollections.GenderList.GetGenderByDetail(model.Gender),
                height = model.Height,
                build = model.Build,
                hairColour = model.HairColour,
                eyeColour = model.EyeColour,
                skinColour = MemoryCollections.SkinColourList.GetSkinColourByDetail(model.SkinColour),
                specialfeatures = model.Specialfeatures,
                //country = model.Country,
                //nationality = model.Nationality,
                PNCID = model.PNCID,
                tipstaffRecordID = model.TipstaffRecordID
        };
            return View(c);
        }
        //
        // GET: /Child/Edit/5
        public ActionResult Edit(string id)
        {
            ChildCreationModel model = new ChildCreationModel();
            var c = _childRepository.GetChild(id);
            //model.child = db.Children.Find(id);
            model.child = new Child()
            {
                childID = c.ChildID,
                nameFirst = c.NameFirst,
                nameLast = c.NameLast,
                nameMiddle = c.NameMiddle,
                dateOfBirth = c.DateOfBirth,
                gender = MemoryCollections.GenderList.GetGenderByDetail(c.Gender),
                height = c.Height,
                build = c.Build,
                hairColour = c.HairColour,
                eyeColour = c.EyeColour,
                skinColour = MemoryCollections.SkinColourList.GetSkinColourByDetail(c.SkinColour),
                specialfeatures = c.Specialfeatures,
                //country = c.Country,
                //nationality = c.Nationality,
                PNCID = c.PNCID,
                tipstaffRecordID = c.TipstaffRecordID
            };
            var tipstaff = _tipstaffRecordRepository.GetEntityByHashKey(c.TipstaffRecordID);

            //if (model.child.childAbduction.caseStatus.sequence > 3)
            //{
            //    TempData["UID"] = model.child.childAbduction.UniqueRecordID;
            //    return RedirectToAction("ClosedFile", "Error");
            //}
            if (tipstaff.CaseStatus == "File Closed" || tipstaff.CaseStatus == "File Archived")
            {
                TempData["UID"] = "CA" + tipstaff.TipstaffRecordID; //This is not entirely correct. They use a numeric ID to identify records
                                                                    //model.applicant.childAbduction.UniqueRecordID;
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
                Child c = model.child;
                _childRepository.Update(new Services.DynamoTables.Child() {
                    ChildID = c.childID,
                    NameFirst = c.nameFirst,
                    NameLast = c.nameLast,
                    NameMiddle = c.nameMiddle,
                    DateOfBirth = c.dateOfBirth,
                    Gender = c.gender.Detail,
                    Height = c.height,
                    Build = c.build,
                    HairColour = c.hairColour,
                    EyeColour = c.eyeColour,
                    SkinColour = c.skinColour.Detail,
                    Specialfeatures = c.specialfeatures,
                    //country = c.Country,
                    //nationality = c.Nationality,
                    PNCID = c.PNCID,
                    TipstaffRecordID = c.tipstaffRecordID
                });
                return RedirectToAction("Details", "ChildAbduction", new { id = model.child.tipstaffRecordID });
            }
            return View(model);
        }

        public ActionResult Create(string id, bool initial=false)
        {
            ChildCreationModel model = new ChildCreationModel(id);
            var tipstaff = _tipstaffRecordRepository.GetEntityByHashKey(id);

            
            if (tipstaff.CaseStatus == "File Closed" || tipstaff.CaseStatus == "File Archived")
            {
                TempData["UID"] = "CA" + tipstaff.TipstaffRecordID; ;// model.tipstaffRecord.UniqueRecordID;
                return RedirectToAction("ClosedFile", "Error");
            }
            //if (model.tipstaffRecord.caseStatus.sequence > 3)
            //{
            //    TempData["UID"] = model.tipstaffRecord.UniqueRecordID;
            //    return RedirectToAction("ClosedFile", "Error");
            //}
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
                string cid = (model.child.childID == null) ? GuidGenerator.GenerateTimeBasedGuid().ToString() : model.child.childID;

                var ca = _tipstaffRecordRepository.GetEntityByHashKey(model.tipstaffRecordID);
                var eldestChild = _childRepository.GetAllChildrenByTipstaffRecordID(model.tipstaffRecordID).OrderBy(c => c.DateOfBirth).ThenBy(c => c.ChildID).FirstOrDefault();

                //ChildAbduction ca = db.ChildAbductions.Find(model.tipstaffRecordID);
                /* if 
                 *  EldestChild is null or
                 *  new child is eldest 
                 * Add the surname of this child to the CA record
                 */
                //Child curEldest = ca.children.OrderBy(c => c.dateOfBirth).ThenBy(c => c.childID).FirstOrDefault();
                string newSurname = model.child.nameLast; //by default set to new childs name
                //if (curEldest != null)
                //{
                //    if (model.child.dateOfBirth > curEldest.dateOfBirth)
                //    {
                //        newSurname = curEldest.nameLast;
                //    }
                //}
                if (eldestChild != null)
                {
                    if (model.child.dateOfBirth > eldestChild.DateOfBirth)
                    {
                        newSurname = eldestChild.NameLast;
                    }
                }
                ca.EldestChild = newSurname;

                _tipstaffRecordRepository.Update(ca);

                _childRepository.AddChild(new Services.DynamoTables.Child() {

                    ChildID = cid,
                    NameLast = model.child.nameLast,
                    NameFirst = model.child.nameFirst,
                    NameMiddle = model.child.nameMiddle,
                    DateOfBirth = model.child.dateOfBirth,
                    Gender = model.child.gender.Detail,
                    Height = model.child.height,
                    Build = model.child.build,
                    HairColour = model.child.hairColour,
                    EyeColour = model.child.eyeColour,
                    SkinColour = model.child.skinColour.Detail,
                    Specialfeatures = model.child.specialfeatures,
                    Country = model.child.country.Detail,
                    Nationality = model.child.nationality.Detail,
                    TipstaffRecordID = model.tipstaffRecordID,
                    PNCID = model.child.PNCID
                });


                ////Add new child
                //ca.children.Add(model.child);

                ////Now save the changes
                //db.SaveChanges();

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
        [OutputCache(Location = OutputCacheLocation.Server, Duration = 10)]
        public PartialViewResult ListChildrenByRecord(string id, int? page)
        {
            ListChildrenByTipstaffRecord model = new ListChildrenByTipstaffRecord();
            //try
            //{
            //    ChildAbduction ca = db.ChildAbductions.Find(id);
            //    model.tipstaffRecordID = ca.tipstaffRecordID;
            //    model.TipstaffRecordClosed = ca.caseStatus.sequence > 3;
            //    model.Children = ca.children.ToXPagedList<Child>(page ?? 1, 8);
            //}
            //catch
            //{
            //    //do nothing!  Return empty model
            //}
            var tipstaff = _tipstaffRecordRepository.GetEntityByHashKey(id);
            model.tipstaffRecordID = id;
            model.Children = _childRepository.GetAllChildrenByTipstaffRecordID(id).ToXPagedList<Tipstaff.Services.DynamoTables.Child>(page ?? 1, 8);
            model.TipstaffRecordClosed = (tipstaff.CaseStatus == "File Closed" || tipstaff.CaseStatus == "File Archived") ? true : false;
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
            _childRepository.Delete(new Services.DynamoTables.Child()
            {
                ChildID = model.Child.childID,
                NameLast = model.Child.nameLast,
                NameFirst = model.Child.nameFirst,
                NameMiddle = model.Child.nameMiddle,
                DateOfBirth = model.Child.dateOfBirth,
                Gender = model.Child.gender.Detail,
                Height = model.Child.height,
                Build = model.Child.build,
                HairColour = model.Child.hairColour,
                EyeColour = model.Child.eyeColour,
                SkinColour = model.Child.skinColour.Detail,
                Specialfeatures = model.Child.specialfeatures,
                Country = model.Child.country.Detail,
                Nationality = model.Child.nationality.Detail,
                TipstaffRecordID = tipstaffRecordID,
                PNCID = model.Child.PNCID
            });
            return RedirectToAction("Details", controller, new { id = tipstaffRecordID });
        }

    }
}
