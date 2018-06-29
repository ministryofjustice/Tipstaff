using System.Collections.Generic;
using System.Linq;
using Tipstaff.Mappers;
using Tipstaff.Models;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Presenters
{
    public class TipstaffPoliceForcesPresenter : ITipstaffPoliceForcesPresenter, IMapper<TipstaffPoliceForce, Tipstaff_PoliceForces>, IMapperCollections<TipstaffPoliceForce, Tipstaff_PoliceForces>
    {
        private readonly ITipstaffPoliceForcesRepository _tpfRepository;
        private readonly IPoliceForcesPresenter _pfPresenter;

        public TipstaffPoliceForcesPresenter(ITipstaffPoliceForcesRepository tpfRepo, IPoliceForcesPresenter pfPresenter)
        {
            _tpfRepository = tpfRepo;
            _pfPresenter = pfPresenter;
        }
        public void Add(TipstaffPoliceForce tpf)
        {
            var dt = GetDynamoTable(tpf);
            _tpfRepository.Add(dt);
        }

        public void Delete(TipstaffPoliceForce tpf)
        {
            var dt = GetDynamoTable(tpf);
            _tpfRepository.Delete(dt);
        }

        public IEnumerable<TipstaffPoliceForce> GetAllTipstaffPoliceForcesByTipstaffRecordID(string id)
        {
            var tpfs = _tpfRepository.GetTipstaffPoliceForcesByTipstaffRecordID(id);
            return tpfs.Select(x => GetModel(x));
        }

        public TipstaffPoliceForce GetTipstaffPoliceForce(string id)
        {
            var dt = _tpfRepository.GetTipstaffPoliceForces(id);
            var mdl = GetModel(dt);
            return mdl;
        }

        public void Update(TipstaffPoliceForce tpf)
        {
            var dt = GetDynamoTable(tpf);
            _tpfRepository.Update(dt);
        }

        public Tipstaff_PoliceForces GetDynamoTable(TipstaffPoliceForce model)
        {
            var entity = new Tipstaff.Services.DynamoTables.Tipstaff_PoliceForces()
            {
                Id = model.tipstaffRecordPoliceForceID,
                PoliceForceID = model.policeForceID,
                TipstaffRecordID = model.tipstaffRecordID
            };

            return entity;
        }

        public TipstaffPoliceForce GetModel(Tipstaff_PoliceForces table)
        {
            var model = new Models.TipstaffPoliceForce()
            {
                tipstaffRecordPoliceForceID = table.Id,
                policeForceID = table.PoliceForceID,
                tipstaffRecordID = table.TipstaffRecordID,
                policeForce = _pfPresenter.GetPoliceForces(table.PoliceForceID)
            };

            return model;
        }

        public IEnumerable<TipstaffPoliceForce> GetAll(IEnumerable<Tipstaff_PoliceForces> entities)
        {
            return entities.Select(x => GetModel(x));
        }

        public IEnumerable<Tipstaff_PoliceForces> GetAll(IEnumerable<TipstaffPoliceForce> entities)
        {
            return entities.Select(x => GetDynamoTable(x));
        }
    }
}