using BPWA.Common.Attributes;

namespace BPWA.Core.Entities
{
    public class Configuration : BaseSoftDeletableEntity
    {
        [Translatable]
        public string AboutUs { get; set; }
        public string WebVersion { get; set; }
        public string ApiVersion { get; set; }
        public string MobileVersion { get; set; }
    }
}
