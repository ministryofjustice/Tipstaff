using System.Collections.Generic;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class SolicitorFirmRepository : ISolicitorFirmRepository
    {
        private readonly IDynamoAPI<SolicitorFirm> _dynamoAPI;

        public SolicitorFirmRepository(IDynamoAPI<SolicitorFirm> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public void AddSolicitorFirm(SolicitorFirm solicitorFirm)
        {
            _dynamoAPI.Save(solicitorFirm);
        }

        public void Delete(SolicitorFirm solicitorFirm)
        {
            _dynamoAPI.Delete(solicitorFirm);
        }

        public IEnumerable<SolicitorFirm> GetAllSolicitorFirms()
        {
            return _dynamoAPI.GetAll();
        }

        public SolicitorFirm GetSolicitorFirm(string id)
        {
            return _dynamoAPI.GetEntityByKey(id);
        }

        public void Update(SolicitorFirm solicitorFirm)
        {
            var entity = _dynamoAPI.GetEntityByKey(solicitorFirm.Id);
            entity.FirmName = solicitorFirm.FirmName;
            entity.AddressLine1 = solicitorFirm.AddressLine1;
            entity.AddressLine2 = solicitorFirm.AddressLine2;
            entity.AddressLine3 = solicitorFirm.AddressLine3;
            entity.Town = solicitorFirm.Town;
            entity.County = solicitorFirm.County;
            entity.Postcode = solicitorFirm.Postcode;
            entity.DX = solicitorFirm.DX;
            entity.PhoneDayTime = solicitorFirm.PhoneDayTime;
            entity.PhoneOutofHours = solicitorFirm.PhoneOutofHours;
            entity.Email = solicitorFirm.Email;
            entity.Active = solicitorFirm.Active;
            entity.Deactivated = solicitorFirm.Deactivated;
            entity.DeactivatedBy = solicitorFirm.DeactivatedBy;

            _dynamoAPI.Save(entity);
        }
    }
}
