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

        void Update(Solicitor solicitor);

        void AddSolicitor(Solicitor solicitor);
    }
}
