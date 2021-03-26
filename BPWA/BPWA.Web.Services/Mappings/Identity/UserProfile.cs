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
            CreateMap<UserAddModel, User>()
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.RoleIds.Select(x => new UserRole { RoleId = x }).ToList()))
                .ForMember(dest => dest.CompanyUsers, opt => opt.MapFrom(src => src.CompanyIds.Select(x => new CompanyUser { CompanyId = x }).ToList()))
                .ForMember(dest => dest.BusinessUnitUsers, opt => opt.MapFrom(src => src.BusinessUnitIds.Select(x => new BusinessUnitUser { BusinessUnitId = x }).ToList()));
            CreateMap<User, UserUpdateModel>()
                .ForMember(dest => dest.SelectedCity, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.RoleIds, opt => opt.MapFrom(src => src.UserRoles.Select(x => x.RoleId).ToList()))
                .ForMember(dest => dest.CompanyIds, opt => opt.MapFrom(src => src.CompanyUsers.Select(x => x.CompanyId).ToList()))
                .ForMember(dest => dest.BusinessUnitIds, opt => opt.MapFrom(src => src.BusinessUnitUsers.Select(x => x.BusinessUnitId).ToList()))
                .ReverseMap()
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.RoleIds.Select(x => new UserRole { RoleId = x }).ToList()))
                .ForMember(dest => dest.CompanyUsers, opt => opt.MapFrom(src => src.CompanyIds.Select(x => new CompanyUser { CompanyId = x }).ToList()))
                .ForMember(dest => dest.BusinessUnitUsers, opt => opt.MapFrom(src => src.BusinessUnitIds.Select(x => new BusinessUnitUser { BusinessUnitId = x }).ToList()));
        }
    }
}
