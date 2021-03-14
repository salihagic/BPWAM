using BPWA.Common.Resources;

namespace BPWA.Common.Extensions
{
    public static class TranslationsHelper
    {
        public static string Translate(string key)
        {
            return Translations.ResourceManager.GetString(key, Translations.Culture);
        }
    }
}
