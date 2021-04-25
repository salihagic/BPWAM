using BPWA.Common.Resources;
using BPWA.Web.Services.Models;
using FluentValidation;

namespace BPWA.Web.Services.ModelValidators
{
    public class NotificationAddModelValidator : AbstractValidator<NotificationAddModel>
    {
        public NotificationAddModelValidator()
        {
            RuleFor(x => x.Title).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.Description).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.NotificationType).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.NotificationDistributionType).NotNull().WithMessage(Translations.Required_field);
        }
    }
}
