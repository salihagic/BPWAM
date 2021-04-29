namespace BPWA.DAL.Models
{
    public interface IBaseSearchModel
    {
        public bool? IsDeleted { get; set; }
        public Pagination Pagination { get; set; }
    }   
    
    public class BaseSearchModel : IBaseSearchModel
    {
        public bool? IsDeleted { get; set; } = false;
        public Pagination Pagination { get; set; } = new Pagination();
    }
}
