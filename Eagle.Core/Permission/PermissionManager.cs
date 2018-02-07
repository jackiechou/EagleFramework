using System;

namespace Eagle.Core.Permission
{
    public static class PermissionManager
    {
        public static PermissionLevel ToPermission(this string source)
        {
            PermissionLevel result;
            Enum.TryParse(source, true, out result);
            return result;
        }
    }
}
