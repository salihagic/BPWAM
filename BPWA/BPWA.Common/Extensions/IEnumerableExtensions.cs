using System.Collections.Generic;
using System.Linq;

namespace BPWA.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> list) => list == null || list.Count() == 0;
        public static bool IsNotEmpty<T>(this IEnumerable<T> list) => !list.IsEmpty();

        public static bool IsEmpty<T>(this List<T> list) => list == null || list.Count == 0;
        public static bool IsNotEmpty<T>(this List<T> list) => !list.IsEmpty();
    }
}
