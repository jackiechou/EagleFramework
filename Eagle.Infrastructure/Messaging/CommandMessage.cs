using System;

namespace Eagle.Infrastructure.Messaging
{
    [Serializable]
    public abstract class CommandMessage : MessageBase, ICommand
    {
        public int Version { get; set; }
    }
}
