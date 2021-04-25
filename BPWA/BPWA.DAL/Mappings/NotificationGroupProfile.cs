﻿using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Models;

namespace BPWA.DAL.Mappings
{
    public class NotificationGroupProfile : Profile
    {
        public NotificationGroupProfile()
        {
            CreateMap<NotificationGroup, NotificationGroupDTO>();
        }
    }
}
