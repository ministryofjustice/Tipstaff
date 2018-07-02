using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Tests.Helpers
{
    public class ChildComparer : IEqualityComparer<Child>
    {
        public string Id { get; set; }
        

        public bool Equals(Child x, Child y)
        {
            return (x.childID == y.childID);
        }

        public int GetHashCode(Child obj)
        {
            return obj.childID.GetHashCode();
        }
    }
}
