using AutoMapper;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Infrastructure.Extension
{
    public static class MapPagedList
    {
        public static IPagedList<TDestination> ToMappedPagedList<TSource,TDestination>(this IPagedList<TSource> list)
        {
            IEnumerable<TDestination> sourceList = Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(list);
            IPagedList<TDestination> pagedResult = new StaticPagedList<TDestination>(sourceList, list.GetMetaData());

            return pagedResult;
        }
    }
}
