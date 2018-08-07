using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public interface ISolicitorPresenter 
    {
        IEnumerable<Solicitor> GetSolicitors();

        Solicitor GetSolicitor(string id);

        void Update(Solicitor solicitor);

        void AddSolicitor(Solicitor solicitor);

        void AddRecord(string solicitorId, string tipstaffRecordId, string key);

        IEnumerable<TipstaffRecordSolicitor> GetTipstaffRecordSolicitors(string tipstaffRecordId);
    }
}
