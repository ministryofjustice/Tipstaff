using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Models;
using Tipstaff.Services.Services;

namespace Tipstaff.Presenters
{
    public class ChildPresenter : IChildPresenter
    {
        private readonly IChildServices _childServices;

        public ChildPresenter(IChildServices childServices)
        {
            _childServices = childServices;
        }

        public Child GetChild(string id)
        {
            Services.dto.Child c = _childServices.GetChild(id);
            Child child = new Child()
            {
                childID = c.ChildID,
                nameLast = c.NameLast,
                nameFirst = c.NameFirst,
                nameMiddle = c.NameMiddle,
                dateOfBirth = c.DateOfBirth,
                height = c.Height,
                build = c.Build,
                hairColour = c.HairColour,
                eyeColour = c.EyeColour,
                specialfeatures = c.Specialfeatures,
                tipstaffRecordID = c.TipstaffRecordID,
                PNCID = c.PNCID,
                gender = MemoryCollections.GenderList.GetGenderByDetail(c.Gender),
                skinColour = MemoryCollections.SkinColourList.GetSkinColourByDetail(c.SkinColour),
                country = MemoryCollections.CountryList.GetCountryByDetail(c.Country),
                nationality = MemoryCollections.NationalityList.GetNationalityByDetail(c.Nationality)
            };
            return child;
        }

        public IEnumerable<Child> GetAllChildrenByTipstaffRecordID(string id)
        {
            IEnumerable<Services.dto.Child> cs = _childServices.GetAllChildrenByTipstaffRecordID(id);
            List<Child> children = new List<Child>();
            foreach (Services.dto.Child c in cs)
            {
                Child child = new Child()
                {
                    childID = c.ChildID,
                    nameLast = c.NameLast,
                    nameFirst = c.NameFirst,
                    nameMiddle = c.NameMiddle,
                    dateOfBirth = c.DateOfBirth,
                    height = c.Height,
                    build = c.Build,
                    hairColour = c.HairColour,
                    eyeColour = c.EyeColour,
                    specialfeatures = c.Specialfeatures,
                    tipstaffRecordID = c.TipstaffRecordID,
                    PNCID = c.PNCID,
                    gender = MemoryCollections.GenderList.GetGenderByDetail(c.Gender),
                    skinColour = MemoryCollections.SkinColourList.GetSkinColourByDetail(c.SkinColour),
                    country = MemoryCollections.CountryList.GetCountryByDetail(c.Country),
                    nationality = MemoryCollections.NationalityList.GetNationalityByDetail(c.Nationality)
                };
                children.Add(child);
            }
            return children;
        }

        public void AddChild(ChildCreationModel model)
        {
            _childServices.AddChild(new Services.dto.Child()
            {
                ChildID = model.child.childID,
                NameLast = model.child.nameLast,
                NameFirst = model.child.nameFirst,
                NameMiddle = model.child.nameMiddle,
                DateOfBirth = model.child.dateOfBirth,
                Height = model.child.height,
                Build = model.child.build,
                HairColour = model.child.hairColour,
                EyeColour = model.child.eyeColour,
                Specialfeatures = model.child.specialfeatures,
                TipstaffRecordID = model.child.tipstaffRecordID,
                PNCID = model.child.PNCID,
                Gender = model.child.gender.Detail,
                SkinColour = model.child.skinColour.Detail,
                Country = model.child.country.Detail,
                Nationality = model.child.nationality.Detail
            });
        }

        public void UpdateChild(ChildCreationModel model)
        {
            _childServices.UpdateChild(new Services.dto.Child()
            {
                ChildID = model.child.childID,
                NameLast = model.child.nameLast,
                NameFirst = model.child.nameFirst,
                NameMiddle = model.child.nameMiddle,
                DateOfBirth = model.child.dateOfBirth,
                Height = model.child.height,
                Build = model.child.build,
                HairColour = model.child.hairColour,
                EyeColour = model.child.eyeColour,
                Specialfeatures = model.child.specialfeatures,
                TipstaffRecordID = model.child.tipstaffRecordID,
                PNCID = model.child.PNCID,
                Gender = model.child.gender.Detail,
                SkinColour = model.child.skinColour.Detail,
                Country = model.child.country.Detail,
                Nationality = model.child.nationality.Detail
            });
        }

        public void DeleteChild(DeleteChild model)
        {
            _childServices.DeleteChild(new Services.dto.Child()
            {
                ChildID = model.Child.childID,
                NameLast = model.Child.nameLast,
                NameFirst = model.Child.nameFirst,
                NameMiddle = model.Child.nameMiddle,
                DateOfBirth = model.Child.dateOfBirth,
                Height = model.Child.height,
                Build = model.Child.build,
                HairColour = model.Child.hairColour,
                EyeColour = model.Child.eyeColour,
                Specialfeatures = model.Child.specialfeatures,
                TipstaffRecordID = model.Child.tipstaffRecordID,
                PNCID = model.Child.PNCID,
                Gender = model.Child.gender.Detail,
                SkinColour = model.Child.skinColour.Detail,
                Country = model.Child.country.Detail,
                Nationality = model.Child.nationality.Detail
            });
        }

        public TipstaffRecord GetTipstaffRecord(string id)
        {
            Services.dto.Tipstaff t = _childServices.GetTipstaffRecord(id);
            TipstaffRecord tipstaff = new TipstaffRecord() {
                tipstaffRecordID = t.TipstaffRecordID,
                createdBy = t.CreatedBy,
                createdOn = t.CreatedOn,
                nextReviewDate = t.NextReviewDate,
                resultDate = t.ResultDate,
                DateExecuted = t.DateExecuted,
                arrestCount = t.ArrestCount,
                prisonCount = t.PrisonCount,
                resultEnteredBy = t.ResultEnteredBy,
                NPO = t.NPO,
                protectiveMarking = MemoryCollections.ProtectiveMarkingsList.GetProtectiveMarkingByDetail(t.ProtectiveMarking),
                result = MemoryCollections.ResultsList.GetResultByDetail(t.Result),
                caseStatus = MemoryCollections.CaseStatusList.GetCaseStatusByDetail(t.CaseStatus)
            };

            return tipstaff;
        }

        public ChildAbduction GetChildAbduction(string id)
        {
            throw new NotImplementedException();
        }
        public void UpdateChildAbduction(ChildAbduction model)
        {
            throw new NotImplementedException();
        }
        
    }
}