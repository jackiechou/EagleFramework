using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Eagle.Common.Services.Ftp
{
    public class FtpFileTransfer
    {
        private string _hostname;
        private string _password;
        private string _username;
        private bool _enableSsl;

        public FtpFileTransfer(string host, string username, string password, bool enableSsl = false)
        {
            _hostname = host;
            _username = username;
            _password = password;
            _enableSsl = enableSsl;
        }
        /// <summary>
        /// Create Directory
        /// </summary>
        /// <param name="dir">dir</param>
        /// <returns></returns>
        public async Task<string> CreateDirectory(string dir)
        {
            var list = dir.Split('/').ToList();
            list.RemoveAll(string.IsNullOrWhiteSpace);

            var parts = list.ToArray();

            for (int i = 1; i <= parts.Length; i++)
            {
                dir = string.Join("/", parts, 0, i);
                await TypeCreateDirectoryAsync(dir);
            }

            return dir;
        }
        /// <summary>
        /// Checks to see if the file already exists on the server
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<bool> FileExistsAsync(string dir, string filename)
        {
            var request = CreateRequest(dir + "/" + filename);
            //request.Credentials = new NetworkCredential("user", "pass");
            request.Method = WebRequestMethods.Ftp.GetDateTimestamp;

            try
            {
                FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode ==
                    FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks to see if the file already exists on the server
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool FileExists(string dir, string filename)
        {
            var request = CreateRequest(dir + "/" + filename);
            //request.Credentials = new NetworkCredential("user", "pass");
            request.Method = WebRequestMethods.Ftp.GetDateTimestamp;

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode ==
                    FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Ftp Create Directory
        /// </summary>
        /// <param name="dirpath">dirpath</param>
        /// <returns></returns>
        private async Task<bool> TypeCreateDirectoryAsync(string dirpath)
        {
            var ftp = CreateRequest(dirpath);

            ftp.Method = WebRequestMethods.Ftp.MakeDirectory;

            try
            {
                using (var response = await ftp.GetResponseAsync())
                using (var stream = response.GetResponseStream())
                using (var sr = new StreamReader(stream))
                {
                    sr.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }



        /// <summary>
        /// Create Request
        /// </summary>
        /// <param name="path">dir</param>
        /// <returns></returns>
        private FtpWebRequest CreateRequest(params string[] parts)
        {
            // e.g. format: ftp://ftp.redcat.com.au/FTP/SHERPA/MADE_ESTABLISHMENTS/PAYSLIPS

            var uri = new Uri("ftp://" + _hostname);

            foreach (var part in parts)
            {
                uri = new Uri(uri, part);
            }

            var request = FtpWebRequest.Create(uri) as FtpWebRequest;
            request.EnableSsl = _enableSsl;
            //request.EnableSsl = true;
            request.Credentials = new NetworkCredential(_username, _password);

            return request;
        }

        /// <summary>
        /// Upload Csv
        /// </summary>
        /// <param name="dir">dir</param>
        /// <param name="filename">filename</param>
        /// <param name="contents">csv</param>
        public bool UploadFile(string dir, string filename, string contents)
        {
            //try
            //{
                var ftp = CreateRequest(dir + "/" + filename);
                ftp.Method = WebRequestMethods.Ftp.UploadFile;
                ftp.UseBinary = false;
                ftp.KeepAlive = false;
                var bytes = System.Text.UnicodeEncoding.UTF8.GetBytes(contents);
                using (var stream = ftp.GetRequestStream())
                    stream.Write(bytes, 0, bytes.Length);
            //}
            //catch (Exception e)
            //{
                // TODO: Do something with exception
            //    return false;
            //}
            return true;
        }

        /// <summary>
        /// Upload Csv
        /// </summary>
        /// <param name="dir">dir</param>
        /// <param name="filename">filename</param>
        /// <param name="contents">csv</param>
        public async Task<bool> UploadFileAsync(string dir, string filename, string contents)
        {
            //try
            //{


                //SaveLocalCopy(dir, filename, csv);

                var ftp = CreateRequest(dir + "/" + filename);

                ftp.Method = WebRequestMethods.Ftp.UploadFile;

                ftp.UseBinary = false;

                ftp.KeepAlive = false;

                var bytes = System.Text.UnicodeEncoding.UTF8.GetBytes(contents);

                using (var stream = await ftp.GetRequestStreamAsync())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
            //}
            //catch (Exception e)
            //{
            //    // TODO: Do something with exception
            //    return false;
            //}
            return true;
        }

    }
}
