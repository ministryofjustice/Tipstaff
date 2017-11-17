﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;

namespace Tipstaff.Presenters
{
    public interface ITipstaffRecordPresenter
    {
        void AddTipstaffRecord(TipstaffRecord record);

        TipstaffRecord GetTipStaffRecord(string id);

        void UpdateTipstaffRecord(TipstaffRecord record);

        IEnumerable<TipstaffRecord> GetAll();
     }
}