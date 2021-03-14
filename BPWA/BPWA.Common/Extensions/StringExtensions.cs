namespace BPWA.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string s)
        {
            return string.IsNullOrEmpty(s) ? string.Empty : $"{s[0].ToString().ToLower()}{s.Substring(1)}";
        }
    }
}
