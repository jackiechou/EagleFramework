using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Eagle.Common.Utilities
{
    public class ImageManager
    {
        public static Stream ResizeAndPadImage(Stream fileStream)
        {
            return ResizeAndPadImages(fileStream);
        }

        public static Stream ResizeAndPadImages(Stream fileStream)
        {
            //max size in Kb
            var image = Image.FromStream(fileStream);
            FixPhotoOrientation(ref image);

            var imageStream = new MemoryStream();
            var imageHeight = Convert.ToInt32(ConfigurationManager.AppSettings["DesktopImageHeight"]);
            var imageWidth = Convert.ToInt32(ConfigurationManager.AppSettings["DesktopImageWidth"]);

            if (image.Width > imageWidth && image.Height > imageHeight)
            {

                var ratioX = (double)imageWidth / image.Width;
                var ratioY = (double)imageHeight / image.Height;
                var ratio = Math.Min(ratioX, ratioY);
                var newWidth = (int)(image.Width * ratio);
                var newHeight = (int)(image.Height * ratio);

                using (var bmPhoto = new Bitmap(newWidth, newHeight, image.PixelFormat))
                {
                    bmPhoto.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                    using (var grPhoto = Graphics.FromImage(bmPhoto))
                    {
                        grPhoto.Clear(Color.White);
                        grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        grPhoto.DrawImage(image,
                            new Rectangle(0, 0, newWidth, newHeight),
                            new Rectangle(0, 0, image.Width, image.Height),
                            GraphicsUnit.Pixel);
                    }

                    ImageCodecInfo encoder;
                    EncoderParameters sherpaEncoderParameters;
                    EncoderSetup(ref image, out encoder, out sherpaEncoderParameters);


                    bmPhoto.Save(imageStream, encoder, sherpaEncoderParameters);
                }
                imageStream.Seek(0, SeekOrigin.Begin);


                image.Dispose();
                return imageStream;
            }

            image.Dispose();
            return fileStream;
        }

        private static void EncoderSetup(ref Image image, out ImageCodecInfo encoder,
            out EncoderParameters sherpaEncoderParameters)
        {
            // assume image is needed encoder is for image format of jpeg
            encoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder =
             System.Drawing.Imaging.Encoder.Quality;
            sherpaEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder,
                Convert.ToInt64(ConfigurationManager.AppSettings["CompressionRatioJPG"]));

            sherpaEncoderParameters.Param[0] = myEncoderParameter;


            // If png change encoder. and compress more
            if (ImageFormat.Png.Equals(image.RawFormat))
            {
                encoder = GetEncoder(ImageFormat.Png);

                myEncoderParameter = new EncoderParameter(myEncoder,
                    Convert.ToInt64(ConfigurationManager.AppSettings["CompressionRatioPNG"]));

                sherpaEncoderParameters.Param[0] = myEncoderParameter;
            }
        }

        private static void FixPhotoOrientation(ref Image image)
        {
            if (Array.IndexOf(image.PropertyIdList, 274) > -1)
            {

                var orientation = (int)image.GetPropertyItem(274).Value[0];
                switch (orientation)
                {
                    case 1:
                        // No rotation required.
                        break;
                    case 2:
                        image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case 3:
                        image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 4:
                        image.RotateFlip(RotateFlipType.Rotate180FlipX);
                        break;
                    case 5:
                        image.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case 6:
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 7:
                        image.RotateFlip(RotateFlipType.Rotate270FlipX);
                        break;
                    case 8:
                        image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }
                // This EXIF data is now invalid and should be removed.
                image.RemovePropertyItem(274);
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

    }
}
