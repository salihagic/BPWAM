using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Models;

namespace BPWA.DAL.Mappings
{
    public class BusinessUnitUserProfile : Profile
    {
        public BusinessUnitUserProfile()
        {
            CreateMap<BusinessUnitUser, BusinessUnitUserDTO>();
        }
    }
}
