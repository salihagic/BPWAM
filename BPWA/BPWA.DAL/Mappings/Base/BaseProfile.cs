using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Models;

namespace BPWA.DAL.Mappings
{
    public class BaseProfile : Profile
    {
        public BaseProfile()
        {
            CreateMap<IBaseAuditableEntity, IBaseDTO>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom<CreatedAtResolver>())
                .IncludeAllDerived();
        }
    }
}
