using System;

namespace Eagle.Common.Extensions
{
    public static class GuidExtensions
    {
        public static bool IsEmpty(this Guid source)
        {
            return source == Guid.Empty;
        }

        public static bool IsNullOrEmpty(this Guid? source)
        {
            return source == null || source.Value.IsEmpty();
        }

        public static bool IsEmptyGuid(this Guid value)
        {
            return value.Equals(Guid.Empty);
        }

        public static bool IsGuid(this string value)
        {
            try
            {
                Guid test;
                return Guid.TryParse(value, out test);
            }
            catch (Exception)
            {
                // ignored
            }
            return false;
        }

        public static Guid ToGuid(this string source)
        {
            return source.IsNullOrEmpty() ? Guid.Empty : new Guid(source);
        }
    }
}
