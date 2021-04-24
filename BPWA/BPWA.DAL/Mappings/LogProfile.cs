using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Models;

namespace BPWA.DAL.Mappings
{
    public class LogProfile : Profile
    {
        public LogProfile()
        {
            CreateMap<Log, LogDTO>();
        }
    }
}
