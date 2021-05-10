namespace BPWA.DAL.Models
{
    public class LanguageDTO : 
        BaseSoftDeletableDTO, 
        IBaseDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
