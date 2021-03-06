using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Models;

namespace BPWA.DAL.Mappings
{
    public class BaseProfile : Profile
    {
        public BaseProfile()
        {
            CreateMap<IBaseAuditableEntity, IBaseAuditableDTO>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom<CreatedAtResolver>())
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom<ModifiedAtResolver>())
                .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom<DeletedAtResolver>())
                .IncludeAllDerived();
        }
    }
}
