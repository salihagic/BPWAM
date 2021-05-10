using BPWA.Common.Attributes;

namespace BPWA.DAL.Models
{
    public class ConfigurationDTO :
        BaseSoftDeletableDTO,
        IBaseDTO
    {
        [Translatable]
        public string AboutUs { get; set; }
        public string WebVersion { get; set; }
        public string ApiVersion { get; set; }
        public string MobileVersion { get; set; }
    }
}
