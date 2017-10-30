using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenters.Interfaces
{
    public interface ISearchPresenter
    {
        SearchModel GetSearchModel(string searchRecord);
    }
}
