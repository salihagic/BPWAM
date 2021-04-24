using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Models;

namespace BPWA.DAL.Mappings
{
    public class NotificationGroupUserProfile : Profile
    {
        public NotificationGroupUserProfile()
        {
            CreateMap<GroupUser, GroupUserDTO>();
        }
    }
}
