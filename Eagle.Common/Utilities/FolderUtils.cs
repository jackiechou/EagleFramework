using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace Eagle.Common.Utilities
{
    public static class FolderUtils
    {
        public static string GetMappedDirectory(string virtualDirectory)
        {
            if (string.IsNullOrEmpty(virtualDirectory)) return null;
            string mappedDir = string.Empty;
            try
            {
                if (String.IsNullOrEmpty(virtualDirectory))
                    mappedDir = AddTrailingSlash(Path.GetFullPath(virtualDirectory));
            }
            catch (Exception exc)
            {
                exc.ToString();
            }
            return mappedDir;
        }
        public static void WriteStream(HttpResponse objResponse, Stream objStream)
        {
            byte[] bytBuffer = new byte[10000];

            try
            {

                long lngDataToRead = objStream.Length;
                while (lngDataToRead > 0)
                {
                    if (objResponse.IsClientConnected)
                    {
                        int intLength = objStream.Read(bytBuffer, 0, 10000);
                        objResponse.OutputStream.Write(bytBuffer, 0, intLength);
                        objResponse.Flush();

                        lngDataToRead = lngDataToRead - intLength;
                    }
                    else
                    {
                        lngDataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                objResponse.Write("Error : " + ex.Message);
            }
            finally
            {
                if (objStream != null)
                {
                    objStream.Close();
                    objStream.Dispose();
                }
            }
        }
        public static string AddTrailingSlash(string strSource)
        {
            if (!strSource.EndsWith("\\"))
                strSource = strSource + "\\";
            return strSource;
        }
        public static string RemoveTrailingSlash(string strSource)
        {
            if (String.IsNullOrEmpty(strSource))
                return "";
            if (strSource.Substring(strSource.Length - 1, 1) == "\\" || strSource.Substring(strSource.Length - 1, 1) == "/")
            {
                return strSource.Substring(0, strSource.Length - 1);
            }
            else
            {
                return strSource;
            }
        }
        public static string StripFolderPath(string strOrigPath)
        {
            if (strOrigPath.IndexOf("\\", StringComparison.Ordinal) != -1)
            {
                return Regex.Replace(strOrigPath, "^0\\\\", "");
            }
            else
            {
                return strOrigPath.StartsWith("0") ? strOrigPath.Substring(1) : strOrigPath;
            }
        }
        public static string FormatFolderPath(string strFolderPath)
        {
            if (String.IsNullOrEmpty(strFolderPath))
            {
                return "";
            }
            else
            {
                if (strFolderPath.EndsWith("/"))
                {
                    return strFolderPath;
                }
                else
                {
                    return strFolderPath + "/";
                }
            }
        }
    }
}
