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
                .ForMember(dest => dest.SelectedCountry, opt => opt.MapFrom(src => src.Country.Name))
                .ReverseMap();
        }
    }
}
