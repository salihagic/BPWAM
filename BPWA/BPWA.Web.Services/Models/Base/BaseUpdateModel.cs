namespace BPWA.Web.Services.Models
{
    public interface IBaseUpdateModel : IBaseUpdateModel<int> {}

    public class BaseUpdateModel : BaseUpdateModel<int>, IBaseUpdateModel {}

    public interface IBaseUpdateModel<TId>
    {
        public TId Id { get; set; }
    }

    public class BaseUpdateModel<TId> : IBaseUpdateModel<TId>
    {
        public TId Id { get; set; }
    }
}
