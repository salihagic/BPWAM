using BPWA.Common.Resources;
using BPWA.Web.Services.Models;
using FluentValidation;

namespace BPWA.Web.Services.ModelValidators
{
    public class TranslationAddModelValidator : AbstractValidator<TranslationAddModel>
    {
        public TranslationAddModelValidator()
        {
            RuleFor(x => x.Culture).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.Key).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.Value).NotNull().WithMessage(Translations.Required_field);
        }
    }
}
