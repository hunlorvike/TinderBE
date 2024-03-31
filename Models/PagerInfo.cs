namespace Tinder_Admin.Models
{
    public class PagerInfo
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int FirstPage { get; set; }

        public int LastPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalRecords { get; set; }

        public int NextPage { get; set; }

        public int PreviousPage { get; set; }

        public bool ShouldShow => TotalRecords > PageSize;

    }
}
