using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface ISolicitorFirmRepository
    {
        void AddSolicitorFirm(SolicitorFirm solicitorFirm);

        SolicitorFirm GetSolicitorFirm(string id);

        string GetSolicitorFirmName(string id);

        IEnumerable<SolicitorFirm> GetAllSolicitorFirms();

        void Update(SolicitorFirm solicitorFirm);

        void Delete(SolicitorFirm solicitorFirm);

    }
}
