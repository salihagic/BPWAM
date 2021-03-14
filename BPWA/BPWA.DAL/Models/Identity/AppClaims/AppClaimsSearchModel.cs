namespace BPWA.DAL.Models
{
    public class AppClaimsSearchModel
    {
        public string Name { get; set; }
        public Pagination Pagination { get; set; } = new Pagination();
    }
}
