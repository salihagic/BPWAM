using AutoMapper;
using BPWA.Core.Entities;
using BPWA.Web.Services.Models;
using System.Linq;

namespace BPWA.DAL.Mappings
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<GroupAddModel, Group>()
                .ForMember(dest => dest.GroupUsers, opt => opt.MapFrom(src => src.UserIds.Select(x => new GroupUser { UserId = x }).ToList()));
            CreateMap<Group, GroupUpdateModel>()
                .ForMember(dest => dest.UserIds, opt => opt.MapFrom(src => src.GroupUsers.Select(x => x.UserId).ToList()))
                .ReverseMap()
                .ForMember(dest => dest.GroupUsers, opt => opt.MapFrom(src => src.UserIds.Select(x => new GroupUser { UserId = x }).ToList()));
        }
    }
}
