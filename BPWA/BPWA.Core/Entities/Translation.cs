namespace BPWA.Core.Entities
{
    public class Translation : BaseEntity
    {
        public string Culture { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
