using System;
using System.Collections.Generic;
using System.Linq;

namespace Eagle.Services.Dtos.Common
{
    public static class DtoExtensions
    {
        public static TListResult ToListResult<TDto, TItemResult, TListResult>(this IEnumerable<TDto> source, Func<TDto, IEnumerable<EntityAction>> getDtoActions = null, Func<IEnumerable<TItemResult>, IEnumerable<EntityAction>> getListActions = null)
            where TDto : DtoBase
            where TItemResult : ItemResult<TDto>, new()
            where TListResult : ListResult<TDto, TItemResult>, new()
        {
            var itemResults = ToItemResults<TDto, TItemResult>(source, getDtoActions).ToList();
            var listResult = new TListResult { Data = itemResults, EntityActions = getListActions != null ? getListActions(itemResults) : new List<EntityAction>() };
            return listResult;
        }

        public static TListResult ToListResult<TDto, TItemResult, TListResult>(this IEnumerable<TDto> source, Func<IEnumerable<EntityAction>> getDtoActions, Func<IEnumerable<EntityAction>> getListActions)
            where TDto : DtoBase
            where TItemResult : ItemResult<TDto>, new()
            where TListResult : ListResult<TDto, TItemResult>, new()
        {
            var itemResults = ToItemResults<TDto, TItemResult>(source, getDtoActions).ToList();
            var listResult = new TListResult { Data = itemResults, EntityActions = getListActions != null ? getListActions() : new List<EntityAction>() };
            return listResult;
        }

        private static IEnumerable<TItemResult> ToItemResults<TDto, TItemResult>(this IEnumerable<TDto> source, Func<TDto, IEnumerable<EntityAction>> getDtoActions = null)
            where TDto : DtoBase
            where TItemResult : ItemResult<TDto>, new()
        {
            foreach (var entry in source)
            {
                var itemResult = entry.ToItemResult<TDto, TItemResult>(getDtoActions: getDtoActions);
                yield return itemResult;
            }
        }

        private static IEnumerable<TItemResult> ToItemResults<TDto, TItemResult>(this IEnumerable<TDto> source, Func<IEnumerable<EntityAction>> getDtoActions = null)
            where TDto : DtoBase
            where TItemResult : ItemResult<TDto>, new()
        {
            foreach (var entry in source)
            {
                var itemResult = entry.ToItemResult<TDto, TItemResult>(getDtoActions);
                yield return itemResult;
            }
        }

        public static TResult ToItemResult<TDto, TResult>(this TDto source, TResult result = null, Func<TDto, IEnumerable<EntityAction>> getDtoActions = null) 
            where TDto : DtoBase
            where TResult : ItemResult<TDto>, new()
        {
            var entityActions = getDtoActions != null ? getDtoActions(source) : new List<EntityAction>();
            var itemResult = result ?? new TResult { Data = source, EntityActions = entityActions };
            return itemResult;
        }

        public static TResult ToItemResult<TDto, TResult>(this TDto source, Func<IEnumerable<EntityAction>> getDtoActions, TResult result = null)
            where TDto : DtoBase
            where TResult : ItemResult<TDto>, new()
        {
            var entityActions = getDtoActions != null ? getDtoActions() : new List<EntityAction>();
            var itemResult = result ?? new TResult { Data = source, EntityActions = entityActions };
            return itemResult;
        }
    }
}
