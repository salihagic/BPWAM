using BPWA.Common.Enumerations;
using BPWA.Common.Extensions;
using System;

namespace BPWA.DAL.Models
{
    public class AccountTypeDTO : 
        BaseSoftDeletableDTO,
        IBaseSoftDeletableDTO,
        IBaseDTO
    {
        public SystemAccountType SystemAccountType { get; set; }
        public string SystemAccountTypeString => TranslationsHelper.Translate(SystemAccountType.ToString());
        public TimeSpan? Duration { get; set; }
        public string DurationString => Duration.HasValue ? $"{Duration.Value.Days} {Common.Resources.Translations.Days.ToLower()} {Duration.Value.Hours} {Common.Resources.Translations.Hours.ToLower()} {Duration.Value.Minutes} {Common.Resources.Translations.Minutes.ToLower()} {Duration.Value.Seconds} {Common.Resources.Translations.Seconds.ToLower()}" : null;
    }
}
