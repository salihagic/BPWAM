namespace BPWA.Web.Services.Models
{
    public class CurrencyUpdateModel : BaseUpdateModel
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
    }
}
