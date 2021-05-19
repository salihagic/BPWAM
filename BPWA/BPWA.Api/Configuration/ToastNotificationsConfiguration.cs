using Microsoft.Extensions.DependencyInjection;
using NToastNotify;

namespace BPWA.Api.Configuration
{
    public static class ToastNotificationsConfiguration
    {
        public static IMvcBuilder ConfigureToastNotifications(this IMvcBuilder builder)
        {
            builder.AddNToastNotifyToastr(new ToastrOptions
            {
                ProgressBar = true,
                PositionClass = ToastPositions.TopRight
            });

            return builder;
        }
    }
}
