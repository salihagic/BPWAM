using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using System.Linq;

namespace BPWA.DAL.Mappings
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<Country, CountryDTO>()
                .ForMember(dest => dest.Currencies, opt => opt.MapFrom(src => src.CountryCurrencies.Select(y => y.Currency).ToList()))
                .ForMember(dest => dest.Languages, opt => opt.MapFrom(src => src.CountryLanguages.Select(y => y.Language).ToList()));
        }
    }
}
