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
    public class SolicitorRepository : ISolicitorRepository
    {
        private readonly IDynamoAPI<Solicitor> _dynamoAPI;

        public SolicitorRepository(IDynamoAPI<Solicitor> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public void AddSolicitor(Solicitor solicitor)
        {
            _dynamoAPI.Save(solicitor);
        }

        public void Delete(Solicitor solicitor)
        {
            _dynamoAPI.Delete(solicitor);
        }

        public Solicitor GetSolicitor(string id)
        {
            return _dynamoAPI.GetEntityByHashKey(id);
        }

        public IEnumerable<Solicitor> GetSolicitors()
        {
            return _dynamoAPI.GetAll();
        }

        public void Update(Solicitor solicitor)
        {
            var entity = _dynamoAPI.GetEntity(solicitor.Id, solicitor.SolicitorFirmID);
            entity.Active = solicitor.Active;
            entity.FirstName = solicitor.FirstName;
            entity.LastName = solicitor.LastName;
            entity.PhoneDayTime = solicitor.PhoneDayTime;
            entity.PhoneOutOfHours = solicitor.PhoneOutOfHours;
            entity.Dectivated = solicitor.Dectivated;
            entity.DectivatedBy = solicitor.DectivatedBy;
            entity.Email = solicitor.Email;
            entity.Salutation = solicitor.Salutation;
            entity.SolicitorFirmID = solicitor.SolicitorFirmID;
            entity.Id = solicitor.Id;
            _dynamoAPI.Save(entity);
        }
    }
}
