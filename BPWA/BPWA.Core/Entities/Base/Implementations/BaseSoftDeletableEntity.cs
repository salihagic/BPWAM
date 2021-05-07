namespace BPWA.Core.Entities
{
    public class BaseSoftDeletableEntity :
        BaseSoftDeletableEntity<int>,
        IBaseEntity,
        IBaseSoftDeletableEntity
        { }

    public class BaseSoftDeletableEntity<TKey> : 
        IBaseEntity<TKey>,
        IBaseSoftDeletableEntity
    {
        public TKey Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
