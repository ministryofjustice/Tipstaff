using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface IApplicantRepository
    {
        void AddApplicant(Applicant contact);

        Applicant GetApplicant(string id);

        IEnumerable<Applicant> GetAllApplicantsByTipstaffRecordID(string id);

        IEnumerable<Applicant> GetAllApplicants();

        void Update(Applicant applicant);

        void Delete(Applicant applicant);
    }
}
