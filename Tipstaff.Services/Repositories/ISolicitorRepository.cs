﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface ISolicitorRepository
    {

        IEnumerable<Solicitor> GetSolicitors();

        void Update(Solicitor solicitor);

        void AddSolicitor(Solicitor solicitor);
    }
}
