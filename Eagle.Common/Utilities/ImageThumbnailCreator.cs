using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Eagle.Common.Utilities
{
    public class ImageThumbnailCreator
    {
        private static readonly IDictionary<string, ImageFormat> ImageFormats = new Dictionary<string, ImageFormat>{
            {"image/png", ImageFormat.Png},
            {"image/gif", ImageFormat.Gif},
            {"image/jpeg", ImageFormat.Jpeg}
        };

        private readonly ImageUtils _resizer;

        public ImageThumbnailCreator()
        {
            _resizer = new ImageUtils();
        }

        public byte[] Create(Stream source, ImageSize desiredSize, string contentType)
        {
            try
            {
                using (var image = Image.FromStream(source))
                {
                    var originalSize = new ImageSize
                    {
                        Height = image.Height,
                        Width = image.Width
                    };

                    var size = _resizer.Resize(originalSize, desiredSize);

                    using (var thumbnail = new Bitmap(size.Width, size.Height))
                    {
                        ScaleImage(image, thumbnail);

                        using (var memoryStream = new MemoryStream())
                        {
                            thumbnail.Save(memoryStream, ImageFormats[contentType]);

                            return memoryStream.ToArray();
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }
        public byte[] CreateFill(Stream source, ImageSize desiredSize, string contentType)
        {
            using (var image = Image.FromStream(source))
            {
                using (var memoryStream = new MemoryStream())
                {
                    FixedSize(image, desiredSize.Width, desiredSize.Height, true).Save(memoryStream, ImageFormats[contentType]);
                    return memoryStream.ToArray();
                }
            }
        }

        private Image FixedSize(Image imgPhoto, int width, int height, bool needToFill)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)width / (float)sourceWidth);
            nPercentH = ((float)height / (float)sourceHeight);
            if (!needToFill)
            {
                if (nPercentH < nPercentW)
                {
                    nPercent = nPercentH;
                }
                else
                {
                    nPercent = nPercentW;
                }
            }
            else
            {
                if (nPercentH > nPercentW)
                {
                    nPercent = nPercentH;
                    destX = (int)Math.Round((width -
                        (sourceWidth * nPercent)) / 2);
                }
                else
                {
                    nPercent = nPercentW;
                    destY = (int)Math.Round((height -
                        (sourceHeight * nPercent)) / 2);
                }
            }

            if (nPercent > 1)
                nPercent = 1;

            int destWidth = (int)Math.Round(sourceWidth * nPercent);
            int destHeight = (int)Math.Round(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(
                destWidth <= width ? destWidth : width,
                destHeight < height ? destHeight : height,
                              PixelFormat.Format32bppRgb);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.CompositingQuality = CompositingQuality.HighQuality;
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
        private void ScaleImage(Image source, Image destination)
        {
            using (var graphics = Graphics.FromImage(destination))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                graphics.DrawImage(source, 0, 0, destination.Width, destination.Height);
            }
        }
    }
}
