﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Infrastructure.DynamoAPI;
using Tipstaff.Services.DynamoTables;
using Tipstaff.Services.Repositories;

namespace Tipstaff.Infrastructure.Repositories
{
    public class SolicitorRepository : ISolicitorRepository
    {
        private readonly IDynamoAPI<Solicitor> _dynamoAPI;

        public SolicitorRepository(IDynamoAPI<Solicitor> dynamoAPI)
        {
            _dynamoAPI = dynamoAPI;
        }

        public void AddSolicitor(Solicitor solicitor)
        {
            _dynamoAPI.Save(solicitor);
        }

        public IEnumerable<Solicitor> GetSolicitors()
        {
            return _dynamoAPI.GetAll();
        }

        public void Update(Solicitor solicitor)
        {
            var entity = _dynamoAPI.GetEntity(solicitor.SolicitorID, solicitor.SolicitorFirmId);
            entity.Active = solicitor.Active;
            entity.FirstName = solicitor.FirstName;
            entity.LastName = solicitor.LastName;
            entity.PhoneDayTime = solicitor.PhoneDayTime;
            entity.PhoneOutOfHours = solicitor.PhoneOutOfHours;
            entity.Dectivated = solicitor.Dectivated;
            entity.DectivatedBy = solicitor.DectivatedBy;
            entity.Email = solicitor.Email;
            entity.Salutation = solicitor.Salutation;
            entity.SolicitorFirmId = solicitor.SolicitorFirmId;
            entity.SolicitorID = solicitor.SolicitorID;
            _dynamoAPI.Save(entity);
        }
    }
}
