using System;

namespace Eagle.Infrastructure.Messaging
{
    [Flags]
    public enum MessageType
    {
        System = 1,
        User = 2
    }
}
