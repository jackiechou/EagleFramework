using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Eagle.WebApi.MediaTypeFormatters
{
    /// <summary>
    /// 
    /// </summary>
    public class BinaryMediaTypeFormatter : MediaTypeFormatter
    {
        private static readonly Type _supportedType = typeof(byte[]);
      
        public BinaryMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/jpeg"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/bmp"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/gif"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/pjpeg"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/tiff"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/x-rgb"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/x-xbitmap"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/x-xpixmap"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/x-xwindowdump"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/cis-cod"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/ief"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/svg+xml"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/x-cmu-raster"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/x-cmx"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/x-icon"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/x-portable-anymap"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/x-portable-bitmap"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/x-portable-graymap"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/x-portable-pixmap"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/png"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/vnd.djvu"));
        }

        public override bool CanReadType(Type type)
        {
            return type == _supportedType;
        }

        public override bool CanWriteType(Type type)
        {
            return type == _supportedType;
        }


        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, 
            System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)

        {
            var taskSource = new TaskCompletionSource<object>();
            try
            {
                var ms = new MemoryStream();
                readStream.CopyTo(ms);
                taskSource.SetResult(ms.ToArray());
            }
            catch (Exception e)
            {
                taskSource.SetException(e);
            }
            return taskSource.Task;
        }

        public override Task WriteToStreamAsync(Type type, object value, 
            Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            var taskSource = new TaskCompletionSource<object>();
            try
            {
                if (value == null)
                    value = new byte[0];
                var ms = new MemoryStream((byte[])value);
                ms.CopyTo(writeStream);
                taskSource.SetResult(null);
            }
            catch (Exception e)
            {
                taskSource.SetException(e);
            }
            return taskSource.Task;
        }
    }
}