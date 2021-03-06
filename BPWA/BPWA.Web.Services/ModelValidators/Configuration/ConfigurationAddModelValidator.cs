using BPWA.Common.Resources;
using BPWA.Web.Services.Models;
using FluentValidation;

namespace BPWA.Web.Services.ModelValidators
{
    public class ConfigurationAddModelValidator : AbstractValidator<ConfigurationAddModel>
    {
        public ConfigurationAddModelValidator()
        {
            RuleFor(x => x.AboutUs).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.ApiVersion).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.WebVersion).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.MobileVersion).NotNull().WithMessage(Translations.Required_field);
        }
    }
}
