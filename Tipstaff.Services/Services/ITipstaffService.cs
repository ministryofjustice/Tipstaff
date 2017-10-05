using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.Services
{
    public interface ITipstaffService
    {
        dto.Tipstaff GetTipstaffRecord(string id);
    }
}
