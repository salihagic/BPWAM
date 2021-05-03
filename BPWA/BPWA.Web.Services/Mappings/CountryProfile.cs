using AutoMapper;
using BPWA.Core.Entities;
using BPWA.Web.Services.Models;
using System.Linq;

namespace BPWA.DAL.Mappings
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<CountryAddModel, Country>();
            CreateMap<Country, CountryUpdateModel>()
                .ForMember(dest => dest.CurrencyIdsDropdownItems, opt => opt.MapFrom(src => src.CountryCurrencies.Select(x => new DropdownItem { Id = x.CurrencyId, Text = x.Currency.Name }).ToList()))
                .ForMember(dest => dest.LanguageIdsDropdownItems, opt => opt.MapFrom(src => src.CountryLanguages.Select(x => new DropdownItem { Id = x.LanguageId, Text = x.Language.Name }).ToList()))
                .ReverseMap();
        }
    }
}
