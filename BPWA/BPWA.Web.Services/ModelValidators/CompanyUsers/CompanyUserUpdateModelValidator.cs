﻿using BPWA.Common.Resources;
using BPWA.Web.Services.Models;
using FluentValidation;

namespace BPWA.Web.Services.ModelValidators
{
    public class CompanyUserUpdateModelValidator : AbstractValidator<CompanyUserUpdateModel>
    {
        public CompanyUserUpdateModelValidator()
        {
            RuleFor(x => x.UserName).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.Email).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.FirstName).NotNull().WithMessage(Translations.Required_field);
            RuleFor(x => x.LastName).NotNull().WithMessage(Translations.Required_field);
        }
    }
}
