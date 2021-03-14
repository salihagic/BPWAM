using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.Web.Services.Models;

namespace BPWA.DAL.Mappings
{
    public class LanguageProfile : Profile
    {
        public LanguageProfile()
        {
            CreateMap<LanguageAddModel, Language>();
            CreateMap<Language, LanguageUpdateModel>().ReverseMap();
        }
    }
}
