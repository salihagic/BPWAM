using System;

namespace BPWA.Common.Configuration
{
    public class CacheSettings
    {
        public TimeSpan? TranslationDuration { get; set; }
        public TimeSpan? CompanyActivityStatusDuration { get; set; }
    }
}
