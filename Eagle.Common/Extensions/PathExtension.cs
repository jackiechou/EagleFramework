using System;
using System.IO;
using System.Web;

namespace Eagle.Common.Extensions
{
    public static class PathExtension
    {
        public static string CanonicalCombine(string basePath, string path)

        {

            if (string.IsNullOrEmpty(basePath) || string.IsNullOrEmpty(path))

                throw new ArgumentNullException();

            basePath = HttpUtility.UrlDecode(basePath);

            path = HttpUtility.UrlDecode(path);

            // Check for invalid characters

            if (path.IndexOfAny(Path.GetInvalidPathChars()) > -1)

                throw new FileNotFoundException("Path not valid");

            // Use Path.Combine
            string filePath = Path.Combine(basePath, path);


            // Check the composed path
            if (!filePath.StartsWith(basePath))

                throw new FileNotFoundException("Path not valid");

            return filePath;
        }
    }
}
