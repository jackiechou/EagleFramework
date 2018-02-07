using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Eagle.Common.Utilities
{
    public class ImageUtils
    {
        private static readonly IDictionary<string, ImageFormat> ImageFormats = new Dictionary<string, ImageFormat>{
            {"image/png", ImageFormat.Png},
            {"image/gif", ImageFormat.Gif},
            {"image/jpeg", ImageFormat.Jpeg}
        };

        /// <summary>
        /// Gets or sets from path.
        /// </summary>
        /// <value>From path.</value>
        public string FromPath { get; private set; }
        /// <summary>
        /// Gets or sets to path.
        /// </summary>
        /// <value>To path.</value>
        public string ToPath { get; private set; }
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets the height. 0 indicates height proportional to width.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; set; }
        private int _compression;
        /// <summary>
        /// Gets or sets the compression. 0 corresponds to the greatest compression, 100 corresponds to the least compression.
        /// </summary>
        /// <value>The compression.</value>
        public int Compression
        {
            get { return _compression; }
            set
            {
                if (value > 100) throw new ArgumentOutOfRangeException("value", "value must be 100 or less");
                if (value < 0) throw new ArgumentOutOfRangeException("value", "value must be 0 or more");

                _compression = value;
            }
        }
        
        public ImageUtils() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageResizer"/> class.
        /// </summary>
        /// <param name="fromPath">From path.</param>
        /// <param name="toPath">To path.</param>
        public ImageUtils(string fromPath, string toPath)
        {
            if (string.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (string.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");
            if (!File.Exists(fromPath))
                throw new FileNotFoundException(fromPath + " does not exist");
            var toDirectory = Path.GetDirectoryName(toPath);
            if (!Directory.Exists(toDirectory))
                throw new DirectoryNotFoundException(toDirectory + " does not exist");
            FromPath = fromPath;
            ToPath = toPath;
            Width = 200;
            Compression = 50; //50%
        }

        /// <summary>
        /// Resizes and saves the image.
        /// </summary>
        public void Resize()
        {
            using (Image original = GetBitmapFromFile(FromPath))
            {
                using (Image thumb = CreateThumbnail(original))
                {
                    //jpeg is b96b3cae-...
                    if (original.RawFormat.Guid == ImageFormat.Jpeg.Guid)
                    {
                        SaveAsJpeg(thumb);
                    }
                    else
                    {
                        thumb.Save(ToPath, original.RawFormat);
                    }
                }
            }
        }

        public ImageSize Resize(ImageSize originalSize, ImageSize targetSize)
        {
            var aspectRatio = (float)originalSize.Width / (float)originalSize.Height;
            var width = targetSize.Width;
            var height = targetSize.Height;

            if (originalSize.Width > targetSize.Width || originalSize.Height > targetSize.Height)
            {
                if (aspectRatio > 1)
                {
                    height = (int)(targetSize.Height / aspectRatio);
                }
                else
                {
                    width = (int)(targetSize.Width * aspectRatio);
                }
            }
            else
            {
                width = originalSize.Width;
                height = originalSize.Height;
            }

            return new ImageSize
            {
                Width = Math.Max(width, 1),
                Height = Math.Max(height, 1)
            };
        }

        /// <summary>
        /// Creates the thumbnail.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <returns></returns>
        private Image CreateThumbnail(Image original)
        {
            //http://west-wind.com/weblog/posts/283.aspx
            Bitmap bmpOut;
            try
            {
                decimal ratio;
                int newWidth;
                int newHeight;

                //if the height isn't defined, get one with the right aspect ratio
                if (Height == 0)
                {
                    ratio = (decimal)Width / original.Width;
                    Height = (int)(original.Height * ratio);
                }

                //If the image is smaller than a thumbnail just return it
                if (original.Width < Width && original.Height < Height)
                    return new Bitmap(original); //return a copy as we dispose of the original

                if (original.Width > original.Height)
                {
                    //wide image
                    ratio = (decimal)Width / original.Width;
                    newWidth = Width;
                    newHeight = (int)(original.Height * ratio);
                }
                else
                {
                    //tall image
                    ratio = (decimal)Height / original.Height;
                    newHeight = Height;
                    newWidth = (int)(original.Width * ratio);
                }

                // This code creates cleaner (though bigger) thumbnails and properly
                // and handles GIF files better by generating a white background for
                // transparent images (as opposed to black)
                bmpOut = new Bitmap(newWidth, newHeight);

                using (Graphics g = Graphics.FromImage(bmpOut))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
                    g.DrawImage(original, 0, 0, newWidth, newHeight);
                }
            }
            catch
            {
                return null;
            }
            return bmpOut;
        }

        private static Bitmap GetBitmapFromFile(string filename)
        {
            return new Bitmap(filename);
        }

        private void SaveAsJpeg(Image img)
        {
            //http://msdn.microsoft.com/en-us/library/bb882583.aspx
            //Quality is 0 (most compression) to 100 (least compression)
            long compression = Compression;

            var jpgEncoder = ImageCodecInfo.GetImageDecoders()
                .FirstOrDefault(x => x.FormatID == ImageFormat.Jpeg.Guid);

            using (var myEncoderParameters = new EncoderParameters(1))
            {
                using (var myEncoderParameter =
                    new EncoderParameter(Encoder.Quality, compression))
                {
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    img.Save(ToPath, jpgEncoder, myEncoderParameters);
                }
            }
        }

        /// <summary>
		/// Creates a resized bitmap from an existing image on disk. Resizes the image by 
		/// creating an aspect ratio safe image. Image is sized to the larger size of width
		/// height and then smaller size is adjusted by aspect ratio.
		/// 
		/// Image is returned as Bitmap - call Dispose() on the returned Bitmap object
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns>Bitmap or null</returns>
		public static Bitmap ResizeImage(string filename, int width, int height)
        {
            Bitmap bmpOut = null;

            try
            {
                Bitmap bmp = new Bitmap(filename);
                ImageFormat format = bmp.RawFormat;

                decimal ratio;
                int newWidth = 0;
                int newHeight = 0;

                //*** If the image is smaller than a thumbnail just return it
                if (bmp.Width < width && bmp.Height < height)
                    return bmp;

                if (bmp.Width > bmp.Height)
                {
                    ratio = (decimal)width / bmp.Width;
                    newWidth = width;
                    decimal lnTemp = bmp.Height * ratio;
                    newHeight = (int)lnTemp;
                }
                else
                {
                    ratio = (decimal)height / bmp.Height;
                    newHeight = height;
                    decimal lnTemp = bmp.Width * ratio;
                    newWidth = (int)lnTemp;
                }

                bmpOut = new Bitmap(newWidth, newHeight);
                Graphics g = Graphics.FromImage(bmpOut);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
                g.DrawImage(bmp, 0, 0, newWidth, newHeight);

                //System.Drawing.Image imgOut = loBMP.GetThumbnailImage(lnNewWidth,lnNewHeight,null,IntPtr.Zero);
                bmp.Dispose();
                bmpOut.Dispose();
            }
            catch
            {
                return null;
            }

            return bmpOut;
        }

        /// <summary>
        /// Resizes an image from byte array and returns a Bitmap.
        /// Make sure you Dispose() the bitmap after you're done 
        /// with it!
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap ResizeImage(byte[] data, int width, int height)
        {
            Bitmap bmpOut = null;

            try
            {
                Bitmap bmp = new Bitmap(new MemoryStream(data));
                ImageFormat format = bmp.RawFormat;

                decimal ratio;
                int newWidth = 0;
                int newHeight = 0;

                //*** If the image is smaller than a thumbnail just return it
                if (bmp.Width < width && bmp.Height < height)
                    return bmp;

                if (bmp.Width > bmp.Height)
                {
                    ratio = (decimal)width / bmp.Width;
                    newWidth = width;
                    decimal lnTemp = bmp.Height * ratio;
                    newHeight = (int)lnTemp;
                }
                else
                {
                    ratio = (decimal)height / bmp.Height;
                    newHeight = height;
                    decimal lnTemp = bmp.Width * ratio;
                    newWidth = (int)lnTemp;
                }

                bmpOut = new Bitmap(newWidth, newHeight);
                Graphics g = Graphics.FromImage(bmpOut);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
                g.DrawImage(bmp, 0, 0, newWidth, newHeight);

                //System.Drawing.Image imgOut = loBMP.GetThumbnailImage(lnNewWidth,lnNewHeight,null,IntPtr.Zero);
                bmp.Dispose();
                //bmpOut.Dispose();
            }
            catch
            {
                return null;
            }

            return bmpOut;
        }

        public static bool ResizeImage(string filename, string outputFilename, int width, int height)
        {
            Bitmap bmpOut = null;

            try
            {
                Bitmap bmp = new Bitmap(filename);
                ImageFormat format = bmp.RawFormat;

                decimal ratio;
                int newWidth = 0;
                int newHeight = 0;

                //*** If the image is smaller than a thumbnail just return it
                if (bmp.Width < width && bmp.Height < height)
                {
                    if (outputFilename != filename)
                        bmp.Save(outputFilename);
                    bmp.Dispose();
                    return true;
                }

                if (bmp.Width > bmp.Height)
                {
                    ratio = (decimal)width / bmp.Width;
                    newWidth = width;
                    decimal temp = bmp.Height * ratio;
                    newHeight = (int)temp;
                }
                else
                {
                    ratio = (decimal)height / bmp.Height;
                    newHeight = height;
                    decimal lnTemp = bmp.Width * ratio;
                    newWidth = (int)lnTemp;
                }

                bmpOut = new Bitmap(newWidth, newHeight);
                Graphics g = Graphics.FromImage(bmpOut);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
                g.DrawImage(bmp, 0, 0, newWidth, newHeight);

                //System.Drawing.Image imgOut = loBMP.GetThumbnailImage(lnNewWidth,lnNewHeight,null,IntPtr.Zero);
                bmp.Dispose();

                bmpOut.Save(outputFilename, format);
                bmpOut.Dispose();
            }
            catch (Exception ex)
            {
                var msg = ex.GetBaseException();
                return false;
            }

            return true;
        }

        public static string CreateWatermark(string virtualBackgroundImagePath, string virtualWatermarkPath, string finalImagePath)
        {           
            Image image = Image.FromFile(HttpContext.Current.Server.MapPath(virtualBackgroundImagePath));//This is the background image
            Image logo = Image.FromFile(HttpContext.Current.Server.MapPath(virtualWatermarkPath)); //This is your watermark
            Graphics g = Graphics.FromImage(image); //Create graphics object of the background image //So that you can draw your logo on it
            Bitmap transparentLogo = new Bitmap(logo.Width, logo.Height); //Create a blank bitmap object //to which we //draw our transparent logo
            Graphics graphics = Graphics.FromImage(transparentLogo);//Create a graphics object so that //we can draw //on the blank bitmap image object
            ColorMatrix colorMatrix = new ColorMatrix(); //An image is represenred as a 5X4 matrix(i.e 4 //columns and 5 //rows)
            colorMatrix.Matrix33 = 0.25F;//the 3rd element of the 4th row represents the transparency
            ImageAttributes imgAttributes = new ImageAttributes();//an ImageAttributes object is used to set all //the alpha //values.This is done by initializing a color matrix and setting the alpha scaling value in the matrix.The address of //the color matrix is passed to the SetColorMatrix method of the //ImageAttributes object, and the //ImageAttributes object is passed to the DrawImage method of the Graphics object.
            imgAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap); graphics.DrawImage(logo, new Rectangle(0, 0, transparentLogo.Width, transparentLogo.Height), 0, 0, transparentLogo.Width, transparentLogo.Height, GraphicsUnit.Pixel, imgAttributes);
            graphics.Dispose();
            g.DrawImage(transparentLogo, image.Width / 2, 30);

            ImageFormat imageFormat = null;
            string physicalFinalFilePath = HttpContext.Current.Server.MapPath(finalImagePath);
            string imageName = Path.GetFileName(physicalFinalFilePath);
            string finalImageExtension = Path.GetExtension(physicalFinalFilePath);
            if (finalImageExtension != null)
                switch (finalImageExtension.ToLower())
                {
                    case ".jpg":
                        imageFormat = ImageFormat.Jpeg;
                        break;
                    case ".jpeg":
                        imageFormat = ImageFormat.Jpeg;
                        break;
                    case ".png":
                        imageFormat = ImageFormat.Png;
                        break;
                    case ".gif":
                        imageFormat = ImageFormat.Gif;
                        break;
                    case ".bmp":
                        imageFormat = ImageFormat.Bmp;
                        break;
                    default:
                        imageFormat = ImageFormat.Jpeg;
                        break;
                }
            if(imageFormat!=null)
                image.Save(physicalFinalFilePath, imageFormat);
            return imageName;
        }
        public static Color[] GetUniqueRandomColor(int count)
        {
            Color[] colors = new Color[count];
            Dictionary<Color, bool> hs = new Dictionary<Color, bool>();

            Random randomColor = new Random();

            for (int i = 0; i < count; i++)
            {
                Color color;
                while (hs.ContainsKey(color = Color.FromArgb(randomColor.Next(70, 200), randomColor.Next(100, 225), randomColor.Next(100, 230))))
                {
                    hs.Add(color, true);
                }
                colors[i] = color;
            }

            return colors;
        }
        public static string[] GetUniqueRandomShapeStrings(int theSize)
        {
            string[] basicShapes = { "circle", "diamond", "square", "triangle", "cross" };
            List<string> shapeList = new List<string>();
            int basicLstTotal = basicShapes.Length;
            if (theSize <= basicLstTotal)
            {
                for (int i = 0; i < theSize; i++)
                    shapeList.Add(basicShapes[i]);
            }
            else
            {
                foreach (var item in basicShapes)
                    shapeList.Add(item);
                while (basicLstTotal < theSize)
                {
                    foreach (var k in basicShapes)
                    {
                        if (basicLstTotal < theSize)
                        {
                            shapeList.Add(k);
                            basicLstTotal++;
                        }
                    }
                }
            }
            return shapeList.ToArray();
        }

        //public static string[] GetUniqueRandomShapeStrings(int count)
        //{
        //    string[] basic_shapes = { "circle", "diamond", "square", "triangle", "cross" };
        //    int basic_array_total = basic_shapes.Count();
        //    string[] shapes = new string[count];
        //    if (count <= basic_array_total)
        //    {
        //        for (int x = 0; x < shapes.Length; x++)
        //        {
        //            shapes[x] = basic_shapes[x];
        //        }
        //    }
        //    else
        //    {

        //    }

        //    string[] shapes = new string[5];
        //    for (int x = 0; x < shapes.Length; x++) 
        //    {
                
        //        shapes[x] = basic_shapes[x];
        //    }

        //    List<string> lst = new List<string>() { "circle", "diamond", "square", "triangle", "cross" };
        //    foreach (var split in lst)
        //    {
        //        features.Add(split[0]);
        //        projects.Add(split[1]);
        //    }



        //    return shapes;
        //}

        public static string[] GetUniqueRandomHexStrings(int count)
        {
            string[] colors = new string[count];
            Dictionary<Color, bool> dict = new Dictionary<Color, bool>();

            Random randomColor = new Random();

            for (int i = 0; i < count; i++)
            {
                Color color;
                while (dict.ContainsKey(color = Color.FromArgb(randomColor.Next(70, 200), randomColor.Next(100, 225), randomColor.Next(100, 230))))
                {
                    dict.Add(color, true);
                }
                //colors[i] = color;
                colors[i] = ColorTranslator.ToHtml(color);
            }
            return colors;
        }
        public static List<string> GetUniqueRandomHexList(int count)
        {
            string[] colors = new string[count];
            Dictionary<Color, bool> dict = new Dictionary<Color, bool>();

            Random randomColor = new Random();

            for (int i = 0; i < count; i++)
            {
                Color color;
                while (dict.ContainsKey(color = Color.FromArgb(randomColor.Next(70, 200), randomColor.Next(100, 225), randomColor.Next(100, 230))))
                dict.Add(color, true);
                //colors[i] = color;
                colors[i] = ColorTranslator.ToHtml(color);
            }
            return colors.ToList();
        }
        public static string ColorToHexString(Color color)
        {
            char[] hexDigits = {
         '0', '1', '2', '3', '4', '5', '6', '7',
         '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

            byte[] bytes = new byte[3];
            bytes[0] = color.R;
            bytes[1] = color.G;
            bytes[2] = color.B;
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }
            return new string(chars);
        }
        public static void CreateFileFromByteArray(string filePath, byte[] fileContent)
        {
            if (!File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                fs.Write(fileContent, 0, fileContent.Length);
                fs.Close();
            }
        }
        public static void CreateFileFromByteArray(string fileNameWithExtension, string virtualDirPath, byte[] fileContent)
        {
            if (string.IsNullOrEmpty(virtualDirPath)) return;
            string physicalDirPath = HttpContext.Current.Server.MapPath(virtualDirPath);
            if (!Directory.Exists(physicalDirPath))
                Directory.CreateDirectory(physicalDirPath);

            if (string.IsNullOrEmpty(fileNameWithExtension)) return;
            string filePath = Path.Combine(physicalDirPath, fileNameWithExtension);
            if (!File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                fs.Write(fileContent, 0, fileContent.Length);
                fs.Close();
            }
        }
        public static void ResizeStream(int maxWidth, int maxHeight, Stream filePath, string outputPath)
        {
            var image = Image.FromStream(filePath);

            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);


            var thumbnailBitmap = new Bitmap(newWidth, newHeight);

            var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbnailGraph.DrawImage(image, imageRectangle);

            thumbnailBitmap.Save(outputPath, image.RawFormat);
            thumbnailGraph.Dispose();
            thumbnailBitmap.Dispose();
            image.Dispose();
        }
        public static Stream ResizeAndPadImageToSquare(Stream src, int width, int height)
        {
            return ResizeAndPadImageToSquare(src, width, height, Color.White);
        }
        public static Stream ResizeAndPadImageToSquare(Stream src, int width, int height, Color bgColour)
        {
            MemoryStream imageStream = new MemoryStream();
            using (Image image = Image.FromStream(src))
            {
                int sourceWidth = image.Width;
                int sourceHeight = image.Height;
                int sourceX = 0;
                int sourceY = 0;
                int destX = 0;
                int destY = 0;

                float nPercent;

                var nPercentW = (width / (float)sourceWidth);
                var nPercentH = (height / (float)sourceHeight);
                if (nPercentH > nPercentW)
                {
                    nPercent = nPercentH;
                    destX = Convert.ToInt16((width -
                                    (sourceWidth * nPercent)) / 2);
                }
                else
                {
                    nPercent = nPercentW;
                    destY = Convert.ToInt16((height -
                                    (sourceHeight * nPercent)) / 2);
                }

                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                using (Bitmap bmPhoto = new Bitmap(width, height, PixelFormat.Format32bppArgb))
                {
                    bmPhoto.SetResolution(72, 72);

                    using (Graphics grPhoto = Graphics.FromImage(bmPhoto))
                    {
                        grPhoto.Clear(bgColour);
                        grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        grPhoto.DrawImage(image,
                            new Rectangle(destX, destY, destWidth, destHeight),
                            new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                            GraphicsUnit.Pixel);


                    }

                    bmPhoto.Save(imageStream, image.RawFormat);

                }
            }

            imageStream.Seek(0, SeekOrigin.Begin);
            return imageStream;
        }
        public static bool IsValidImage(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return false;
            var extension = Path.GetExtension(filePath);

            string imageFilter = "*.gif,*.png,*.gif,*.jpg,*.jpeg,*.bmp,*.tiff";
            var allowedExtensions = imageFilter.Split(',');
            return allowedExtensions.Any(e => e.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase));
        }
    }

    public class ImageSize
    {
        public int Height
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }
    }
}
