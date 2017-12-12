using System.Collections.Generic;
using System.Linq;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;
using TPLibrary.DynamoAPI;

namespace Tipstaff.Infrastructure.Repositories
{
    public class ApplicantRepository : IApplicantRepository
    {
        private readonly IDynamoAPI<Applicant> _dynamoAPI;

        public ApplicantRepository(IDynamoAPI<Applicant> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public void AddApplicant(Applicant applicant)
        {
            _dynamoAPI.Save(applicant);
        }

        public void Delete(Applicant applicant)
        {
            _dynamoAPI.Delete(applicant);
        }

        public IEnumerable<Applicant> GetAllApplicants()
        {
            return _dynamoAPI.GetAll();
        }

        public IEnumerable<Applicant> GetAllApplicantsByTipstaffRecordID(string id)
        {
            return _dynamoAPI.GetAll().Where(c => c.TipstaffRecordID == id);
        }

        public Applicant GetApplicant(string id)
        {
            return _dynamoAPI.GetEntityByHashKey(id);
        }

        public void Update(Applicant applicant)
        {
            var entity = _dynamoAPI.GetEntityByHashKey(applicant.Id);
            entity.NameFirst = applicant.NameFirst;
            entity.NameLast = applicant.NameLast;
            entity.AddressLine1 = applicant.AddressLine1;
            entity.AddressLine2 = applicant.AddressLine2;
            entity.AddressLine3 = applicant.AddressLine3;
            entity.Town = applicant.Town;
            entity.County = applicant.County;
            entity.Postcode = applicant.Postcode;
            entity.Phone = applicant.Phone;
            entity.Salutation = applicant.Salutation;
            entity.TipstaffRecordID = applicant.TipstaffRecordID;

            _dynamoAPI.Save(entity);
        }
    }
}
