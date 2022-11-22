namespace Global.Models.PagedList
{
    public class PagedListMetaData
    {
        public PagedListMetaData()
        {
        }

        public int PageCount { get; set; }

        public int TotalItemCount { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }

        public bool IsFirstPage { get; set; }

        public bool IsLastPage { get; set; }
    }
}
