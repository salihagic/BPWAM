using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BPWA.Common.Resources
{
    public class TranslationOptions
    {
        public static List<LanguageModel> SupportedLanguages => new List<LanguageModel>
        {
            new LanguageModel
            {
                Name = "English",
                CultureInfo = new CultureInfo("en-US"),
                IconPath = "/assets/media/flags/flag-us.png"
            },
        };

        public static LanguageModel DefaultLanguage => SupportedLanguages[0];
        public static List<CultureInfo> SupportedCultures => SupportedLanguages.Select(x => x.CultureInfo).ToList();
    }

    public class LanguageModel
    {
        public string Name { get; set; }
        public CultureInfo CultureInfo { get; set; }
        public string IconPath { get; set; }
    }
}
