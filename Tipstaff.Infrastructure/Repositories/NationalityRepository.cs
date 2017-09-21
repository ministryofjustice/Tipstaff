using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Infrastructure.Repositories
{
    public class NationalityRepository : INationalityRepository
    {
        private readonly IDynamoAPI<Nationality> _dynamoAPI;

        public NationalityRepository(IDynamoAPI<Nationality> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public void AddNationality(Nationality nationality)
        {
            _dynamoAPI.Save(nationality);
        }

        public void Delete(Nationality nationality)
        {
            _dynamoAPI.Delete(nationality);
        }

        public IEnumerable<Nationality> GetAllNationalities()
        {
            return _dynamoAPI.GetAll();
        }

        public Nationality GetNationality(string id)
        {
            return _dynamoAPI.GetEntityByHashKey(id);
        }

        public void Update(Nationality nationality)
        {
            var entity = _dynamoAPI.GetEntityByHashKey(nationality.NationalityId);
            entity.Detail = nationality.Detail;
            entity.Active = nationality.Active;
            entity.Deactivated = nationality.Deactivated;
            entity.DeactivatedBy = nationality.DeactivatedBy;
            _dynamoAPI.Save(entity);
        }
    }
}
