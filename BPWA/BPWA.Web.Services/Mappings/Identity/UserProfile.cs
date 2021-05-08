using AutoMapper;
using BPWA.Core.Entities;
using BPWA.Web.Services.Models;
using System.Linq;

namespace BPWA.DAL.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserAddModel, User>();
            CreateMap<User, UserUpdateModel>()
                .ForMember(dest => dest.CityIdDropdownItem, opt => opt.MapFrom(src => new DropdownItem { Id = src.CityId.GetValueOrDefault(), Text = src.City.Name }))
                .ForMember(dest => dest.RoleIdsDropdownItems, opt => opt.MapFrom(src => src.UserRoles.Select(x => new DropdownItem<string> { Id = x.RoleId, Text = x.Role.Name }).ToList()))
                .ReverseMap();
            
            CreateMap<User, AccountUpdateModel>()
                .ForMember(dest => dest.SelectedCity, opt => opt.MapFrom(src => src.City.Name))
                .ReverseMap()
                .ForMember(dest => dest.City, opt => opt.Ignore());
        }
    }
}
