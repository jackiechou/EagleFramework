using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Entities;
using Eagle.Services.Dtos;

namespace Eagle.Services.EntityMapping.Common
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TTarget> ToEntities<TSource, TTarget>(this IEnumerable<TSource> sourceEntries, Func<TSource, TTarget, TTarget> postMap = null)
            where TSource : DtoBase
            where TTarget : EntityBase
        {
            return sourceEntries.Select(sourceEntry => sourceEntry.ToEntity(null, postMap));
        }

        public static IEnumerable<TTarget> ToDtos<TSource, TTarget>(this IEnumerable<TSource> sourceEntries, Func<TSource, TTarget, TTarget> postMap = null)
            where TSource : EntityBase
            where TTarget : DtoBase
        {
            return sourceEntries.Select(sourceEntry => sourceEntry.ToDto(postMap));
        }
    }
}