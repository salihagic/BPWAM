using AutoMapper;
using BPWA.Core.Entities;
using BPWA.Web.Services.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace BPWA.DAL.Mappings
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<CountryAddModel, Country>()
                .ForMember(dest => dest.CountryCurrencies, opt => opt.MapFrom(src => src.CurrencyIds.Select(x => new CountryCurrency { CurrencyId = x }).ToList()))
                .ForMember(dest => dest.CountryLanguages, opt => opt.MapFrom(src => src.LanguageIds.Select(x => new CountryLanguage { LanguageId = x }).ToList()));
            CreateMap<Country, CountryUpdateModel>()
                .ForMember(dest => dest.CurrencyIds, opt => opt.MapFrom(src => src.CountryCurrencies.Select(x => x.CurrencyId).ToList()))
                .ForMember(dest => dest.LanguageIds, opt => opt.MapFrom(src => src.CountryLanguages.Select(x => x.LanguageId).ToList()))
                .ReverseMap()
                .ForMember(dest => dest.CountryCurrencies, opt => opt.MapFrom(src => src.CurrencyIds.Select(x => new CountryCurrency { CurrencyId = x }).ToList()))
                .ForMember(dest => dest.CountryLanguages, opt => opt.MapFrom(src => src.LanguageIds.Select(x => new CountryLanguage { LanguageId = x }).ToList()));
        }
    }
}
