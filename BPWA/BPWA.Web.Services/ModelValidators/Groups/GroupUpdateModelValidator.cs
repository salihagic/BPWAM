using BPWA.Common.Resources;
using BPWA.Web.Services.Models;
using FluentValidation;

namespace BPWA.Web.Services.ModelValidators
{
    public class GroupUpdateModelValidator : AbstractValidator<GroupUpdateModel>
    {
        public GroupUpdateModelValidator()
        {
            RuleFor(x => x.Title).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.Description).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.UserIds).NotEmpty().WithMessage(Translations.Required_field);
        }
    }
}
