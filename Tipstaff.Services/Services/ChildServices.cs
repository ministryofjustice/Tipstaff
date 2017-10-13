using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.dto;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Services.Services
{
    public class ChildServices: IChildServices
    {
        public readonly IChildRepository _childRepo;
        public readonly ITipstaffRecordRepository _tipstaffRepo;

        public ChildServices(IChildRepository cr, ITipstaffRecordRepository tr)
        {
            _childRepo = cr;
            _tipstaffRepo = tr;
        }

        public void AddChild(Child child)
        {
            _childRepo.AddChild(new DynamoTables.Child() {
                Id = child.ChildID,
                NameLast = child.NameLast,
                NameFirst = child.NameFirst,
                NameMiddle = child.NameMiddle,
                DateOfBirth = child.DateOfBirth,
                Height = child.Height,
                Build = child.Build,
                HairColour = child.HairColour,
                EyeColour = child.EyeColour,
                Specialfeatures = child.Specialfeatures,
                TipstaffRecordID = child.TipstaffRecordID,
                PNCID = child.PNCID,
                Gender = child.Gender,
                SkinColour = child.SkinColour,
                Country = child.Country,
                Nationality = child.Nationality
            });
        }

        public void DeleteChild(Child child)
        {
            _childRepo.Delete(new DynamoTables.Child()
            {
                Id = child.ChildID,
                NameLast = child.NameLast,
                NameFirst = child.NameFirst,
                NameMiddle = child.NameMiddle,
                DateOfBirth = child.DateOfBirth,
                Height = child.Height,
                Build = child.Build,
                HairColour = child.HairColour,
                EyeColour = child.EyeColour,
                Specialfeatures = child.Specialfeatures,
                TipstaffRecordID = child.TipstaffRecordID,
                PNCID = child.PNCID,
                Gender = child.Gender,
                SkinColour = child.SkinColour,
                Country = child.Country,
                Nationality = child.Nationality
            });
        }

        public List<Child> GetAllChildrenByTipstaffRecordID(string id)
        {
            IEnumerable<DynamoTables.Child> cs = _childRepo.GetAllChildrenByTipstaffRecordID(id);
            List<Child> children = new List<Child>();
            foreach (DynamoTables.Child c in cs)
            {
                Child child = new Child()
                {
                    ChildID = c.Id,
                    NameLast = c.NameLast,
                    NameFirst = c.NameFirst,
                    NameMiddle = c.NameMiddle,
                    DateOfBirth = c.DateOfBirth,
                    Height = c.Height,
                    Build = c.Build,
                    HairColour = c.HairColour,
                    EyeColour = c.EyeColour,
                    Specialfeatures = c.Specialfeatures,
                    TipstaffRecordID = c.TipstaffRecordID,
                    PNCID = c.PNCID,
                    Gender = c.Gender,
                    SkinColour = c.SkinColour,
                    Country = c.Country,
                    Nationality = c.Nationality
                };
                children.Add(child);
            }
            return children;
        }

        public Child GetChild(string id)
        {
            DynamoTables.Child c = _childRepo.GetChild(id);
            Child child = new Child() {
                ChildID = c.Id,
                NameLast = c.NameLast,
                NameFirst = c.NameFirst,
                NameMiddle = c.NameMiddle,
                DateOfBirth = c.DateOfBirth,
                Height = c.Height,
                Build = c.Build,
                HairColour = c.HairColour,
                EyeColour = c.EyeColour,
                Specialfeatures = c.Specialfeatures,
                TipstaffRecordID = c.TipstaffRecordID,
                PNCID = c.PNCID,
                Gender = c.Gender,
                SkinColour = c.SkinColour,
                Country = c.Country,
                Nationality = c.Nationality
            };
            return child;
        }

        public void UpdateChild(Child child)
        {
            _childRepo.Update(new DynamoTables.Child()
            {
                Id = child.ChildID,
                NameLast = child.NameLast,
                NameFirst = child.NameFirst,
                NameMiddle = child.NameMiddle,
                DateOfBirth = child.DateOfBirth,
                Height = child.Height,
                Build = child.Build,
                HairColour = child.HairColour,
                EyeColour = child.EyeColour,
                Specialfeatures = child.Specialfeatures,
                TipstaffRecordID = child.TipstaffRecordID,
                PNCID = child.PNCID,
                Gender = child.Gender,
                SkinColour = child.SkinColour,
                Country = child.Country,
                Nationality = child.Nationality
            });
        }

        public dto.Tipstaff GetTipstaffRecord(string id)
        {
            DynamoTables.TipstaffRecord tr = _tipstaffRepo.GetEntityByHashKey(id);
            dto.Tipstaff t = new dto.Tipstaff() {
                TipstaffRecordID = tr.Id,
                CreatedBy = tr.CreatedBy,
                CreatedOn = tr.CreatedOn.Value,
                NextReviewDate = tr.NextReviewDate.Value,
                ResultDate = tr.ResultDate,
                DateExecuted = tr.DateExecuted,
                ArrestCount = tr.ArrestCount,
                PrisonCount = tr.PrisonCount,
                ResultEnteredBy = tr.ResultEnteredBy,
                NPO = tr.NPO,
                ProtectiveMarking = tr.ProtectiveMarking,
                Result = tr.Result,
                CaseStatus = tr.CaseStatus
            };

            return t;
        }


    }
}
