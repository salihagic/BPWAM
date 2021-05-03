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
            CreateMap<GroupAddModel, Group>();
            CreateMap<Group, GroupUpdateModel>()
                .ForMember(dest => dest.UserIdsDropdownItems, opt => opt.MapFrom(src => src.GroupUsers.Select(x => new DropdownItem<string> { Id = x.UserId, Text = $"{x.User.FirstName} {x.User.LastName}" }).ToList()))
                .ReverseMap();
        }
    }
}
