namespace BPWA.DAL.Models.Translations
{
    public class TranslationCacheModel
    {
        public string Culture { get; set; }
        public string Key { get; set; }
        public string KeyHash { get; set; }
        public string Value { get; set; }

        public string CacheKey => $"{KeyHash}-{Culture}";
    }
}
