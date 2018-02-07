using System.Net.Http;
using Eagle.Services.Dtos.Common;
using Newtonsoft.Json;

namespace Eagle.Services.Exceptions
{
    public static class HttpResponseMessageExtensions
    {
        public static void EnsureSuccessResult(this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            //try
            //{
                var result = JsonConvert.DeserializeObject<Result>(response.Content.ReadAsStringAsync().Result,
                new JsonResultConverter());

                if (result != null)
                {
                    if (result is FailResult)
                    {
                        throw ((FailResult)result).ToException();
                    }
                }
            //}
            //catch (JsonReaderException e)
            //{
            //    //TODO: Remove this catch exception when all response from web api are Json
            //    //swallow JsonReaderException because response result might not be Json
            //}
            
        }
    }
}
