using System.Collections.Generic;

namespace Global.Models.PagedList
{
    public interface IPagedList<T>
    {
        List<T> Subset { get; set; }

        int Count { get; }

        int PageCount { get; set; }

        int TotalItemCount { get; set; }

        int Skip { get; set; }

        int Take { get; set; }

        bool HasPreviousPage { get; set; }

        bool HasNextPage { get; set; }

        bool IsFirstPage { get; set; }

        bool IsLastPage { get; set; }
    }
}
