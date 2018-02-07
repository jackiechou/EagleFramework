using System.Collections.Generic;

namespace Eagle.Services.Dtos.Common
{
    public class DataResult: Result
    {
        public string Message { get; set; }
        public dynamic Data { get; set; }
        public List<Error> Errors { get; set; }
    }
}
