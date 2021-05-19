using BPWA.Web.Services.ModelValidators;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace BPWA.Api.Configuration
{
    public static class FluentValidationConfiguration
    {
        public static IMvcBuilder ConfigureFluentValidation(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<FluentValidationReferenceClassWebServices>());

            return mvcBuilder;
        }
    }
}
