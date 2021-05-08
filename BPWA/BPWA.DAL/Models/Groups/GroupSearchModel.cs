namespace BPWA.DAL.Models
{
    public class GroupSearchModel : BaseSearchModel
    {
        public string SearchTerm { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
    }
}
