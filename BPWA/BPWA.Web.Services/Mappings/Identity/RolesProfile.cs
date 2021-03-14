using AutoMapper;
using BPWA.Common.Security;
using BPWA.Core.Entities;
using BPWA.Web.Services.Models;
using System.Linq;

namespace BPWA.DAL.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleAddModel, Role>()
                .ForMember(dest => dest.RoleClaims, opt => opt.MapFrom(src => src.Claims.Select(x =>
                new RoleClaim
                {
                    ClaimType = AppClaimsHelper.Authorization.Type,
                    ClaimValue = x
                }).ToList()));

            CreateMap<Role, RoleUpdateModel>()
                .ForMember(dest => dest.Claims, opt => opt.MapFrom(src => src.RoleClaims.Select(x => x.ClaimValue).ToList()))
                .ReverseMap()
                .ForMember(dest => dest.RoleClaims, opt => opt.MapFrom(src => src.Claims.Select(x =>
                new RoleClaim
                {
                    ClaimType = AppClaimsHelper.Authorization.Type,
                    ClaimValue = x
                }).ToList()));
        }
    }
}
