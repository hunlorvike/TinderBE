namespace Tinder_Admin.Models
{
    public class QueryData
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public bool GetAllRecords { get; set; }
    }
}
