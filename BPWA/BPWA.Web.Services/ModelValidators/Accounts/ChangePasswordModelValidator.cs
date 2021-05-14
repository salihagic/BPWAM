using BPWA.Common.Resources;
using BPWA.Web.Services.Models;
using FluentValidation;

namespace BPWA.Web.Services.ModelValidators
{
    public class ChangePasswordModelValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordModelValidator()
        {
            RuleFor(x => x.Password).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.NewPassword).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.NewPasswordConfirmed)
                .Equal(x => x.NewPassword)
                .When(x => !string.IsNullOrEmpty(x.Password))
                .WithMessage(Translations.Passwords_do_not_match);
        }
    }
}
