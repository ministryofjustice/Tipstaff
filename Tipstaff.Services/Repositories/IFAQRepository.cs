using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface IFAQRepository
    {
        void AddFaQ(FAQ contact);

        FAQ GetFAQ(string id);

        IEnumerable<FAQ> GetAllFAQ();

        void Update(FAQ faq);

        void Delete(FAQ faq);
    }
}
