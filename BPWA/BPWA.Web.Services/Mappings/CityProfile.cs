using AutoMapper;
using BPWA.Core.Entities;
using BPWA.Web.Services.Models;

namespace BPWA.DAL.Mappings
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<CityAddModel, City>();
            CreateMap<City, CityUpdateModel>()
                .ForMember(dest => dest.CountryIdDropdownItem, opt => opt.MapFrom(src => new DropdownItem { Id = src.CountryId, Text = src.Country.Name }))
                .ReverseMap();
        }
    }
}
