using AutoMapper;
using BPWA.Core.Entities;
using BPWA.Web.Services.Models;

namespace BPWA.DAL.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserAddModel, User>();
            CreateMap<UserUpdateModel, User>()
                .ReverseMap()
                .ForMember(dest => dest.SelectedCity, opt => opt.MapFrom(src => src.City.Name));
        }
    }
}
