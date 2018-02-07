using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Net;
using System.Threading;

namespace Eagle.Common.Utilities
{
    public static class FileUtils
    {
        public static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
        public static void CreateFileFromStream(string filePath, Stream stream)
        {
            int bufferSize = (int)stream.Length;
            if (bufferSize == 0) return;
            using (FileStream fileStream = File.Create(filePath, bufferSize))
            {
                byte[] buffer = new byte[bufferSize];
                stream.Read(buffer, 0, Convert.ToInt32(buffer.Length));
                fileStream.Write(buffer, 0, buffer.Length);
            }
        }
        private static bool CreateByteArrayToFile(string filePath, byte[] buffer)
        {
            bool result = false;
            try
            {
                FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                fileStream.Write(buffer, 0, buffer.Length);
                fileStream.Close();
                result = true;
            }
            catch (Exception ex) { ex.ToString(); result = false; }
            return result;
        }
        public static void DownloadFile(string url, string saveAs)
        {
            try
            {
                WebClient webClient = new WebClient();
                // Downloads the resource with the specified URI to a local file.
                webClient.DownloadFile(url, saveAs);
            }
            catch (Exception exception)
            {
                exception.ToString();
            }
        }
        public static string DownloadHtmlPage(string url)
        {
            string pageContent = null;
            try
            {
                // Open a connection
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                // You can also specify additional header values like the user agent or the referer: (Optional)
                httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                httpWebRequest.Referer = "http://www.google.com/";

                // set timeout for 10 seconds (Optional)
                httpWebRequest.Timeout = 10000;

                // Request response:
                WebResponse webResponse = httpWebRequest.GetResponse();

                // Open data stream:
                Stream webStream = webResponse.GetResponseStream();

                // Create reader object:
                StreamReader streamReader = new StreamReader(webStream);

                // Read the entire stream content:
                pageContent = streamReader.ReadToEnd();

                // Cleanup
                streamReader.Close();
                webStream.Close();
                webResponse.Close();
            }
            catch (Exception exception)
            {
                // Error
                exception.ToString();
                return null;
            }

            return pageContent;
        }
        
        public static long GetFileSize(string filePath)
        {
            long fileSize = 0;
            if (File.Exists(filePath))
            {
                FileInfo file = new FileInfo(filePath);
                fileSize = file.Length;
            }

            return fileSize;
        }
        public static byte[] GetBytesFromFile(string filePath)
        {
            FileStream fileStream;

            byte[] fileByte;

            using (fileStream = File.OpenRead(filePath))
            {
                fileByte = new byte[fileStream.Length];
                fileStream.Read(fileByte, 0, Convert.ToInt32(fileStream.Length));
            }

            return fileByte;
        }
        public static byte[] GetByteFromFileUpload(FileUpload fileUpload1)
        {
            //C1
            //FileInfo file_info = new FileInfo(FileUpload1.PostedFile.FileName);
            //byte[] file_content = null;
            //if (file_info.Exists)
            //{
            //    file_content = new byte[file_info.Length];
            //    FileStream imagestream = file_info.OpenRead();
            //    imagestream.Read(file_content, 0, file_content.Length);
            //    imagestream.Close();
            //}

            //C2:
            byte[] fileContent = fileUpload1.FileBytes;
            fileUpload1.PostedFile.InputStream.Read(fileContent, 0, fileContent.Length);
            //C3:
            //byte[] file_content = new byte[FileUpload1.PostedFile.InputStream.Length];
            //FileUpload1.PostedFile.InputStream.Seek(0, SeekOrigin.Begin);
            //FileUpload1.PostedFile.InputStream.Read(file_content, 0, file_content.Length);
            //FileUpload1.PostedFile.InputStream.Write(file_content,0, file_content.Length);   

            return fileContent;
        }
        /// <summary>
        /// Detects the byte order mark of a file and returns
        /// an appropriate encoding for the file.
        /// </summary>
        /// <param name="srcFile"></param>
        /// <returns></returns>
        public static Encoding GetFileEncoding(string srcFile)
        {
            // Use Default of Encoding.Default (Ansi CodePage)
            Encoding enc = Encoding.Default;

            // Detect byte order mark if any - otherwise assume default

            byte[] buffer = new byte[5];
            FileStream file = new FileStream(srcFile, FileMode.Open);
            file.Read(buffer, 0, 5);
            file.Close();

            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
                enc = Encoding.UTF8;
            else if (buffer[0] == 0xfe && buffer[1] == 0xff)
                enc = Encoding.Unicode;
            else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
                enc = Encoding.UTF32;

            else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
                enc = Encoding.UTF7;

            return enc;
        }
        public static void GetFileContent(string filepath)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(filepath);
            StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string n = sr.ReadLine();
            request.GetResponse().Close();
        }
        public static byte[] GetByteFromFilePath(string filePath)
        {
            //Get byte array of file
            byte[] buffer = null;

            if (File.Exists(filePath))
            {
                buffer = File.ReadAllBytes(filePath); //Small file is okay, big file throw exception
            }
            return buffer;

            //Write byte array to file
            //C1: Response.BinaryWrite(byteArray);
            //C2: Response.OutputStream.Write(byteArray,0,byteArray.Length);
            //Set Content Type => Response.ContentType="images/png";
        }

        public static byte[] ReadAndProcessLargeFile(string filePath)
        {
            int maxByteBuffer = 32*1024;
            byte[] buffer = new byte[maxByteBuffer];           // create buffer
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            try
            {
                // Total bytes to read vs Length of the file:
                long totalUnreadDataLength = fileStream.Length;

                // Read the bytes.
                while (totalUnreadDataLength > 0)
                {
                    // Read the data in buffer. Read until total Unread Data Length returns 0 (end of the stream has been reached)
                    var readDataLength = fileStream.Read(buffer, 0, maxByteBuffer);
                    totalUnreadDataLength = totalUnreadDataLength - readDataLength;
                }
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

        public static void TransmitFile(string fullPath, string outFileName)
        {
            System.IO.Stream iStream = null;

            // Buffer to read 10K bytes in chunk:
            byte[] buffer = new Byte[10000];

            // Length of the file:
            int length;

            // Total bytes to read:
            long dataToRead;

            // Identify the file to download including its path.
            string filepath = fullPath;

            // Identify the file name.
            string filename = System.IO.Path.GetFileName(filepath);

            try
            {
                // Open the file.
                iStream = new System.IO.FileStream(filepath, System.IO.FileMode.Open,
                            System.IO.FileAccess.Read, System.IO.FileShare.Read);


                // Total bytes to read:
                dataToRead = iStream.Length;

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + outFileName);
                HttpContext.Current.Response.AddHeader("Content-Length", iStream.Length.ToString());

                // Read the bytes.
                while (dataToRead > 0)
                {
                    // Verify that the client is connected.
                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        // Read the data in buffer.
                        length = iStream.Read(buffer, 0, 10000);

                        // Write the data to the current output stream.
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);

                        // Flush the data to the output.
                        HttpContext.Current.Response.Flush();

                        buffer = new Byte[10000];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        //prevent infinite loop if user disconnects
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
            finally
            {
                if (iStream != null)
                {
                    //Close the file.
                    iStream.Close();
                }
                HttpContext.Current.Response.Close();
            }
        }

        public static void SplitFile(string filePath, int chunkSize, string path)
        {
            const int bufferSize = 20 * 1024;
            byte[] buffer = new byte[bufferSize];

            using (Stream inputStream = File.OpenRead(filePath))
            {
                int index = 0;
                while (inputStream.Position < inputStream.Length)
                {
                    using (Stream outputStream = File.Create(path + "\\" + index))
                    {
                        int remaining = chunkSize, bytesRead;
                        while (remaining > 0 && (bytesRead = inputStream.Read(buffer, 0,
                                Math.Min(remaining, bufferSize))) > 0)
                        {
                            outputStream.Write(buffer, 0, bytesRead);
                            remaining -= bytesRead;
                        }
                    }
                    index++;
                    Thread.Sleep(500);
                }
            }
        }


        /// <summary>
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        public static byte[] ReadStreamFully(Stream stream)
        {
            //use 32K.
            byte[] buffer = new byte[32 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        public static byte[] ReadStreamFully(Stream stream, int initialLength)
        {
            // If we've been passed an unhelpful initial length, just use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }

        public static byte[] GetByteFromHtmlInputFile(HtmlInputFile inputFile)
        {
            HttpPostedFile myFile = inputFile.PostedFile;
            byte[] fileContent = null;
            if (inputFile.PostedFile != null)
            {
                int nFileLen = myFile.ContentLength;
                fileContent = new byte[nFileLen];
                myFile.InputStream.Read(fileContent, 0, nFileLen);
            }
            return fileContent;
        }
        public static string GetFileExtension(string fileName)
        {
            string ext = string.Empty;
            int fileExtPos = fileName.LastIndexOf(".", StringComparison.Ordinal);
            if (fileExtPos >= 0)
                ext = fileName.Substring(fileExtPos, fileName.Length - fileExtPos);

            return ext;
        }
        public static string GetFileName(string fullFileName)
        {
            string ext = string.Empty;
            int fileExtPos = fullFileName.LastIndexOf(".", StringComparison.Ordinal);
            if (fileExtPos >= 0)
                ext = fullFileName.Substring(0, fullFileName.Length - fileExtPos);

            return ext;
        }
        public static string GenerateEncodedFileNameWithDate(string strFileInput, string strKeys)
        {
            //Current Date
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            culture.DateTimeFormat.DateSeparator = string.Empty;
            culture.DateTimeFormat.ShortDatePattern = "yyyyMMdd";
            culture.DateTimeFormat.LongDatePattern = "yyyyMMdd";
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            string currentDateYyyymmddHhmmssMmm = DateTime.UtcNow.ToString("yyyyMMdd-hhmmss") + DateTime.UtcNow.Millisecond.ToString();

            strFileInput = new Regex(@"[\s+\\\/:\*\?\@\&\#\$\^\%\--\+\;\,\“\”\""\''<>|]").Replace(StringUtils.ConvertToUnSign(strFileInput), string.Empty);

            string extension = Path.GetExtension(strFileInput);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(strFileInput);
            string encodeResult;
            if (!string.IsNullOrEmpty(strKeys))
                encodeResult = StringUtils.ConvertTitle2Alias(fileNameWithoutExtension) + "-" + currentDateYyyymmddHhmmssMmm + "-" + strKeys + extension;
            else
                encodeResult = StringUtils.ConvertTitle2Alias(fileNameWithoutExtension) + "-" + currentDateYyyymmddHhmmssMmm + extension;
            return encodeResult;
        }
        public static string GenerateEncodedFileNameWithDate(string strFileInput)
        {
            //Current Date
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            culture.DateTimeFormat.DateSeparator = string.Empty;
            culture.DateTimeFormat.ShortDatePattern = "yyyyMMdd";
            culture.DateTimeFormat.LongDatePattern = "yyyyMMdd";
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            string currentDateYyyymmddHhmmssMmm = DateTime.UtcNow.ToString("yyyyMMdd-hhmmss") + DateTime.UtcNow.Millisecond.ToString();

            strFileInput = new Regex(@"[\s+\\\/:\*\?\@\&\#\$\^\%\--\+\;\,\“\”\""\''<>|]").Replace(StringUtils.ConvertToUnSign(strFileInput), string.Empty);

            string extension = Path.GetExtension(strFileInput);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(strFileInput);
            string encodeResult = StringUtils.ConvertTitle2Alias(fileNameWithoutExtension) + "-" + currentDateYyyymmddHhmmssMmm + extension;
            return encodeResult;
        }
        public static string GenerateEncodedFileNameWithDateGuid(string strFileInput)
        {
            //Current Date
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US")
            {
                DateTimeFormat =
                {
                    DateSeparator = string.Empty,
                    ShortDatePattern = "yyyyMMdd",
                    LongDatePattern = "yyyyMMdd"
                }
            };
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            string currentDateYyyymmddHhmmssMmm = DateTime.UtcNow.ToString("yyyyMMdd-hhmmss") + DateTime.UtcNow.Millisecond.ToString();

            string extension = Path.GetExtension(strFileInput);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(strFileInput);
            string encodeResult = StringUtils.ConvertTitle2Alias(fileNameWithoutExtension) + "-" + currentDateYyyymmddHhmmssMmm + "-" + Guid.NewGuid() + extension;
            return encodeResult;
        }
        public static string GenerateEncodedFileNameWithDateSignature(string strFileInput, string signature = null)
        {
            //Current Date
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US")
            {
                DateTimeFormat =
                {
                    DateSeparator = string.Empty,
                    ShortDatePattern = "yyyyMMdd",
                    LongDatePattern = "yyyyMMdd"
                }
            };
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            string currentDateYyyymmddHhmmss = DateTime.UtcNow.ToString("yyyyMMdd-hhmmss");

            string extension = Path.GetExtension(strFileInput);
            string signal = ((signature != null) ? "-" + signature : "");
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(strFileInput);
            string encodeResult = StringUtils.ConvertTitle2Alias(fileNameWithoutExtension) + "-" + currentDateYyyymmddHhmmss + signal + extension;
            return encodeResult;
        }
        public static bool IsFileUsedbyAnotherProcess(string filePath)
        {
            try
            {
                File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException ex)
            {
                ex.ToString();
                return true;
            }
            return false;
        }
        public static bool IsImage(HttpPostedFileBase postedFile)
        {
            int ImageMinimumBytes = 512;
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                        postedFile.ContentType.ToLower() != "image/jpeg" &&
                        postedFile.ContentType.ToLower() != "image/pjpeg" &&
                        postedFile.ContentType.ToLower() != "image/gif" &&
                        postedFile.ContentType.ToLower() != "image/x-png" &&
                        postedFile.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            var extension = Path.GetExtension(postedFile.FileName);
            if (extension != null && (extension.ToLower() != ".jpg" && extension.ToLower() != ".png" && extension.ToLower() != ".gif" && extension.ToLower() != ".jpeg"))
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.InputStream.CanRead)
                {
                    return false;
                }

                if (postedFile.ContentLength < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[512];
                postedFile.InputStream.Read(buffer, 0, 512);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }


            //----------------------------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //----------------------------------------------------------------
            try
            {
                using (var bitmap = new Bitmap(postedFile.InputStream))
                {
                    if (!bitmap.Size.IsEmpty)
                    {
                        return true;

                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public static bool IsImage(HttpPostedFileBase postedFile, out int? width, out int? height)
        {
            bool result = false;
            width = null;
            height = null;
            try
            {
                using (var bitmap = new Bitmap(postedFile.InputStream))
                {
                    if (!bitmap.Size.IsEmpty)
                    {
                        result = true;
                        width = bitmap.Width;
                        height = bitmap.Height;
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        //public static void WriteResX(string fileName, List<KeyValue> lstResDef)
        //{
        //    try
        //    {
        //        using (ResXResourceWriter writer = new ResXResourceWriter(fileName))
        //        {
        //            foreach (KeyValue obj in lstResDef)
        //                writer.AddResource(obj.Key, obj.Value);
        //            writer.Generate();
        //        }
        //    }
        //    catch { throw new Exception("Error while saving " + fileName); }
        //}
        public static string ReplaceBackSlash(string filepath)
        {
            if (filepath != null)
            {
                filepath = filepath.Replace("\\", "/");
            }
            return filepath;
        }
        public static string UploadFile(HttpPostedFileBase fileUpload)
        {
            string newfileName = string.Empty;
            string physicalUploadPath = HttpContext.Current.Server.MapPath("~/Uploads");
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                if (fileUpload.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(fileUpload.FileName);
                    string extension = Path.GetExtension(fileUpload.FileName);
                    newfileName = $"{Guid.NewGuid()}-{fileName}{extension}";

                    if (Directory.Exists(physicalUploadPath))
                    {
                        string filePath = Path.Combine(physicalUploadPath, newfileName);
                        if (!File.Exists(filePath))
                        {
                            fileUpload.SaveAs(filePath);
                            fileName = newfileName;
                        }
                    }
                }
            }
            return newfileName;
        }
        public static string[] UploadFile(HttpPostedFileBase fileUpload, string virtualUploadPath)
        {
            string[] fileNameArray = new string[2];
            string physicalUploadPath = HttpContext.Current.Server.MapPath(virtualUploadPath);

            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                string extension = Path.GetExtension(fileUpload.FileName);
                string fileName = Path.GetFileNameWithoutExtension(fileUpload.FileName);
                string newFileName = $"{fileName}{Guid.NewGuid()}{extension}";

                if (Directory.Exists(physicalUploadPath))
                {
                    string filePath = Path.Combine(physicalUploadPath, newFileName);
                    if (!File.Exists(filePath))
                    {
                        fileUpload.SaveAs(filePath);
                        fileNameArray[0] = fileName;
                        fileNameArray[1] = newFileName;
                    }
                }
            }
            return fileNameArray;
        }
        public static string UploadFile(HttpPostedFileBase fileUpload, string virtualUploadPath, bool isFolderCreatedByDate)
        {
            string result = string.Empty;
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                string physicalUploadPath = HttpContext.Current.Server.MapPath(virtualUploadPath);
                if (Directory.Exists(physicalUploadPath))
                {
                    string fileName = Path.GetFileName(fileUpload.FileName);
                    string newFileName = GenerateEncodedFileNameWithDateSignature(fileName);

                    if (isFolderCreatedByDate)
                    {
                        string date = DateTime.Today.Day.ToString();
                        string month = DateTime.Today.Month.ToString();
                        string year = DateTime.Today.Year.ToString();

                        if (!Directory.Exists(physicalUploadPath))
                            Directory.CreateDirectory(physicalUploadPath);
                        if (!Directory.Exists(physicalUploadPath + "\\" + year))
                            Directory.CreateDirectory(physicalUploadPath + "\\" + year);
                        if (!Directory.Exists(physicalUploadPath + "\\" + year + "\\" + month))
                            Directory.CreateDirectory(physicalUploadPath + "\\" + year + "\\" + month);
                        if (!Directory.Exists(physicalUploadPath + "\\" + year + "\\" + month + "\\" + date))
                            Directory.CreateDirectory(physicalUploadPath + "\\" + year + "\\" + month + "\\" + date);

                        physicalUploadPath = physicalUploadPath + "\\" + year + "\\" + month + "\\" + date;
                        result = year + "/" + month + "/" + date + "/" + newFileName;
                    }
                    else
                        result = newFileName;

                    string filePath = Path.Combine(physicalUploadPath, HttpContext.Current.Server.HtmlEncode(newFileName));
                    fileUpload.SaveAs(filePath);
                }
            }
            return result;
        }
        public static string[] UploadFileWithThumbnail(HttpPostedFileBase fileUpload, string virtualUploadPath, int? resizedImgWidth = 120, int? resizedImgHeight = 120)
        {
            //begin xu ly file upload=======================================================================================================            
            string[] result = new string[2];
            if (fileUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(fileUpload.FileName);
                string fileExt = Path.GetExtension(fileUpload.FileName).ToLower().Trim();
                string newFileName = GenerateEncodedFileNameWithDateSignature(fileName);
                string mainImageName = HttpContext.Current.Server.HtmlEncode($"{newFileName}{fileExt}");
                string frontImageName = HttpContext.Current.Server.HtmlEncode($"{newFileName}_thumb{fileExt}");


                string physicalUploadPath = HttpContext.Current.Server.MapPath(virtualUploadPath);
                if (Directory.Exists(physicalUploadPath))
                {
                    result[0] = mainImageName;
                    result[1] = frontImageName;

                    string mainImagePath = Path.Combine(physicalUploadPath, mainImageName);
                    string frontImagePath = Path.Combine(physicalUploadPath, frontImageName);

                    // Save main image================================================================================
                    System.Drawing.Image image = System.Drawing.Image.FromStream(fileUpload.InputStream);
                    image.Save(mainImagePath);

                    if (!string.IsNullOrEmpty(mainImagePath) && resizedImgWidth != null && resizedImgHeight != null)
                    {
                        //System.Drawing.Image thumb = image.GetThumbnailImage((int)resizedImgWidth, (int)resizedImgHeight, null, IntPtr.Zero);
                        //thumb.Save(frontImagePath);
                        ResizeImage(mainImagePath, frontImagePath, (int)resizedImgWidth, (int)resizedImgHeight);
                        image.Dispose();
                        //thumb.Dispose();
                    }
                }
            }
            return result;
        }

        public static void ResizeImage(string inputFilePath, string outputFilePath, int resizeWidth, int resizeHeight)
        {
            using (System.Drawing.Image photo = new Bitmap(inputFilePath))
            {
                //double aspectRatio = (double)photo.Width / photo.Height;
                //double boxRatio = resizeWidth / resizeHeight;
                //double scaleFactor = 0;

                //if (photo.Width < resizeWidth && photo.Height < resizeHeight)
                //{
                //    // keep the image the same size since it is already smaller than our max width/height
                //    scaleFactor = 1.0;
                //}
                //else
                //{
                //    if (boxRatio > aspectRatio)
                //        scaleFactor = resizeHeight / photo.Height;
                //    else
                //        scaleFactor = resizeWidth / photo.Width;
                //}

                //int newWidth = (int)(photo.Width * scaleFactor);
                //int newHeight = (int)(photo.Height * scaleFactor);

                using (Bitmap bmp = new Bitmap(resizeWidth, resizeHeight))
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                        g.DrawImage(photo, 0, 0, resizeWidth, resizeHeight);

                        var outputFormat = GetImageFormat(outputFilePath);

                        if (ImageFormat.Png.Equals(outputFormat))
                        {
                            bmp.Save(outputFilePath, outputFormat);
                        }
                        else if (ImageFormat.Jpeg.Equals(outputFormat))
                        {
                            ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
                            EncoderParameters encoderParameters;
                            using (encoderParameters = new EncoderParameters(1))
                            {
                                // use jpeg info[1] and set quality to 90
                                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                                bmp.Save(outputFilePath, info[1], encoderParameters);
                            }
                        }
                    }
                }
            }
        }
        private static ImageFormat GetImageFormat(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            if (string.IsNullOrEmpty(extension))
            {
                throw new ArgumentException($"Unable to determine file extension for filePath: {filePath}");
            }

            switch (extension.ToLower())
            {
                case ".bmp":
                    return ImageFormat.Bmp;
                case ".gif":
                    return ImageFormat.Gif;
                case ".ico":
                    return ImageFormat.Icon;
                case ".jpg":
                    return ImageFormat.Jpeg;
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".png":
                    return ImageFormat.Png;
                case ".tif":
                    return ImageFormat.Tiff;
                case ".tiff":
                    return ImageFormat.Tiff;
                case ".wmf":
                    return ImageFormat.Wmf;
                default:
                    throw new NotImplementedException();
            }
        }
        public static string[] UploadFileWithThumbnail(HttpPostedFileBase fileUpload, string virtualUploadPath, int? resizedImgWidth = 120, int? resizedImgHeight = 120, bool isFolderCreatedByDate = false)
        {
            //begin xu ly file upload=======================================================================================================            
            string[] result = new string[2];
            if (fileUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(fileUpload.FileName);
                string fileExt = Path.GetExtension(fileUpload.FileName).ToLower().Trim();
                string newFileName = GenerateEncodedFileNameWithDateSignature(fileName);
                string mainImageName = HttpContext.Current.Server.HtmlEncode($"{newFileName}{fileExt}");
                string frontImageName = HttpContext.Current.Server.HtmlEncode($"{newFileName}_thumb{fileExt}");


                string physicalUploadPath = HttpContext.Current.Server.MapPath(virtualUploadPath);
                if (Directory.Exists(physicalUploadPath))
                {
                    if (isFolderCreatedByDate)
                    {
                        string date = DateTime.Today.Day.ToString();
                        string month = DateTime.Today.Month.ToString();
                        string year = DateTime.Today.Year.ToString();

                        if (!Directory.Exists(physicalUploadPath))
                            Directory.CreateDirectory(physicalUploadPath);
                        if (!Directory.Exists(physicalUploadPath + "\\" + year))
                            Directory.CreateDirectory(physicalUploadPath + "\\" + year);
                        if (!Directory.Exists(physicalUploadPath + "\\" + year + "\\" + month))
                            Directory.CreateDirectory(physicalUploadPath + "\\" + year + "\\" + month);
                        if (!Directory.Exists(physicalUploadPath + "\\" + year + "\\" + month + "\\" + date))
                            Directory.CreateDirectory(physicalUploadPath + "\\" + year + "\\" + month + "\\" + date);

                        physicalUploadPath = physicalUploadPath + "\\" + year + "\\" + month + "\\" + date;

                        result[0] = (year + "\\" + month + "\\" + date + "\\" + mainImageName).Replace("\\", "/");
                        result[1] = (year + "\\" + month + "\\" + date + "\\" + frontImageName).Replace("\\", "/");
                    }
                    else
                    {
                        result[0] = mainImageName;
                        result[1] = frontImageName;
                    }

                    string mainImagePath = Path.Combine(physicalUploadPath, mainImageName);
                    string frontImagePath = Path.Combine(physicalUploadPath, frontImageName);

                    // Save main image================================================================================
                    System.Drawing.Image image = System.Drawing.Image.FromStream(fileUpload.InputStream);
                    image.Save(mainImagePath);

                    if (!string.IsNullOrEmpty(mainImagePath) && resizedImgWidth != null && resizedImgHeight != null)
                    {
                        //System.Drawing.Image thumb = image.GetThumbnailImage((int)resizedImgWidth, (int)resizedImgHeight, null, IntPtr.Zero);
                        //thumb.Save(frontImagePath);
                        ResizeImage(mainImagePath, frontImagePath, (int)resizedImgWidth, (int)resizedImgHeight);
                        image.Dispose();
                        //thumb.Dispose();
                    }
                }
            }
            return result;
        }
        public static string UploadFileAndRemoveOldFile(HttpPostedFileBase fileUpload, string oldVirtualFilePath)
        {
            string newFileName = string.Empty;
            string physicalUploadPath = HttpContext.Current.Server.MapPath("~/Uploads");
            string physicalOldFilePath = HttpContext.Current.Server.MapPath(oldVirtualFilePath);

            if (fileUpload.ContentLength > 0)
            {
                string extension = Path.GetExtension(fileUpload.FileName);
                newFileName = $"{Guid.NewGuid()}-{extension}";

                if (Directory.Exists(physicalUploadPath))
                {
                    if (oldVirtualFilePath != string.Empty && File.Exists(physicalOldFilePath))
                        File.Delete(physicalOldFilePath);

                    string filePath = Path.Combine(physicalUploadPath, newFileName);
                    if (!File.Exists(filePath))
                    {
                        fileUpload.SaveAs(filePath);
                    }
                }
            }
            return newFileName;
        }
        public static string[] UploadFileAndRemoveOldFile(HttpPostedFileBase fileUpload, string oldVirtualFilePath, string virtualUploadPath)
        {
            string[] fileNames = new string[2];
            string physicalUploadPath = HttpContext.Current.Server.MapPath(virtualUploadPath);
            string physicalOldFilePath = HttpContext.Current.Server.MapPath(oldVirtualFilePath);

            if (fileUpload.ContentLength > 0)
            {
                string extension = Path.GetExtension(fileUpload.FileName);
                string fileName = Path.GetFileNameWithoutExtension(fileUpload.FileName);
                string newFileName = $"{fileName}-{Guid.NewGuid()}{extension}";

                if (Directory.Exists(physicalUploadPath))
                {
                    if (oldVirtualFilePath != string.Empty && File.Exists(physicalOldFilePath))
                        File.Delete(physicalOldFilePath);

                    string filePath = Path.Combine(physicalUploadPath, newFileName);
                    if (!File.Exists(filePath))
                    {
                        fileUpload.SaveAs(filePath);
                        fileNames[0] = fileName;
                        fileNames[1] = newFileName;
                    }
                }
            }
            return fileNames;
        }
        public static string UploadAndResizeImage(HttpPostedFileBase fileUpload, string virtualUploadPath, int? width = null, int? height = null)
        {
            string newFileName = string.Empty;
            string physicalUploadPath = HttpContext.Current.Server.MapPath(virtualUploadPath);

            if (fileUpload.ContentLength > 0)
            {
                string extension = Path.GetExtension(fileUpload.FileName);
                string fileName = Path.GetFileNameWithoutExtension(fileUpload.FileName);
                newFileName = $"{Guid.NewGuid()}-{fileName}-{extension}";

                if (Directory.Exists(physicalUploadPath))
                {
                    string filePath = Path.Combine(physicalUploadPath, newFileName);
                    if (!File.Exists(filePath))
                    {
                        fileUpload.SaveAs(filePath);
                        if (width != null && height != null)
                        {
                            ResizeImage(filePath, (int)width, (int)height);
                        }
                    }
                }
            }
            return newFileName;
        }
        public static string UploadAndResizeImageAndRemoveOldImage(HttpPostedFileBase fileUpload, string oldVirtualFilePath, int? width = null, int? height = null)
        {
            string newFileName = string.Empty;
            string physicalUploadPath = HttpContext.Current.Server.MapPath("~/Uploads");
            string physicalOldFilePath = HttpContext.Current.Server.MapPath(oldVirtualFilePath);

            if (fileUpload.ContentLength > 0)
            {
                string extension = Path.GetExtension(fileUpload.FileName);
                newFileName = $"{Guid.NewGuid()}{extension}";

                if (Directory.Exists(physicalUploadPath))
                {
                    if (oldVirtualFilePath != string.Empty && File.Exists(physicalOldFilePath))
                        File.Delete(physicalOldFilePath);

                    string filePath = Path.Combine(physicalUploadPath, newFileName);
                    if (!File.Exists(filePath))
                    {
                        fileUpload.SaveAs(filePath);
                        if (width != null && height != null)
                        {
                            ResizeImage(filePath, (int)width, (int)height);
                        }
                    }
                }
            }
            return newFileName;
        }
        public static string UploadAndResizeImageAndRemoveOldImage(HttpPostedFileBase fileUpload, string oldVirtualFilePath, string virtualUploadPath, int? width = null, int? height = null)
        {
            string newFileName = string.Empty;
            string physicalUploadPath = HttpContext.Current.Server.MapPath(virtualUploadPath);
            string physicalOldFilePath = HttpContext.Current.Server.MapPath(oldVirtualFilePath);

            if (fileUpload.ContentLength > 0)
            {
                string fileName = Path.GetFileNameWithoutExtension(fileUpload.FileName);
                string extension = Path.GetExtension(fileUpload.FileName);
                newFileName = $"{Guid.NewGuid()}-{fileName}-{extension}";

                if (Directory.Exists(physicalUploadPath))
                {
                    if (oldVirtualFilePath != string.Empty && File.Exists(physicalOldFilePath))
                        File.Delete(physicalOldFilePath);

                    string filePath = Path.Combine(physicalUploadPath, newFileName);
                    if (!File.Exists(filePath))
                    {
                        fileUpload.SaveAs(filePath);
                        if (width != null && height != null)
                        {
                            ResizeImage(filePath, (int)width, (int)height);
                        }
                    }
                }
            }
            return newFileName;
        }
        public static void ResizeImage(string lcFilename, int lnWidth = 150, int lnHeight = 150)
        {
            Bitmap bmpOut = null;

            try
            {
                Bitmap loBmp = new Bitmap(lcFilename);
                ImageFormat loFormat = loBmp.RawFormat;

                decimal lnRatio;
                int lnNewWidth = 0;
                int lnNewHeight = 0;

                if (loBmp.Width < lnWidth && loBmp.Height < lnHeight)
                {
                    loBmp.Dispose();
                    return;
                }
                //return loBMP;

                if (loBmp.Width > loBmp.Height)
                {
                    lnRatio = (decimal)lnWidth / loBmp.Width;
                    lnNewWidth = lnWidth;
                    decimal lnTemp = loBmp.Height * lnRatio;
                    lnNewHeight = (int)lnTemp;
                }
                else
                {
                    lnRatio = (decimal)lnHeight / loBmp.Height;
                    lnNewHeight = lnHeight;
                    decimal lnTemp = loBmp.Width * lnRatio;
                    lnNewWidth = (int)lnTemp;
                }


                bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
                Graphics g = Graphics.FromImage(bmpOut);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
                g.DrawImage(loBmp, 0, 0, lnNewWidth, lnNewHeight);

                loBmp.Dispose();
            }
            catch
            {
                return;
            }
            bmpOut.Save(lcFilename);
            bmpOut.Dispose();
        }

        #region VIDEO FILE ======================================================================================================
        public static string upload_video(HttpPostedFileBase videoUpload, string videoDirPath)
        {

            string result = string.Empty;
            if (videoUpload != null && videoUpload.ContentLength > 0)
            {
                if (!Directory.Exists(videoDirPath))
                    Directory.CreateDirectory(videoDirPath);

                string fileName = Path.GetFileNameWithoutExtension(videoUpload.FileName);
                string fileExt = Path.GetExtension(videoUpload.FileName).ToLower().Trim();
                string fileType = videoUpload.ContentType;

                bool allowedType = false;

                switch (fileType)
                {
                    case ("video/avi"):
                        allowedType = true;
                        break;
                    case ("video/flv"):
                        allowedType = true;
                        fileExt = ".flv";
                        break;
                    case ("video/mp4"):
                        allowedType = true;
                        break;
                    case ("video/mpeg"):
                        allowedType = true;
                        break;
                    case ("image/wav"):
                        allowedType = true;
                        break;
                    case ("application/x-shockwave-flash"):
                        allowedType = true;
                        break;
                    default:
                        allowedType = false;
                        break;
                }

                if (allowedType)
                {
                    string newFileNameExt = StringUtils.GetEncodeString(fileName) + fileExt;
                    string filePath = Path.Combine(videoDirPath, HttpContext.Current.Server.HtmlEncode(newFileNameExt));

                    if (File.Exists(filePath))
                        result = "0";
                    else
                    {
                        videoUpload.SaveAs(filePath);
                        result = newFileNameExt;
                    }
                }
                else
                {
                    result = "-1";
                }
            }
            return result;
        }
        #endregion ==============================================================================================================

        public static byte[] ConvertImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, ImageFormat.Gif);
            return ms.ToArray();
        }
        public static System.Drawing.Image ConvertByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
        public static byte[] ConvertStreamToByteArray(HttpPostedFileBase fileUpload)
        {
            Stream fs = fileUpload.InputStream;
            BinaryReader br = new BinaryReader(fs);
            Byte[] fileContent = br.ReadBytes((Int32)fs.Length);
            return fileContent;
        }
        public static bool DeleteDirectory(string targetDir)
        {
            bool result = false;

            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(targetDir, false);

            return result;
        }
        public static bool DeleteDirectoryFiles(string targetDir, string extTodelete)
        {
            bool result = false;

            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);
            string[] extArrTodelete = extTodelete.Split(',');
            foreach (string file in files)
            {
                foreach (string deleteFile in extArrTodelete)
                {
                    if (deleteFile.Contains(Path.GetExtension(file)))
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                        File.Delete(file);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Creates the folder if needed.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static bool CreateFolderIfNeeded(string virtualPath)
        {
            bool result = true;
            if (!Directory.Exists(virtualPath))
            {
                try
                {
                    Directory.CreateDirectory(virtualPath);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// Opens a stream reader with the appropriate text encoding applied.
        /// </summary>
        /// <param name="srcFile"></param>
        public static StreamReader OpenStreamReaderWithEncoding(string srcFile)
        {
            Encoding enc = GetFileEncoding(srcFile);
            return new StreamReader(srcFile, enc);
        }
        /// <summary>
        /// Returns the full path of a full physical filename
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string JustPath(string path)
        {
            FileInfo fi = new FileInfo(path);
            return fi.DirectoryName + "\\";
        }

        /// <summary>
        /// Returns a fully qualified path from a partial or relative
        /// path.
        /// </summary>
        /// <param name="Path"></param>
        public static string GetFullPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            return Path.GetFullPath(path);
        }

        /// <summary>
        /// Returns a relative path string from a full path based on a base path
        /// provided.
        /// </summary>
        /// <param name="fullPath">The path to convert. Can be either a file or a directory</param>
        /// <param name="basePath">The base path on which relative processing is based. Should be a directory.</param>
        /// <returns>
        /// String of the relative path.
        /// 
        /// Examples of returned values:
        ///  test.txt, ..\test.txt, ..\..\..\test.txt, ., .., subdir\test.txt
        /// </returns>
        public static string GetRelativePath(string fullPath, string basePath)
        {
            // ForceBasePath to a path
            if (!basePath.EndsWith("\\"))
                basePath += "\\";

            Uri baseUri = new Uri(basePath);
            Uri fullUri = new Uri(fullPath);

            Uri relativeUri = baseUri.MakeRelativeUri(fullUri);

            // Uri's use forward slashes so convert back to backward slahes
            return relativeUri.ToString().Replace("/", "\\");


            //// Start by normalizing paths
            //fullPath = fullPath.ToLower();
            //basePath = basePath.ToLower();

            //if ( basePath.EndsWith("\\") ) 
            //    basePath = basePath.Substring(0,basePath.Length-1);
            //if ( fullPath.EndsWith("\\") ) 
            //    fullPath = fullPath.Substring(0,fullPath.Length-1);

            //// First check for full path
            //if ( (fullPath+"\\").IndexOf(basePath + "\\") > -1) 
            //    return  fullPath.Replace(basePath,".");

            //// Now parse backwards
            //string BackDirs = string.Empty;
            //string PartialPath = basePath;
            //int Index = PartialPath.LastIndexOf("\\");
            //while (Index > 0) 
            //{
            //    // Strip path step string to last backslash
            //    PartialPath = PartialPath.Substring(0,Index );

            //    // Add another step backwards to our pass replacement
            //    BackDirs = BackDirs + "..\\" ;

            //    // Check for a matching path
            //    if ( fullPath.IndexOf(PartialPath) > -1 ) 
            //    {
            //        if ( fullPath == PartialPath )
            //            // We're dealing with a full Directory match and need to replace it all
            //            return fullPath.Replace(PartialPath,BackDirs.Substring(0,BackDirs.Length-1) );
            //        else
            //            // We're dealing with a file or a start path
            //            return fullPath.Replace(PartialPath+ (fullPath == PartialPath ?  string.Empty : "\\"),BackDirs);
            //    }
            //    Index = PartialPath.LastIndexOf("\\",PartialPath.Length-1);
            //}

            //return fullPath;
        }

        public static string[] PopulateFileList(string[] srcDirs, string[] searchPatterns, SearchOption searchOption = SearchOption.AllDirectories)
        {
            var fileList = from path in srcDirs
                           from searchPattern in searchPatterns
                           from file in Directory.GetFiles(path, searchPattern, searchOption)
                           select file;
            string[] arrayFileList = fileList.ToArray();
            return arrayFileList;
        }


        public static FileInfo[] PopulateSortedFileArrayList(string physicalSrcDirs, string[] searchPatterns, SearchOption searchOption = SearchOption.AllDirectories)
        {
            //HashSet<string> hashSet_FileTypes = new HashSet<string>(searchPatterns);
            //string[] fileInfo = Directory.GetFiles(physicalSrcDirs, "*.*", searchOption);
            //IEnumerable<string> fileList = searchPatterns.SelectMany(fileExtension => Directory.GetFiles(physicalSrcDirs, fileExtension).ToArray());
            //DirectoryInfo dirInfo = new DirectoryInfo(physicalSrcDirs);
            //System.IO.FileInfo[] fileInfoList = dirInfo.GetFiles("*.*", searchOption);          
            //IEnumerable<string> fileList = fileInfo.Where(f => hashSet_FileTypes.Contains(new System.IO.FileInfo(f).Extension,StringComparer.OrdinalIgnoreCase));            
            //string[] colection = Directory.GetFiles(physicalSrcDirs).Select(x => new System.IO.FileInfo(x).Name).ToArray();

            if (searchPatterns == null)
                throw new ArgumentNullException("No result found with the file extension" + searchPatterns);
            DirectoryInfo dirInfo = new DirectoryInfo(physicalSrcDirs);
            var allowedExtensions = new HashSet<string>(searchPatterns, StringComparer.OrdinalIgnoreCase);
            FileInfo[] fileInfoList = dirInfo.EnumerateFiles().Where(f => allowedExtensions.Contains(f.Extension.ToLower())).ToArray();
            return fileInfoList;
        }

        public static FileInfo[] PopulateSortedFileList(string physicalSrcDirs, string[] searchPatterns, SearchOption searchOption = SearchOption.AllDirectories)
        {
            HashSet<string> hashSetFileTypes = new HashSet<string>(searchPatterns);
            //string[] fileInfo = Directory.GetFiles(physicalSrcDirs, "*.*", SearchOption.AllDirectories);
            //IEnumerable<string> fileList = fileInfo.Where(f => hashSet_FileTypes.Contains(new System.IO.FileInfo(f).Extension,StringComparer.OrdinalIgnoreCase));

            DirectoryInfo dirInfo = new DirectoryInfo(physicalSrcDirs);
            FileInfo[] fileInfoList = dirInfo.GetFiles("*.*", searchOption);
            IEnumerable<FileInfo> fileList = from file in fileInfoList
                                             from ext in hashSetFileTypes
                                             where string.Equals(ext, file.Extension, StringComparison.OrdinalIgnoreCase)
                                             orderby file.LastWriteTime
                                             select file;
            return fileList.ToArray();
        }

        public static FileInfo[] GetFileByExtensions(string srcDir, string[] fileExtension)
        {
            if (fileExtension == null) throw new ArgumentNullException("No found with the file extension");
            var allowedFileExtensions = new HashSet<string>(fileExtension, StringComparer.OrdinalIgnoreCase);
            DirectoryInfo dirInfo = new DirectoryInfo(srcDir);
            IEnumerable<FileInfo> fileList = dirInfo.GetFiles();
            FileInfo[] files = fileList.Where(f => allowedFileExtensions.Contains(f.Extension.ToLower())).ToArray();
            return files;
        }


        //Get File List With Predefined Extension From Directory ================================================================
        public static FileInfo[] GetFileListWithPredefinedExtensionFromDirectory(string targetDirectory, string fileExtension)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(targetDirectory);
            string internalExtension = string.Concat("*.", fileExtension);
            FileInfo[] fileInfo = directoryInfo.GetFiles(internalExtension, SearchOption.AllDirectories);
            return fileInfo;
        }

        //PopulateFileListWithPredefinedExtensionFromDirectory2DDL ============================================================
        public static void PopulateFileListWithPredefinedExtensionFromDirectory2Ddl(string targetDirectory, string fileExtension, DropDownList dropdownlist, string insertText, string insertValue, string selectedValue, bool autopostback)
        {
            FileInfo[] fileInfo = GetFileListWithPredefinedExtensionFromDirectory(targetDirectory, fileExtension);
            dropdownlist.Items.Clear();
            ListItemCollection lstCollection = new ListItemCollection();
            foreach (FileInfo fileInfoTemp in fileInfo)
            {
                ListItem listItem = new ListItem(fileInfoTemp.Name, fileInfoTemp.Name);
                lstCollection.Add(listItem);
            }

            dropdownlist.DataSource = lstCollection;
            dropdownlist.DataTextField = "Text";
            dropdownlist.DataValueField = "Value";
            dropdownlist.DataBind();
            if (!string.IsNullOrEmpty(insertText))
                dropdownlist.Items.Insert(0, new ListItem(insertText, insertValue));
            if (!string.IsNullOrEmpty(selectedValue))
                dropdownlist.SelectedValue = selectedValue;
            else
                dropdownlist.SelectedIndex = 0;
            dropdownlist.AutoPostBack = autopostback;
        }

        public static void Copy(Stream input, string targetFile, int length)
        {
            byte[] buffer = new byte[8192];
            using (Stream output = File.OpenWrite(targetFile))
            {
                int bytesRead = 1;
                while (length > 0 && bytesRead > 0)
                {
                    bytesRead = input.Read(buffer, 0, Math.Min(length, buffer.Length));
                    output.Write(buffer, 0, bytesRead);
                    length -= bytesRead;
                }
            }
        }

        public static void CopyFile(string sourceFile, string targetFile)
        {
            if (File.Exists(targetFile))
            {
                File.Delete(targetFile);
            }
            Stream input = File.OpenRead(sourceFile);
            Copy(input, targetFile, (int)input.Length);
            GC.Collect();
        }

        #region DOWNLOAD FILE ================================================================================
        public static void DownloadFile(string virtualFilePath)
        {
            string filepath = HttpContext.Current.Server.MapPath(virtualFilePath);
            FileStream fStream = null;
            try
            {
                fStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                byte[] byteBuffer = new byte[(int)fStream.Length];
                fStream.Read(byteBuffer, 0, (int)fStream.Length);

                fStream.Close();
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Length", byteBuffer.Length.ToString());
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + virtualFilePath);
                HttpContext.Current.Response.BinaryWrite(byteBuffer);
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                if (fStream != null)
                {
                    ex.ToString();
                    fStream.Close();
                    fStream.Dispose();
                }
            }
        }

        public static void DownloadFileByFileInfo(string virtualFilePath)
        {
            //HttpContext.Current.Request.PhysicalApplicationPath
            string physicalFilePath = HttpContext.Current.Server.MapPath(virtualFilePath);
            FileInfo fileInfo = new FileInfo(physicalFilePath);
            if (fileInfo.Exists)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileInfo.Name);
                HttpContext.Current.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.WriteFile(fileInfo.FullName);
                HttpContext.Current.Response.End();
            }
        }
        #endregion ===========================================================================================

        #region CREATE - UPDATE - DELETE FILE ========================================================================
        public static void CreateFile(string filePath, string strContent)
        {
            try
            {
                StreamWriter write;
                StreamReader s;
                if (File.Exists(filePath) == false)
                {
                    write = new StreamWriter(filePath);
                    write.WriteLine(strContent);
                    write.Close();
                }
                else
                {
                    s = File.OpenText(filePath);
                    string line = null;
                    while ((line = s.ReadLine()) != null)
                    {
                        strContent += line + Environment.NewLine;
                    }
                    s.Close();
                    write = new StreamWriter(filePath);
                    write.WriteLine(strContent);
                    write.Close();
                }
            }
            catch (Exception e) { e.Message.ToString(); }
        }

        public static string ReadFile(string fileName)
        {
            string content = "";
            StreamReader s;
            if (File.Exists(fileName) == false)
            {
                return "";
            }
            else
            {
                s = File.OpenText(fileName);
                string line = null;
                while ((line = s.ReadLine()) != null)
                {
                    content += line + Environment.NewLine;
                }
                s.Close();
                return content;
            }
        }

        /// <summary>
        /// Cập nhật nhật nôi dung của file
        /// </summary>
        public static void UpDateFile(string fileName, string newConTent)
        {
            StreamWriter write;
            if (File.Exists(fileName) == false)
                Console.WriteLine("No Have fileName");
            else
            {
                write = new StreamWriter(fileName);
                write.WriteLine(newConTent);
                write.Close();
            }

        }

        public static string[] GetDirectoryFileInfoFromFilePath(string filePath)
        {
            string[] result = new string[2];
            string filename = string.Empty, direction = string.Empty;

            if (filePath.IndexOf("/") > -1)
            {
                result[0] = filePath.Substring(0, filePath.LastIndexOf("/"));//direction 
                result[1] = filePath.Remove(0, filePath.LastIndexOf("/")).Trim('/');//filename
            }
            else
            {
                result[0] = "";//direction 
                result[1] = filePath;//filename
            }

            return result;
        }

        public static void DeleteFileWithFileNameAndPredefinedPath(string fileName, string dirPath)
        {
            if (fileName != null)
            {
                bool exists = Directory.Exists(dirPath);
                if (exists == true)
                {
                    string filePath = Path.Combine(dirPath, fileName);
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }
            }
        }

        public static void DeleteFileWithPredefinedDatePath(string orginalFilePath, string uploadDirPath)
        {
            var result = GetDirectoryFileInfoFromFilePath(orginalFilePath);
            string dirPath = uploadDirPath + "/" + result[0].Replace("/", "\\");
            string fileName = result[1];
            if (Directory.Exists(uploadDirPath))
            {
                string filePath = Path.Combine(dirPath, fileName);
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        /// <summary>
        /// Deletes files based on a file spec and a given timeout.
        /// This routine is useful for cleaning up temp files in 
        /// Web applications.
        /// </summary>
        /// <param name="filespec">A filespec that includes path and/or wildcards to select files</param>
        /// <param name="seconds">The timeout - if files are older than this timeout they are deleted</param>
        public static void DeleteTimedoutFiles(string filespec, int seconds)
        {
            string path = Path.GetDirectoryName(filespec);
            string spec = Path.GetFileName(filespec);
            string[] files = Directory.GetFiles(path, spec);

            foreach (string file in files)
            {
                try
                {
                    if (File.GetLastWriteTimeUtc(file) < DateTime.UtcNow.AddSeconds(seconds * -1))
                        File.Delete(file);
                }
                catch { }  // ignore locked files
            }
        }
        #endregion DELETE FILE ====================================================================================================

        public static List<FileInfo> GetFileListOfFixedPath(string physicalPath, string[] extensions, string searchOption)
        {
            List<FileInfo> list = new List<FileInfo>();
            foreach (string ext in extensions)
            {
                if (searchOption == "AllDirectories")
                    list.AddRange(new DirectoryInfo(physicalPath).GetFiles("*" + ext, SearchOption.AllDirectories).Where(p =>
                          p.Extension.Equals(ext, StringComparison.CurrentCultureIgnoreCase))
                          .ToArray());
                else
                    list.AddRange(new DirectoryInfo(physicalPath).GetFiles("*" + ext, SearchOption.TopDirectoryOnly).Where(p =>
                          p.Extension.Equals(ext, StringComparison.CurrentCultureIgnoreCase))
                          .ToArray());
            }
            return list;
        }

        public static int RemoveFilesByExtensionsWithFixedPath(string physicalPath, string strFileExtensions, string searchOption, string strAllotedTime)
        {
            DateTime currentDate = DateTime.UtcNow;
            int allotedTime = 0;
            int numFile = 0;
            if (!string.IsNullOrEmpty(strAllotedTime))
                allotedTime = Convert.ToInt32(strAllotedTime);

            //FileInfo[] fileInfo = GetFileListWithPredefinedExtensionFromDirectory(physical_dir_path, file_extension);
            string[] fileExtensions = strFileExtensions.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            List<FileInfo> lst = GetFileListOfFixedPath(physicalPath, fileExtensions, searchOption);
            if (lst.Count > 0)
            {
                foreach (FileInfo fi in lst)
                {
                    TimeSpan timeSpan = currentDate.Subtract(fi.CreationTime);
                    if (timeSpan.Days >= allotedTime)
                    {
                        string filePath = Path.Combine(physicalPath, fi.Name);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            numFile++;
                        }
                    }
                }
            }
            return numFile;
        }
        //================================================================================
        //Save a Stream to a File
        // readStream is the stream you need to read
        // writeStream is the stream you want to write to
        private static void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            int length = 256;
            Byte[] buffer = new Byte[length];
            int bytesRead = readStream.Read(buffer, 0, length);
            // write the required bytes
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, length);
            }
            readStream.Close();
            writeStream.Close();

            //string saveTo = "some path to save";
            //// create a write stream
            //FileStream writeStream = new FileStream(saveTo, FileMode.Create, FileAccess.Write);
            //// write to the stream
            //ReadWriteStream(readStream, writeStream);
        }
        public static byte[] ReadByteArryFromFile(string destPath)
        {
            byte[] buff = null;
            FileStream fs = new FileStream(destPath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(destPath).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
        }
        /// <summary>
        /// Copies the content of the one stream to another.
        /// Streams must be open and stay open.
        /// </summary>
        public static void CopyStream(Stream source, Stream dest, int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            int read;
            while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                dest.Write(buffer, 0, read);
            }
        }

        /// <summary>
        /// Copies the content of one stream to another by appending to the target stream
        /// Streams must be open when passed in.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <param name="bufferSize"></param>
        /// <param name="append"></param>
        public static void CopyStream(Stream source, Stream dest, int bufferSize, bool append)
        {
            if (append)
                dest.Seek(0, SeekOrigin.End);

            CopyStream(source, dest, bufferSize);
            return;
        }

        #region MIME TYPE REGISTRY =======================================

        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static UInt32 FindMimeFromData(
            UInt32 pBc,
            [MarshalAs(UnmanagedType.LPStr)] String pwzUrl,
            [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
            UInt32 cbSize,
            [MarshalAs(UnmanagedType.LPStr)] String pwzMimeProposed,
            UInt32 dwMimeFlags,
            out UInt32 ppwzMimeOut,
            UInt32 dwReserverd
        );

        private static string GetMimeFromRegistry(string filename)
        {
            string mime = "application/octetstream";
            string ext = Path.GetExtension(filename).ToLower();
            RegistryKey rk = Registry.ClassesRoot.OpenSubKey(ext);
            if (rk != null && rk.GetValue("Content Type") != null)
                mime = rk.GetValue("Content Type").ToString();
            return mime;
        }
        public static string GetMimeTypeFromFileAndRegistry(string filename)
        {
            if (!File.Exists(filename))
                return GetMimeFromRegistry(filename);

            byte[] buffer = new byte[256];
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                if (fs.Length >= 256)
                    fs.Read(buffer, 0, 256);
                else
                    fs.Read(buffer, 0, (int)fs.Length);
            }

            try
            {
                UInt32 mimetype;
                FindMimeFromData(0, null, buffer, 256, null, 0, out mimetype, 0);
                IntPtr mimeTypePtr = new IntPtr(mimetype);
                string mime = Marshal.PtrToStringUni(mimeTypePtr);
                Marshal.FreeCoTaskMem(mimeTypePtr);
                if (string.IsNullOrWhiteSpace(mime) || mime == "text/plain" || mime == "application/octet-stream")
                {
                    return GetMimeFromRegistry(filename);
                }
                return mime;
            }
            catch
            {
                return GetMimeFromRegistry(filename);
            }
        }

        public static string GetMimeType(FileInfo fileInfo)
        {
            string mimeType = "application/unknown";

            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(fileInfo.Extension.ToLower());

            if (regKey != null)
            {
                object contentType = regKey.GetValue("Content Type");

                if (contentType != null)
                    mimeType = contentType.ToString();
            }

            return mimeType;
        }

        #endregion ====================================================================

        public static string ToAbsolutePath(string virtualPath)
        {
            return VirtualPathUtility.ToAbsolute(virtualPath);
        }

        public static string CombinePaths(string basePath, string relativePath)
        {
            return VirtualPathUtility.Combine(VirtualPathUtility.AppendTrailingSlash(basePath), relativePath);
        }

        /// <summary>
        /// Count the number of lines in the file specified.
        /// </summary>
        /// <param name="path">The filename to count lines.</param>
        /// <returns>The number of lines in the file.</returns>
        public static long CountLinesInFile(string path)
        {
            long count = 0;
            using (StreamReader reader = new StreamReader(path))
            {
                while ((reader.ReadLine()) != null)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
