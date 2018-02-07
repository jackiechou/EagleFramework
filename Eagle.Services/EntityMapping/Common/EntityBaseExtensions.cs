using System;
using AutoMapper;
using Eagle.Entities;
using Eagle.Services.Dtos;

namespace Eagle.Services.EntityMapping.Common
{
    public static class EntityBaseExtensions
    {
        public static TTarget ToDto<TSource, TTarget>(this TSource source, Func<TSource, TTarget, TTarget> postMap = null)
            where TSource : EntityBase
            where TTarget : DtoBase
        {
            try
            {
                if (source == null) return null;

                var config = new MapperConfiguration(cfg => { cfg.CreateMap<TSource, TTarget>(); });
                IMapper mapper = config.CreateMapper();

                var target = mapper.Map<TSource, TTarget>(source);

                var result = (postMap != null) ? postMap(source, target) : target;
                return result;
            }
            catch (AutoMapperMappingException autoMapperException)
            {
                if (autoMapperException.InnerException != null) throw autoMapperException.InnerException;
                return null;
                // this will break your call stack
                // you may not know where the error is called and rather
                // want to clone the InnerException or throw a brand new Exception
            }
        }
    }
}
