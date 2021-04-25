using BPWA.Common.Resources;
using BPWA.Web.Services.Models;
using FluentValidation;

namespace BPWA.Web.Services.ModelValidators
{
    public class GroupAddModelValidator : AbstractValidator<GroupAddModel>
    {
        public GroupAddModelValidator()
        {
            RuleFor(x => x.Title).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.Description).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.UserIds).NotEmpty().WithMessage(Translations.Required_field);
        }
    }
}
