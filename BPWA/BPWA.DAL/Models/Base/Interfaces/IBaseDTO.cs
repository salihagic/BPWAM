namespace BPWA.DAL.Models
{
    public interface IBaseDTO : IBaseDTO<int> { }

    public interface IBaseDTO<TKey>
    {
        public TKey Id { get; set; }
    }
}
