namespace BPWA.DAL.Models
{
    public class CurrencyDTO :
        BaseSoftDeletableDTO,
        IBaseDTO
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
    }
}
