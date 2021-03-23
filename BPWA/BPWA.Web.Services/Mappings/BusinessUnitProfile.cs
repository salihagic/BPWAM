using AutoMapper;
using BPWA.Core.Entities;
using BPWA.Web.Services.Models;

namespace BPWA.DAL.Mappings
{
    public class BusinessUnitProfile : Profile
    {
        public BusinessUnitProfile()
        {
            CreateMap<BusinessUnitAddModel, BusinessUnit>();
            CreateMap<BusinessUnit, BusinessUnitUpdateModel>()
                .ReverseMap();
        }
    }
}
