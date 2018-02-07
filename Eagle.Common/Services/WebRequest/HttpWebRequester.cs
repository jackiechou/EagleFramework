using System;
using System.IO;

namespace Eagle.Common.Services.WebRequest
{
    public enum RequestMethod
    {
        POST,
        GET
    }

    public abstract class HttpWebRequester
    {
        protected HttpWebRequester()
        {
            this.Timeout = 20000;
        }

        public int Timeout { get; set; }

        public virtual string SendRequest(string serviceUrl, string queryString, RequestMethod requestMethod)
        {
            string url = requestMethod == RequestMethod.POST ? serviceUrl : String.Format("{0}?{1}", serviceUrl, queryString);

            var request = System.Net.WebRequest.Create(url);

            request.Method = requestMethod.ToString();
            request.Timeout = this.Timeout;

            if (requestMethod == RequestMethod.POST)
            {
                request.ContentLength = queryString.Length;
                request.ContentType = "application/x-www-form-urlencoded";

                using (var writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(queryString);
                }
            }

            var response = request.GetResponse();

            using (var reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(28591)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
