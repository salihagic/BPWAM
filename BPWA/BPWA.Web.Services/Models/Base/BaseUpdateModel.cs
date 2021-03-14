namespace BPWA.Web.Services.Models
{
    public class BaseUpdateModel : BaseUpdateModel<int> {}

    public class BaseUpdateModel<TId>
    {
        public TId Id { get; set; }
    }
}
