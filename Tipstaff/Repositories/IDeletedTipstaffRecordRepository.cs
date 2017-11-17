using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface IDeletedTipstaffRecordRepository
    {
        void Add(DeletedTipstaffRecord record);

        void Remove(DeletedTipstaffRecord record);

        IEnumerable<DeletedTipstaffRecord> GetAll();
    }
}
