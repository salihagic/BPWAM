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
            CreateMap<NotificationAddModel, Notification>();
            CreateMap<Notification, NotificationUpdateModel>()
                .ForMember(dest => dest.UserIdDropdownItem, opt => opt.MapFrom(src => new DropdownItem<string> { Id = src.UserId, Text = $"{src.User.FirstName} {src.User.LastName}" }))
                .ForMember(dest => dest.GroupIdsDropdownItems, opt => opt.MapFrom(src => src.NotificationGroups.Select(x => new DropdownItem { Id = x.GroupId, Text = x.Group.Title }).ToList()))
                .ReverseMap();
        }
    }
}
