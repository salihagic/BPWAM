using BPWA.Common.Resources;
using BPWA.Web.Services.Models;
using FluentValidation;

namespace BPWA.Web.Services.ModelValidators
{
    public class BusinessUnitAddModelValidator : AbstractValidator<BusinessUnitAddModel>
    {
        public BusinessUnitAddModelValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage(Translations.Required_field);
        }
    }
}
