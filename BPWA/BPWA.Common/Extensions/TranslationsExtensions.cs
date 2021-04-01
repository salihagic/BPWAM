using BPWA.Common.Resources;
using System.Collections.Generic;

namespace BPWA.Common.Extensions
{
    public static class TranslationsHelper
    {
        public static string Translate(string key)
        {
            return Translations.ResourceManager.GetString(key, Translations.Culture);
        }

        public static List<string> GetTranslatableProps<T>(this T element)
        {
            if (element == null)
                return new List<string>();

            var translationKeys = new List<string>();

            foreach (var prop in element.GetType().GetProperties())
            {
                var propValue = prop.GetValue(element);

                if (propValue != null)
                {
                    if (prop.IsTranslatable() && prop.PropertyType == typeof(string))
                    {
                        translationKeys.Add(propValue as string);
                    }
                    else if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                    {
                        var subClassProps = propValue.GetTranslatableProps();

                        if (subClassProps.IsNotEmpty())
                        {
                            translationKeys.AddRange(subClassProps);
                        }
                    }
                }
            }

            return translationKeys;
        }

        public static T SetTranslatableProps<T>(this T element, Dictionary<string, string> translations)
        {
            if (element == null)
                return default(T);

            foreach (var prop in element.GetType().GetProperties())
            {
                var propValue = prop.GetValue(element);

                if (propValue != null)
                {
                    if (prop.IsTranslatable() && prop.PropertyType == typeof(string))
                    {
                        var translatedPropValue = translations.GetValueOrDefault(propValue as string);

                        if (translatedPropValue.HasValue())
                            prop.SetValue(element, translatedPropValue);
                    }
                    else if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                    {
                       propValue.SetTranslatableProps(translations);
                    }
                }
            }

            return element;
        }
    }
}
