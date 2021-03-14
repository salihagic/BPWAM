using BPWA.Common.Resources;
using BPWA.Web.Services.Models;
using FluentValidation;

namespace BPWA.Web.Services.ModelValidators
{
    public class RoleUpdateModelValidator : AbstractValidator<RoleUpdateModel>
    {
        public RoleUpdateModelValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage(Translations.Required_field);
        }
    }
}
