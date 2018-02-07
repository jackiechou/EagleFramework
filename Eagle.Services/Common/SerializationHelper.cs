using System;
using Newtonsoft.Json;

namespace Eagle.Services.Common
{
    /// <summary>
    /// Serialized helper class
    /// </summary>
    public static class SerializationHelper
    {
        /// <summary>
        /// Try to deserialize the JSON content
        /// </summary>
        /// <typeparam name="T">Expected Type T after deserialize</typeparam>
        /// <param name="content">JSON Content to be deserialize</param>
        /// <param name="t">Type T object value</param>
        /// <returns>true/false</returns>
        public static bool TryDeserialize<T>(string content, out T t) where T : class
        {
            t = null;
            try
            {
                t = JsonConvert.DeserializeObject<T>(content);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
    }
}
