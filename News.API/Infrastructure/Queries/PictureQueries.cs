using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using News.API.Models;
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Global.Models.PagedList;
using News.API.Models.Domain;
using Global.Models.Filter;

namespace News.API.Infrastructure.Queries
{
    public interface IPictureQueries : IQueryRepository
    {
        IPagedList<PictureViewModel> GetPagedList(RequestFilterModel filterModel);
    }
    public class PictureQueries : QueryRepository<Models.Picture, int>, IPictureQueries
    {
        public PictureQueries(NewsDbContext context) : base(context)
        {
        }

        public IPagedList<PictureViewModel> GetPagedList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<PictureViewModel>(filterModel);
            return dapperExecution.ExecutePaginateQuery();
        }
    }
}
