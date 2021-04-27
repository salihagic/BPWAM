namespace BPWA.DAL.Models
{
    public class UserTokenDTO : BaseDTO, IBaseDTO
    {
        public string UserId { get; set; }
        public string LoginProvider { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
