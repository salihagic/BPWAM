using BPWA.Common.Resources;
using BPWA.Web.Services.Models;
using FluentValidation;

namespace BPWA.Web.Services.ModelValidators
{
    public class CountryAddModelValidator : AbstractValidator<CountryAddModel>
    {
        public CountryAddModelValidator()
        {
            RuleFor(x => x.Code).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.Name).NotNull().WithMessage(Translations.Required_field);
        }
    }
}
