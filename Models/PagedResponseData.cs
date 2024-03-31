namespace Tinder_Admin.Models
{
    public class PagedResponseData<DataType> : ResponseData<DataType> where DataType : class
    {
        public PagerInfo Pager { get; set; } = new PagerInfo();
    }
}
