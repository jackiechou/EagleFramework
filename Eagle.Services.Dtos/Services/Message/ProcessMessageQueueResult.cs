using System;
using System.Collections.Generic;

namespace Eagle.Services.Dtos.Services.Message
{
    public class ProcessMessageQueueResult : DtoBase
    {
        public int ProcessMessageCount { get; set; }
        public IList<Exception> Exceptions { get; set; }
    }
}
