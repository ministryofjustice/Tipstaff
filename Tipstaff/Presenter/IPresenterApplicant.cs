using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenter
{
    public interface IPresenterApplicant
    {
        Applicant GetApplicant(string id);

        TipstaffRecord GetTipstaffRecord(string id);

        IEnumerable<Applicant> GetAllApplicantsByTipstaffRecordID(string id);

        void AddApplicant(ApplicantCreationModel model);

        void UpdateApplicant(ApplicantEditModel model);

        void DeleteApplicant(DeleteApplicant model);
    }
}
