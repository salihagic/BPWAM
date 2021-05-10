using AutoMapper;
using BPWA.Core.Entities;
using BPWA.Web.Services.Models;

namespace BPWA.DAL.Mappings
{
    public class ConfigurationProfile : Profile
    {
        public ConfigurationProfile()
        {
            CreateMap<ConfigurationAddModel, Configuration>();
            CreateMap<Configuration, ConfigurationUpdateModel>()
                .ReverseMap();
        }
    }
}
