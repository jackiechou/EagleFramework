using System;

namespace Eagle.Common.Extensions.EnumHelper
{
    public class EnumAttribute : Attribute
    {
        public EnumAttribute(string description)
        {
            Description = description;
        }

        public string Description { get; }
    }
}
