using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Services.Services
{
    public interface IChildServices
    {
        dto.Child GetChild(string id);

        List<dto.Child> GetAllChildrenByTipstaffRecordID(string id);

        void AddChild(dto.Child child);

        void UpdateChild(dto.Child child);

        void DeleteChild(dto.Child child);

        dto.Tipstaff GetTipstaffRecord(string id);
    }
}
