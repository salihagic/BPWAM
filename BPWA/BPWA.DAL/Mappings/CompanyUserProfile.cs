using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Models;

namespace BPWA.DAL.Mappings
{
    public class CompanyUserProfile : Profile
    {
        public CompanyUserProfile()
        {
            CreateMap<CompanyUser, CompanyUserDTO>();
        }
    }
}
