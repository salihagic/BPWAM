using BPWA.Common.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace BPWA.Api.Configuration
{
    public static class LocalizationConfiguration
    {
        public static IServiceCollection ConfigureLocalization(this IServiceCollection services)
        {
            services.AddLocalization(x => x.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(TranslationOptions.DefaultLanguage.CultureInfo);
                options.SupportedCultures = TranslationOptions.SupportedCultures;
                options.SupportedUICultures = TranslationOptions.SupportedCultures;
            });

            return services;
        }
    }
}
