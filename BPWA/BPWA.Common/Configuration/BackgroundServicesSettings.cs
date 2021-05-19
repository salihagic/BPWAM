using System;

namespace BPWA.Common.Configuration
{
    public class BackgroundServicesSettings
    {
        public TimeSpan AccountDeactivationNotificationsServiceRepeatingPeriod { get; set; }
        public TimeSpan? AccountDeactivationNotificationMargin { get; set; }
        public TimeSpan AccountDeactivationServiceRepeatingPeriod { get; set; }
        public TimeSpan GuestAccountsDeleterServiceRepeatingPeriod { get; set; }
    }
}
