using AutoMapper;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class PageListMapperConverter<TDestination> : ITypeConverter<object, IPagedList<TDestination>>
    {
        IPagedList<TDestination> ITypeConverter<object, IPagedList<TDestination>>.Convert(dynamic source, IPagedList<TDestination> destination, ResolutionContext context)
        {
            if (source == null) return default;

            destination = destination ?? new PagedList<TDestination>(source.Skip ?? 0, source.Take ?? 10, source.TotalItemCount ?? 0);
            destination.Subset = context.Mapper.Map<List<TDestination>>(source.Subset);
            return destination;
        }
    }
}
