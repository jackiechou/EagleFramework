using System;

namespace Eagle.Services.Service.MessageBroadCaster
{
    public class BroadCastListenerEventArgs : EventArgs
    {
        public string Message { get; internal set; }
    }
}
