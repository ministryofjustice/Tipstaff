using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenters.Interfaces
{
    public interface IFAQPresenter
    {
        void Add(FAQ faq);

        void Update(FAQ faq);

        FAQ GetById(string id);

        IEnumerable<FAQ> GetAll();
    }
}
