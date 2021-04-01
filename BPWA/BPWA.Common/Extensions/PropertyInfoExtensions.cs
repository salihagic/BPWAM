using BPWA.Common.Attributes;
using System.Linq;
using System.Reflection;

namespace BPWA.Common.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool IsTranslatable(this PropertyInfo prop)
        {
            return prop.GetCustomAttributes(true)
                       .Any(x => x.GetType() == typeof(TranslatableAttribute));
        }
    }
}
