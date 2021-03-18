﻿using AutoMapper;
using BPWA.Common.Security;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;

namespace BPWA.Controllers
{
    [Authorize(Policy = AppClaims.Authorization.CitiesManagement)]
    public class CitiesController :
        BaseCRUDController<
            City,
            CitySearchModel,
            CityDTO,
            CityAddModel,
            CityUpdateModel
            >
    {
        public CitiesController(
            ICitiesWebService service,
            IToastNotification toast,
            IMapper mapper
            ) :
            base(service, toast, mapper)
        { }
    }
}
