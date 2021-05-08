namespace BPWA.Core.Entities
{
    public interface IBaseEntity : IBaseEntity<int> { }

    public interface IBaseEntity<TKey> 
    {
        public TKey Id { get; set; }
    }
}
