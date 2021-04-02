using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Models;

namespace BPWA.DAL.Mappings
{
    public class TranslationProfile : Profile
    {
        public TranslationProfile()
        {
            CreateMap<Translation, TranslationDTO>();
        }
    }
}
