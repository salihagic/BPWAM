using BPWA.Common.Enumerations;
using System;

namespace BPWA.Web.Services.Models
{
    public class AccountTypeAddModel
    {
        public SystemAccountType? SystemAccountType { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}
