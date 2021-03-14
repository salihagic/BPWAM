using BPWA.Common.Resources;
using BPWA.Web.Services.Models;
using FluentValidation;

namespace BPWA.Web.Services.ModelValidators
{
    public class CityUpdateModelValidator : AbstractValidator<CityUpdateModel>
    {
        public CityUpdateModelValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.CountryId).NotNull().WithMessage(Translations.Required_field);
        }
    }
}
