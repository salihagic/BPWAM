using AutoMapper;
using BPWA.Core.Entities;
using BPWA.Web.Services.Models;

namespace BPWA.DAL.Mappings
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<CurrencyAddModel, Currency>();
            CreateMap<Currency, CurrencyUpdateModel>().ReverseMap();
        }
    }
}
