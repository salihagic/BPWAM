using BPWA.Common.Resources;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BPWA.Web.Configuration
{
    public static class DataAnnotationsLocalizationConfiguration
    {
        public static IMvcBuilder ConfigureDataAnnotationsLocalization(this IMvcBuilder builder)
        {
            builder.AddDataAnnotationsLocalization(options =>
            {
                var assemblyName = new AssemblyName(typeof(TranslationOptions).Assembly.FullName);
                options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create("Resource", assemblyName.Name);
            });

            return builder;
        }
    }
}
