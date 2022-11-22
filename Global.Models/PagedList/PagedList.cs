using System;
using System.Collections.Generic;
using System.Linq;

namespace Global.Models.PagedList
{
    public class PagedList<T> : PagedListMetaData, IPagedList<T>
    {
        public List<T> Subset { get; set; }

        public PagedList()
        {
        }

        public PagedList(int skip, int take, int totalItemCount)
        {
            this.Subset = new List<T>();
            this.TotalItemCount = totalItemCount;
            this.Take = take;
            this.Skip = skip;
            this.PageCount = this.TotalItemCount > 0 ? (int)Math.Ceiling((double)this.TotalItemCount / (double)this.Take) : 0;
            bool flag = this.PageCount > 0 && this.Skip <= this.TotalItemCount;
            this.HasPreviousPage = flag && this.Skip >= this.Take;
            this.HasNextPage = flag && this.Skip + this.Take < this.TotalItemCount && this.Skip >= 0;
            this.IsFirstPage = flag && this.Skip == 0;
            this.IsLastPage = flag && this.Skip + this.Take >= this.TotalItemCount;
        }

        public PagedList(IQueryable<T> superset, int skip, int take)
            : this(skip, take, superset?.Count() ?? 0)
        {
            if (this.TotalItemCount <= 0 || superset == null)
                return;
            this.Subset.AddRange(skip == 0
                ? superset.Take(take).ToList()
                : superset.Skip(skip).Take(take).ToList());
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.Subset?.GetEnumerator();
        }

        public T this[int index] => this.Subset[index];

        public int Count => this.Subset?.Count ?? 0;
    }
}
