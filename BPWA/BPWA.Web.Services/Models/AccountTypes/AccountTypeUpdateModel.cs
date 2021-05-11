using BPWA.Common.Enumerations;
using System;

namespace BPWA.Web.Services.Models
{
    public class AccountTypeUpdateModel : BaseUpdateModel
    {
        public SystemAccountType? SystemAccountType { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}
