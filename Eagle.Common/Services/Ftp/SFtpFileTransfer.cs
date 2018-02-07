using Renci.SshNet;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Eagle.Common.Services.Ftp
{
    public class SFtpFileTransfer
    {
        private string _hostname;
        private string _password;
        private string _username;
        private bool _enableSsl;
        private string _sftpKey;
        private SftpClient _sftp;
        bool _connectViaKey = false;

        public SFtpFileTransfer(string host, string username, string password, bool enableSsl = false)
        {
            _hostname = host;
            _username = username;
            _password = password;
            _enableSsl = enableSsl;
            _connectViaKey = false;
        }
        public SFtpFileTransfer(string host, string username, string ppkKey = "")
        {
            _hostname = host;
            _username = username;
            _sftpKey = ppkKey;
            _connectViaKey = true;
        }

        /// <summary>
        /// Create Directory
        /// </summary>
        /// <param name="dir">dir</param>
        /// <returns></returns>
        public async Task<string> CreateDirectory(string dir)
        {
            //try
            {
                await Task.Run(() =>
                {
                    ConnectAndPerformRequest(() =>
                    {
                        if (!_sftp.Exists(dir))
                            _sftp.CreateDirectory(dir);
                    });
                }
                );
            }
            //catch (Exception e)
            //{
            //    // TODO: Do something with exception
            //    return string.Empty;
            //}
            return dir;
        }
        /// <summary>
        /// Checks to see if the file already exists on the server
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<bool> FileExistsAsync(string dir, string filename)
        {
            //try
            {
                await Task.Run(() =>
                    {
                        ConnectAndPerformRequest(() =>
                        {
                            var path = dir + "/" + filename;
                            _sftp.Exists(path);
                        });
                    }
                );
            }
            //catch (Exception e)
            //{
            //    // TODO: Do something with exception
            //    return false;
            //}
            return false;
        }
        /// <summary>
        /// Checks to see if the file already exists on the server
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool FileExists(string dir, string filename)
        {
            //try
            {
                ConnectAndPerformRequest(() =>
                {
                    var path = dir + "/" + filename;
                    _sftp.Exists(path);
                });
            }
            //catch (Exception e)
            //{
            //    // TODO: Do something with exception
            //    return false;
            //}
            return false;
        }


        private Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        /// <summary>
        /// Upload File
        /// </summary>
        /// <param name="dir">dir</param>
        /// <param name="filename">filename</param>
        /// <param name="contents">file contents</param>
        public bool UploadFile(string dir, string filename, string contents)
        {
            //try
            {
                ConnectAndPerformRequest(() =>
                    {
                        var path = dir + "/" + filename;
                        _sftp.UploadFile(GenerateStreamFromString(contents), path);
                    });
            }
            //catch (Exception e)
            //{
            //    // TODO: Do something with exception
            //    return false;
            //}
            return true;
        }

        /// <summary>
        /// Upload File
        /// </summary>
        /// <param name="dir">dir</param>
        /// <param name="filename">filename</param>
        /// <param name="contents">file contents</param>
        public async Task<bool> UploadFileAsync(string dir, string filename, string contents)
        {
            //try
            {
                var success = false;
                await Task.Run(() => success = UploadFile(dir, filename, contents));
                return success;

            }
            //catch (Exception e)
            //{
            //    // TODO: Do something with exception
            //    return false;
            //}
        }

        private void ConnectAndPerformRequest(Action ftpAction)
        {
            if (_connectViaKey)
            {
                using (_sftp = new SftpClient(_hostname, _username, new PrivateKeyFile(GenerateStreamFromString(_sftpKey))))
                {
                    _sftp.Connect();
                    ftpAction();
                }
            }
            else
            {
                using (_sftp = new SftpClient(_hostname, _username, _password))
                {
                    _sftp.Connect();
                    ftpAction();
                }
            }
        }

    }
}
