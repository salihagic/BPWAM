using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Models;

namespace BPWA.DAL.Mappings
{
    public class UserTokenProfile : Profile
    {
        public UserTokenProfile()
        {
            CreateMap<UserToken, UserTokenDTO>();
        }
    }
}
