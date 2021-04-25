using AutoMapper;
using BPWA.Core.Entities;
using BPWA.Web.Services.Models;
using System.Linq;

namespace BPWA.DAL.Mappings
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<NotificationAddModel, Notification>()
                .ForMember(dest => dest.NotificationGroups, opt => opt.MapFrom(src => src.GroupIds.Select(x => new NotificationGroup { GroupId = x }).ToList()));
            CreateMap<Notification, NotificationUpdateModel>()
                .ForMember(dest => dest.SelectedUser, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.GroupIds, opt => opt.MapFrom(src => src.NotificationGroups.Select(x => x.GroupId).ToList()))
                .ReverseMap()
                .ForMember(dest => dest.NotificationGroups, opt => opt.MapFrom(src => src.GroupIds.Select(x => new NotificationGroup { GroupId = x }).ToList()));
        }
    }
}
