using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Eagle.Common.Extensions
{
    public static class HttpExtensions
    {
        //Uri url = new Uri("http://localhost/rest/something/browse").AddQuery("page", "0").AddQuery("pageSize", "200");
        public static Uri AddQuery(this Uri uri, string name, string value)
        {
            var ub = new UriBuilder(uri);

            // decodes urlencoded pairs from uri.Query to HttpValueCollection
            var httpValueCollection = HttpUtility.ParseQueryString(uri.Query);
            httpValueCollection.Add(name, value);

            // this code block is taken from httpValueCollection.ToString() method
            // and modified so it encodes strings with HttpUtility.UrlEncode
            if (httpValueCollection.Count == 0)
                ub.Query = String.Empty;
            else
            {
                var sb = new StringBuilder();

                for (int i = 0; i < httpValueCollection.Count; i++)
                {
                    string text = httpValueCollection.GetKey(i);
                    {
                        text = HttpUtility.UrlEncode(text);

                        string val = (text != null) ? (text + "=") : string.Empty;
                        string[] vals = httpValueCollection.GetValues(i);

                        if (sb.Length > 0)
                            sb.Append('&');

                        if (vals == null || vals.Length == 0)
                            sb.Append(val);
                        else
                        {
                            if (vals.Length == 1)
                            {
                                sb.Append(val);
                                sb.Append(HttpUtility.UrlEncode(vals[0]));
                            }
                            else
                            {
                                for (int j = 0; j < vals.Length; j++)
                                {
                                    if (j > 0)
                                        sb.Append('&');

                                    sb.Append(val);
                                    sb.Append(HttpUtility.UrlEncode(vals[j]));
                                }
                            }
                        }
                    }
                }

                ub.Query = sb.ToString();
            }

            return ub.Uri;
        }
        public static string ToQueryString(this Dictionary<string, string> source)
        {
            return String.Join("&", source.Select(kvp =>
                $"{HttpUtility.UrlEncode(kvp.Key)}={HttpUtility.UrlEncode(kvp.Value)}").ToArray());
        }

        public static string ToQueryString(this NameValueCollection source)
        {
            return String.Join("&", source.Cast<string>().Select(key =>
                $"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(source[key])}").ToArray());
        }

        #region Url Extension

        public static string AddToQueryString(this string url, params object[] keysAndValues)
        {
            return UpdateQueryString(url, q =>
            {
                for (var i = 0; i < keysAndValues.Length; i += 2)
                {
                    q.Set(keysAndValues[i].ToString(), keysAndValues[i + 1].ToString());
                }
            });
        }

        public static string RemoveFromQueryString(this string url, params string[] keys)
        {
            return UpdateQueryString(url, q =>
            {
                foreach (var key in keys)
                {
                    q.Remove(key);
                }
            });
        }

        public static string UpdateQueryString(string url, Action<NameValueCollection> func)
        {
            var urlWithoutQueryString = url.Contains('?') ? url.Substring(0, url.IndexOf('?')) : url;
            var queryString = url.Contains('?') ? url.Substring(url.IndexOf('?')) : null;
            var query = HttpUtility.ParseQueryString(queryString ?? string.Empty);

            func(query);

            return urlWithoutQueryString + (query.Count > 0 ? "?" : string.Empty) + query;
        }
        #endregion
    }
}
