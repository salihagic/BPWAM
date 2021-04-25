using System.Security.Cryptography;
using System.Text;

namespace BPWA.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string s)
        {
            return string.IsNullOrEmpty(s) ? string.Empty : $"{s[0].ToString().ToLower()}{s.Substring(1)}";
        }

        public static bool IsEmpty(this string s) => string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s);
        public static bool IsNotEmpty(this string s) => !s.IsEmpty();
        public static bool HasValue(this string s) => !string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s);

        public static string Base64Encode(this string s)
        {
            if (!s.HasValue())
                return string.Empty;

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(s);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string s)
        {
            if (!s.HasValue())
                return string.Empty;

            var base64EncodedBytes = System.Convert.FromBase64String(s);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string GetHashString(this string s)
        {
            StringBuilder hash = new StringBuilder();
            foreach (byte b in s.GetHash())
                hash.Append(b.ToString("X2"));

            return hash.ToString();
        }

        public static byte[] GetHash(this string s)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(s));
        }
    }
}
