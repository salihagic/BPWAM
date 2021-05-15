using System;

namespace BPWA.Common.Configuration
{
    public class AppSettings
    {
        public TimeSpan? AccountLifespan { get; set; }
        public TimeSpan? GuestAccountLifespan { get; set; }
    }
}
