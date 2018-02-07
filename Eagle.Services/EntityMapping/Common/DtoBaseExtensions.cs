using System;
using AutoMapper;
using Eagle.Entities;
using Eagle.Services.Dtos;

namespace Eagle.Services.EntityMapping.Common
{
    internal static class DtoBaseExtensions
    {
        public static TTarget ToEntity<TSource, TTarget>(this TSource source, TTarget target = null, Func<TSource, TTarget, TTarget> postMap = null)
            where TSource : DtoBase
            where TTarget : EntityBase
        {
            var config = new MapperConfiguration(cfg => {cfg.CreateMap<TSource, TTarget>();});
            IMapper mapper = config.CreateMapper();

            var mappedTarget = target == null ? mapper.Map<TSource, TTarget>(source) : mapper.Map(source, target);

            var result = (postMap != null) ? postMap(source, mappedTarget) : mappedTarget;
            return result;
        }
    }
}
