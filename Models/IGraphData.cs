using System;
using System.Collections.Generic;
namespace Tipstaff.Models
{
    interface IGraphData
    {
        ICollection<string> Keys { get; }
        string ShortTitle { get; }
        string Title { get; }
        ICollection<int?> Values { get; }
    }
}
