namespace BPWA.Core.Entities
{
    public class Translation : 
        BaseEntity, 
        IBaseEntity
    {
        public string Culture { get; set; }
        public string Key { get; set; }
        public string KeyHash { get; set; }
        public string Value { get; set; }
    }
}
