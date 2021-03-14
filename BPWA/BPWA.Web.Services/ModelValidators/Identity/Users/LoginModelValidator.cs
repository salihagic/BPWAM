using BPWA.Common.Resources;
using BPWA.Web.Services.Models;
using FluentValidation;

namespace BPWA.Web.Services.ModelValidators
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.UserName).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.Password).NotNull().WithMessage(Translations.Required_field);
        }
    }
}
