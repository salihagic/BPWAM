using AutoMapper;
using BPWA.Core.Entities;
using BPWA.Web.Services.Models;

namespace BPWA.DAL.Mappings
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<CompanyAddModel, Company>();
            CreateMap<Company, CompanyUpdateModel>()
                .ReverseMap();
        }
    }
}
