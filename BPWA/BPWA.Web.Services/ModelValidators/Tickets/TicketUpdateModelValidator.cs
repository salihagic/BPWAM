using BPWA.Common.Resources;
using BPWA.Web.Services.Models;
using FluentValidation;

namespace BPWA.Web.Services.ModelValidators
{
    public class TicketUpdateModelValidator : AbstractValidator<TicketUpdateModel>
    {
        public TicketUpdateModelValidator()
        {
            RuleFor(x => x.Title).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.Description).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.TicketType).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.TicketStatus).NotNull().WithMessage(Translations.Required_field);
        }
    }
}
