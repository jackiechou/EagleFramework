using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Eagle.Services.SystemManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Eagle.Services.Service
{
    public class RestService : BaseService
    {
        public ILogService LogService { get; set; }
        private readonly IDictionary<string, string> _contentHeaders;

        public RestService(IDictionary<string, string> contentHeaders)
        {
            if (contentHeaders == null)
            {
                throw new ArgumentNullException(nameof(contentHeaders));
            }
            _contentHeaders = contentHeaders;
        }

        public RestService()
        {
            _contentHeaders = new Dictionary<string, string>();
        }

        public async Task<string> DeleteAsync(JObject json, string apiUrl)
        {
            try
            {
                var data = json.ToString(Formatting.None, null);

                Logger.Debug($"Request API Delete: {data}");

                var content = new StringContent(data);
                content.Headers.ContentType = new MediaTypeHeaderValue(("application/json"));
                content.Headers.ContentLength = json.ToString(Formatting.None, null).Length;

                var aHandler = new HttpClientHandler {ClientCertificateOptions = ClientCertificateOption.Automatic};
                var httpClient = new HttpClient(aHandler);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                AddContentHeaders(httpClient);

                var request = new HttpRequestMessage(HttpMethod.Delete, apiUrl)
                {
                    Content = content
                };

                var response = await httpClient.SendAsync(request);
                var message = response.EnsureSuccessStatusCode();
                if (message.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    Logger.Debug($"Response API Delete: {apiUrl}, {responseString}");

                    return responseString;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"ERROR: Delete API: {ex}");
            }

            return string.Empty;
        }

        public async Task<string> GetAsync(Dictionary<string, object> json, string apiUrl)
        {
            try
            {
                var aHandler = new HttpClientHandler {ClientCertificateOptions = ClientCertificateOption.Automatic};
                var httpClient = new HttpClient(aHandler);
                AddContentHeaders(httpClient);

                var uri = GetUriWithparameters(apiUrl, json);

                Logger.Debug($"Request API Get: {apiUrl}, {uri}");

                var response = await httpClient.GetAsync(uri);
                var message = response.EnsureSuccessStatusCode();
                if (message.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    Logger.Debug($"Response API GET: {responseString}");

                    return responseString;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"ERROR Get API: {ex}");
            }

            return string.Empty;
        }

        public async Task<string> PostAsync(JObject json, string apiUrl)
        {
            try
            {
                var data = json.ToString(Formatting.None, null);

                Logger.Debug($"Request API Post: {apiUrl}, {data}");

                var content = new StringContent(data);
                content.Headers.ContentType = new MediaTypeHeaderValue(("application/json"));

                var aHandler = new HttpClientHandler {ClientCertificateOptions = ClientCertificateOption.Automatic};
                var httpClient = new HttpClient(aHandler);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                AddContentHeaders(httpClient);

                var uri = new Uri($"{apiUrl}");
                var response = await httpClient.PostAsync(uri, content);
                var message = response.EnsureSuccessStatusCode();
                if (message.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    Logger.Debug($"Response API POST: {responseString}");

                    return responseString;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"ERROR Post API: {ex}");
            }

            return string.Empty;
        }

        public async Task<string> PutAsync(JObject json, string apiUrl)
        {
            try
            {
                var data = json.ToString(Formatting.None, null);

                Logger.Debug($"Request API Put: {apiUrl}, {data}");

                var content = new StringContent(data);
                content.Headers.ContentType = new MediaTypeHeaderValue(("application/json"));
                content.Headers.ContentLength = json.ToString(Formatting.None, null).Length;

                var aHandler = new HttpClientHandler {ClientCertificateOptions = ClientCertificateOption.Automatic};
                var httpClient = new HttpClient(aHandler);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                AddContentHeaders(httpClient);

                var uri = new Uri($"{apiUrl}");
                var response = await httpClient.PutAsync(uri, content);
                var message = response.EnsureSuccessStatusCode();
                if (message.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    Logger.Debug($"Response API Put: {responseString}");

                    return responseString;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"ERROR Put API: {ex}");
            }

            return string.Empty;
        }

        #region private methods

        private void AddContentHeaders(HttpClient httpClient)
        {
            foreach (var headerItem in _contentHeaders)
            {
                httpClient.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
            }
        }

        private string GetUriWithparameters(string uri, Dictionary<string, object> queryParams = null, int port = -1)
        {
            var builder = new UriBuilder(uri) {Port = port};
            if (null != queryParams && 0 < queryParams.Count)
            {
                var query = HttpUtility.ParseQueryString(builder.Query);
                foreach (var item in queryParams)
                {
                    if (item.Value != null && !string.IsNullOrEmpty(item.Value.ToString()))
                    {
                        query[item.Key] = item.Value.ToString();
                    }
                }
                builder.Query = query.ToString();
            }
            return builder.Uri.ToString();
        }

        #endregion private methods

    }
}
