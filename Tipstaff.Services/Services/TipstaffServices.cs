using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.dto;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Services.Services
{
    public class TipstaffService: ITipstaffService
    {
        public readonly ITipstaffRecordRepository _tipstaffRepo;

        public TipstaffService(ITipstaffRecordRepository tr)
        {
            _tipstaffRepo = tr;
        }

        public dto.Tipstaff GetTipstaffRecord(string id)
        {
            throw new NotImplementedException();
        }
    }
}
