using AutoMapper;
using BPWA.Core.Entities;
using BPWA.Web.Services.Models;

namespace BPWA.DAL.Mappings
{
    public class AccountTypeProfile : Profile
    {
        public AccountTypeProfile()
        {
            CreateMap<AccountTypeAddModel, AccountType>();
            CreateMap<AccountType, AccountTypeUpdateModel>()
                .ReverseMap()
                .ForMember(src => src.SystemAccountType, opt => opt.Ignore());
        }
    }
}
