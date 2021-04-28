using BPWA.Common.Extensions;
using BPWA.Common.Security;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace BPWA.DAL.Services
{
    public class CurrentTimezone : ICurrentTimezone
    {
        public string TimezoneId() => User?.FindFirstValue(AppClaims.Meta.TimezoneId);
        public TimeZoneInfo Timezone() => TimezoneId().IsNotEmpty() ? TimeZoneInfo.FindSystemTimeZoneById(TimezoneId()) : TimeZoneInfo.Utc;

        public DateTime? FromUtc(DateTime? dateTime)
        {
            if (dateTime == null || Timezone() == null)
                return dateTime;

            return TimeZoneInfo.ConvertTime(dateTime.Value, TimeZoneInfo.Utc, Timezone());
        }

        public DateTime? ToUtc(DateTime? dateTime)
        {
            if (dateTime == null || Timezone() == null)
                return dateTime;

            return TimeZoneInfo.ConvertTime(dateTime.Value, Timezone(), TimeZoneInfo.Utc);
        }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public CurrentTimezone(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
