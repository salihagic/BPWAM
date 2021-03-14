using BPWA.Common.Resources;
using BPWA.Web.Services.Models;
using FluentValidation;

namespace BPWA.Web.Services.ModelValidators
{
    public class CityAddModelValidator : AbstractValidator<CityAddModel>
    {
        public CityAddModelValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.CountryId).NotNull().WithMessage(Translations.Required_field);
        }
    }
}
