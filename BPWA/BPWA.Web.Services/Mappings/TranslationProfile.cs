using AutoMapper;
using BPWA.Core.Entities;
using BPWA.Web.Services.Models;

namespace BPWA.DAL.Mappings
{
    public class TranslationProfile : Profile
    {
        public TranslationProfile()
        {
            CreateMap<TranslationAddModel, Translation>();
            CreateMap<Translation, TranslationUpdateModel>()
                .ReverseMap();
        }
    }
}
