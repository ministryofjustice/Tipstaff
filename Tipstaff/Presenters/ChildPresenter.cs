using System.Collections.Generic;
using System.Linq;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class ChildPresenter : IChildPresenter, IMapper<Models.Child, Tipstaff.Services.DynamoTables.Child>, IMapperCollections<Models.Child, Tipstaff.Services.DynamoTables.Child>
    {
        private readonly IChildRepository _childRepository;
        private readonly ITipstaffRecordPresenter _tipstaffPresenter;
        //private readonly ITipstaffRecordRepository _tipstaffRepository;

        public ChildPresenter(IChildRepository childRepo, ITipstaffRecordPresenter tipstaffPresenter)
        {
            _childRepository = childRepo;
          //  _tipstaffRepository = tipstaffRepository;
           
            _tipstaffPresenter = tipstaffPresenter;
        }

        public Models.Child GetChild(string id)
        {
            var child = _childRepository.GetChild(id);
            return GetModel(child);
        }

        public IEnumerable<Models.Child> GetAllChildrenByTipstaffRecordID(string id)
        {
            var children = _childRepository.GetAllChildrenByTipstaffRecordID(id);
            return children.Select(x=> GetModel(x));
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

        public Models.TipstaffRecord GetTipstaffRecord(string id, LazyLoader loader = null)
        {
            var tipstaff = _tipstaffPresenter.GetTipStaffRecord(id,loader);

            return tipstaff;
        }
        
        //public void UpdateChildAbduction(ChildAbduction model)
        //{
        //    _caPresenter.UpdateChildAbduction(model);
        //}

        public Models.Child GetModel(Services.DynamoTables.Child table)
        {
            //var tipstaffRecord = _tipstaffRepository.(table.TipstaffRecordID.ToString());
            var ca = _tipstaffPresenter.GetChildAbduction(table.TipstaffRecordID);
            //ca = (Models.ChildAbduction)tipstaffRecord;

            var model = new Models.Child()
            {
                build = table.Build,
                childAbduction = ca,
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
                nationality = MemoryCollections.NationalityList.GetNationalityList().FirstOrDefault(x=>x.Detail == table.Nationality),
                PNCID = table.PNCID,
                skinColour = MemoryCollections.SkinColourList.GetSkinColourByDetail(table.SkinColour),
                specialfeatures = table.Specialfeatures,
                tipstaffRecordID = table.TipstaffRecordID.ToString(),
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
                Nationality = MemoryCollections.NationalityList.GetNationalityList().FirstOrDefault(x=>x.Detail == model.nationality.Detail)?.Detail,
                PNCID = model.PNCID,
                SkinColour = model.skinColour?.Detail,
                
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

        public IEnumerable<Models.Child> GetAllChildren()
        {
            var children = _childRepository.GetAllChildren();
            return GetAll(children);
        }
    }
}
