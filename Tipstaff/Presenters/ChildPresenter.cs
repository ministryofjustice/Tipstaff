using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using Tipstaff.Services.Services;

namespace Tipstaff.Presenters
{
    public class ChildPresenter : IChildPresenter, IMapper<Models.Child, Tipstaff.Services.DynamoTables.Child>, IMapperCollections<Models.Child, Tipstaff.Services.DynamoTables.Child>
    {
        private readonly IChildRepository _childRepository;
        private readonly ITipstaffRecordPresenter _tipstaffPresenter;
        private readonly IChildAbductionPresenter _caPresenter;

        public ChildPresenter(IChildRepository childRepo, ITipstaffRecordPresenter tipstaffPresenter, IChildAbductionPresenter caPresenter)
        {
            _childRepository = childRepo;
            _tipstaffPresenter = tipstaffPresenter;
            _caPresenter = caPresenter;
        }

        public Models.Child GetChild(string id)
        {
            var child = _childRepository.GetChild(id);
            return GetModel(child);
        }

        public IEnumerable<Models.Child> GetAllChildrenByTipstaffRecordID(string id)
        {
            var children = _childRepository.GetAllChildrenByTipstaffRecordID(id);
            return GetAll(children);
        }

        public void AddChild(ChildCreationModel model)
        {
            var entity = GetDynamoTable(model.child);
            _childRepository.AddChild(entity);
        }

        public void UpdateChild(ChildCreationModel model)
        {
            var entity = GetDynamoTable(model.child);
            _childRepository.Update(entity);
        }

        public void DeleteChild(DeleteChild model)
        {
            var entity = GetDynamoTable(model.Child);
            _childRepository.Delete(entity);
        }

        public Models.TipstaffRecord GetTipstaffRecord(string id)
        {
            var tipstaff = _tipstaffPresenter.GetTipStaffRecord(id);

            return tipstaff;
        }

        public ChildAbduction GetChildAbduction(string id)
        {
            return (ChildAbduction)_tipstaffPresenter.GetTipStaffRecord(id);
        }
        public void UpdateChildAbduction(ChildAbduction model)
        {
            _caPresenter.UpdateChildAbduction(model);
        }

        public Models.Child GetModel(Services.DynamoTables.Child table)
        {
            var model = new Models.Child()
            {
                build = table.Build,
                childAbduction = (ChildAbduction) GetTipstaffRecord(table.TipstaffRecordID),
                childID = table.Id,
                country = MemoryCollections.CountryList.GetCountryByDetail(table.Country),
                dateOfBirth = table.DateOfBirth,
                eyeColour = table.EyeColour,
                gender = MemoryCollections.GenderList.GetGenderByDetail(table.Gender),
                hairColour = table.HairColour,
                height = table.Height,
                nameFirst = table.NameFirst,
                nameLast = table.NameLast,
                nameMiddle = table.NameMiddle,
                nationality = MemoryCollections.NationalityList.GetNationalityByDetail(table.Nationality),
                PNCID = table.PNCID,
                skinColour = MemoryCollections.SkinColourList.GetSkinColourByDetail(table.SkinColour),
                specialfeatures = table.Specialfeatures,
                tipstaffRecordID = table.TipstaffRecordID
            };

            return model;
        }

        public Services.DynamoTables.Child GetDynamoTable(Models.Child model)
        {
            var entity = new Services.DynamoTables.Child()
            {
                Id = model.childID,
                Build = model.build,
                TipstaffRecordID = model.tipstaffRecordID,
                Specialfeatures = model.specialfeatures,
                Country = model.country.Detail,
                DateOfBirth = model.dateOfBirth,
                EyeColour = model.eyeColour,
                Gender = model.gender.Detail,
                HairColour = model.hairColour,
                Height = model.height,
                NameFirst = model.nameFirst,
                NameLast = model.nameLast,
                NameMiddle = model.nameMiddle,
                Nationality = model.nationality.Detail,
                PNCID = model.PNCID,
                SkinColour = model.skinColour.Detail
            };

            return entity;
        }

        public IEnumerable<Models.Child> GetAll(IEnumerable<Services.DynamoTables.Child> entities)
        {
            return entities.Select(x => GetModel(x));
        }

        public IEnumerable<Services.DynamoTables.Child> GetAll(IEnumerable<Models.Child> entities)
        {
            return entities.Select(x => GetDynamoTable(x));
        }
    }
}