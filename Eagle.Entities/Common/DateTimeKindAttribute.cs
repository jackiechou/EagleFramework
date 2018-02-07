using System;
using System.Linq;
using System.Reflection;

namespace Eagle.Entities.Common
{
    /// <summary>
    /// Allows us to decorate DateTime and DateTime? to set the DateTimeKind on the fly.
    /// ie. [DateTimeKind(DateTimeKind.Utc)]
    /// 
    /// If attribute does not exist for a DateTime or DateTime? for a given Entity, it will be marked as Utc
    /// 
    /// Modified from:
    /// http://stackoverflow.com/questions/4648540/entity-framework-datetime-and-utc
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DateTimeKindAttribute : Attribute
    {
        private readonly DateTimeKind _kind;

        public DateTimeKindAttribute(DateTimeKind kind)
        {
            _kind = kind;
        }

        public DateTimeKind Kind
        {
            get { return _kind; }
        }

        public static void Apply(object entity)
        {
            // Only allow Entities that inherit from CleanEntityBase
            if (entity == null || !typeof(EntityBase).IsAssignableFrom(entity.GetType()))
                return;

            var properties = entity.GetType().GetProperties()
                .Where(x => x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?));

            foreach (var property in properties)
            {
                var dt = property.PropertyType == typeof(DateTime?)
                    ? (DateTime?)property.GetValue(entity)
                    : (DateTime)property.GetValue(entity);
                if (dt == null)
                    continue;

                var attr = property.GetCustomAttribute<DateTimeKindAttribute>();

                // Set DateTimeKind to Utc by default, unless the DateTime is decorated with DateTimeKindAttribute
                if (attr == null)
                {
                    property.SetValue(entity, DateTime.SpecifyKind(dt.Value, DateTimeKind.Utc));
                }
                else
                {
                    property.SetValue(entity, DateTime.SpecifyKind(dt.Value, attr.Kind));
                }

            }
        }
    }
}
