using System;

namespace Eagle.Infrastructure.Messaging
{
    [Serializable]
    public abstract class EventMessage : MessageBase, IEvent
    {
        public int Version { get; set; }
    }
}
