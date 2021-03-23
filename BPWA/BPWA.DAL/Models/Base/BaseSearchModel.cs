namespace BPWA.DAL.Models
{
    public class BaseSearchModel
    {
        public bool? IsDeleted { get; set; } = false;
        public Pagination Pagination { get; set; } = new Pagination();
        virtual public bool IsDirty => false;
    }
}
