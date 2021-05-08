namespace BPWA.DAL.Models
{
    public class BaseSoftDeletableDTO :
        BaseSoftDeletableDTO<int>,
        IBaseSoftDeletableDTO
        { }

    public class BaseSoftDeletableDTO<TKey> : 
        IBaseSoftDeletableDTO
    {
        public TKey Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
