using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Models;

namespace BPWA.DAL.Mappings
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, GroupDTO>();
        }
    }
}
