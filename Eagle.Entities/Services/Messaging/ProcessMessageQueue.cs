using System;
using System.Collections.Generic;

namespace Eagle.Entities.Services.Messaging
{
    public class ProcessMessageQueue : EntityBase
    {
        public int ProcessMessageCount { get; set; }
        public IList<Exception> Exceptions { get; set; }
    }
}
