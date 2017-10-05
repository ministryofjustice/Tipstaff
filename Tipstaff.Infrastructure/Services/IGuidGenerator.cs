using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipstaff.Infrastructure.Services
{
    public interface IGuidGenerator
    {
        Guid GenerateTimeBasedGuid();
    }
}
