using BPWA.Common.Extensions;
using BPWA.Common.Resources;
using BPWA.Web.Services.Models;
using FluentValidation;

namespace BPWA.Web.Services.ModelValidators
{
    public class ResetPasswordModelValidator : AbstractValidator<ResetPasswordModel>
    {
        public ResetPasswordModelValidator()
        {
            RuleFor(x => x.Password).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.PasswordConfirmed).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.PasswordConfirmed)
                .Equal(x => x.Password)
                .When(x => x.PasswordConfirmed.HasValue())
                .WithMessage(Translations.Passwords_do_not_match);
        }
    }
}
