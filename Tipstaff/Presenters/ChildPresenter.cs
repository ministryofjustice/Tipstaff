using System.Collections.Generic;
using System.Linq;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class ChildPresenter : IChildPresenter, IMapper<Child, Tipstaff.Services.DynamoTables.Child>, IMapperCollections<Models.Child, Tipstaff.Services.DynamoTables.Child>
    {
       // private readonly ITipstaffRecordPresenter _tipstaffPresenter;
       private readonly ITipstaffRecordRepository _tipstaffRepository;

        public ChildPresenter(ITipstaffRecordRepository tipstaffRepository)
        {
            _tipstaffRepository = tipstaffRepository;
        }

        public Models.Child GetChild(string id, string childId)
        {
            // var child = _tipstaffPresenter.GetChild(id);
            var record = _tipstaffRepository.GetEntityByHashKey(id);
            var child = record.Children.FirstOrDefault(x => x.Id == childId);
            return GetModel(child);
        }

        public IList<Models.Child> GetAllChildrenByTipstaffRecordID(string id)
        {
            var record = _tipstaffRepository.GetEntityByHashKey(id);
            var children = record.Children;
            return children.Select(x=> GetModel(x)).ToList();
        }

        public void AddChild(ChildCreationModel model)
        {
            var entity = GetDynamoTable(model.child);
            var record = _tipstaffRepository.GetEntityByHashKey(model.tipstaffRecordID);
            record.Children.Add(entity);
            _tipstaffRepository.Update(record);
            //_childRepository.AddChild(entity);
        }

        public void UpdateChild(ChildCreationModel model)
        {
            var entity = GetDynamoTable(model.child);
            var record = _tipstaffRepository.GetEntityByHashKey(model.tipstaffRecordID);
            record.Children.Add(entity);
            _tipstaffRepository.Update(record);
        }

        public void DeleteChild(DeleteChild model)
        {
            var record = _tipstaffRepository.GetEntityByHashKey(model.Child.tipstaffRecordID);
            var child = record.Children.FirstOrDefault(x=>x.Id == model.Child.childID);
            child.IsActive = false;
            _tipstaffRepository.Update(record);
        }


        //public void UpdateChildAbduction(ChildAbduction model)
        //{
        //    _caPresenter.UpdateChildAbduction(model);
        //}

        public Models.Child GetModel(Services.DynamoTables.Child table)
        {
            //var tipstaffRecord = _tipstaffRepository.(table.TipstaffRecordID.ToString());
            ///var ca = _tipstaffPresenter.GetChildAbduction(table.Id);
            //ca = (Models.ChildAbduction)tipstaffRecord;

            var model = new Models.Child()
            {
                build = table.Build,
               // childAbduction = ca,
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
               /// tipstaffRecordID = table.TipstaffRecordID.ToString(),
            };

            return model;
        }

        public Services.DynamoTables.Child GetDynamoTable(Models.Child model)
        {
            var entity = new Services.DynamoTables.Child()
            {
                Id = model.childID,
                Build = model.build,
                //TipstaffRecordID = model.tipstaffRecordID,
                Specialfeatures = model.specialfeatures,
                Country = MemoryCollections.CountryList.GetCountryByID(model.country.CountryID)?.Detail,
                DateOfBirth = model.dateOfBirth,
                EyeColour = model.eyeColour,
                Gender = MemoryCollections.GenderList.GetGenderById(model.gender.GenderId)?.Detail,
                HairColour = model.hairColour,
                Height = model.height,
                NameFirst = model.nameFirst,
                NameLast = model.nameLast,
                NameMiddle = model.nameMiddle,
                Nationality = MemoryCollections.NationalityList.GetNationalityByID(model.nationality.NationalityID)?.Detail,
                PNCID = model.PNCID,
                SkinColour = MemoryCollections.SkinColourList.GetSkinColourById(model.skinColour.SkinColourId)?.Detail,
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
            //var children = _tipstaffRepository.GetAllByCondition();
            //return GetAll(children);

            return null;
        }
    }
}
