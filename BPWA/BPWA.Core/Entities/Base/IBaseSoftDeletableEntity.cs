namespace BPWA.Core.Entities
{
    public interface IBaseSoftDeletableEntity
    {
        public bool IsDeleted { get; set; }
    }
}
