using System.Collections.Generic;
using Newtonsoft.Json;

namespace Eagle.Services.Dtos.Common
{
    public class CaptchaRequest
    {
        public string PublicKey { get; set; }
        public string Response { get; set; }
    }
    public class CaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}
