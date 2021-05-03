using System;

namespace BPWA.DAL.Services
{
    public interface ICurrentTimezone
    {
        string TimezoneId();
        TimeZoneInfo Timezone();
        public DateTime? FromUtc(DateTime? dateTime);
        public DateTime? ToUtc(DateTime? dateTime);
    }
}
