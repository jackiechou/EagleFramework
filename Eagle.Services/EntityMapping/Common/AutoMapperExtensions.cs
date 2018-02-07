using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Eagle.Entities;
using Eagle.Services.Dtos;

namespace Eagle.Services.EntityMapping.Common
{
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// Ignore all unmapped properties.
        /// </summary>
        public static IMappingExpression<TSource, TDestination> IgnoreUnmapped<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> expression)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var sourceType = typeof (TSource);
            var destinationProperties = typeof (TDestination).GetProperties(flags);

            foreach (var property in destinationProperties)
            {
                if (sourceType.GetProperty(property.Name, flags) == null)
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }
            return expression;
        }

        //public static void InheritMapping<TSource, TDestination>(
        //    this IMappingExpression<TSource, TDestination> mappingExpression,
        //    Action<InheritMappingExpresssion<TSource, TDestination>> action)
        //{
        //    InheritMappingExpresssion<TSource, TDestination> x =
        //        new InheritMappingExpresssion<TSource, TDestination>(mappingExpression);
        //    action(x);
        //    x.ConditionsForAll();
        //}
 
        //public class InheritMappingExpresssion<TSource, TDestination>
        //{
        //    private readonly IMappingExpression<TSource, TDestination> _sourcExpression;

        //    public InheritMappingExpresssion(IMappingExpression<TSource, TDestination> sourcExpression)
        //    {
        //        _sourcExpression = sourcExpression;
        //    }

        //    public void IncludeSourceBase<TSourceBase>(
        //        bool ovverideExist = false)
        //    {
        //        Type sourceType = typeof (TSourceBase);
        //        Type destinationType = typeof (TDestination);
        //        if (!sourceType.IsAssignableFrom(typeof (TSource))) throw new NotSupportedException();
        //        Result(sourceType, destinationType);
        //    }

        //    public void IncludeDestinationBase<TDestinationBase>()
        //    {
        //        Type sourceType = typeof (TSource);
        //        Type destinationType = typeof (TDestinationBase);
        //        if (!destinationType.IsAssignableFrom(typeof (TDestination))) throw new NotSupportedException();
        //        Result(sourceType, destinationType);
        //    }

        //    public void IncludeBothBases<TSourceBase, TDestinatioBase>()
        //    {
        //        Type sourceType = typeof (TSourceBase);
        //        Type destinationType = typeof (TDestinatioBase);
        //        if (!sourceType.IsAssignableFrom(typeof (TSource))) throw new NotSupportedException();
        //        if (!destinationType.IsAssignableFrom(typeof (TDestination))) throw new NotSupportedException();
        //        Result(sourceType, destinationType);
        //    }

        //    internal void ConditionsForAll()
        //    {
        //        _sourcExpression.ForAllMembers(x => x.Condition(r => _conditions.All(c => c(r))));
        //    }

        //    private List<Func<ResolutionContext, bool>> _conditions = new List<Func<ResolutionContext, bool>>();

        //    private void Result(Type typeSource, Type typeDest)
        //    {
        //        _sourcExpression.BeforeMap((x, y) =>
        //        {
        //            Mapper.Map(x, y, typeSource, typeDest);
        //        });
        //        _conditions.Add(
        //            (r) => NotAlreadyMapped(typeSource, typeDest, r, typeof (TSource), typeof (TDestination)));
        //    }
        //}
    }
}
