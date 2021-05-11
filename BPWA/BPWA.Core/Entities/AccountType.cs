using BPWA.Common.Enumerations;
using System;

namespace BPWA.Core.Entities
{
    public class AccountType : BaseSoftDeletableEntity
    {
        public SystemAccountType SystemAccountType { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}
