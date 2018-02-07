using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace Eagle.WebApp.Handlers
{
    /// <summary>
    /// Summary description for CaptchaHandler
    /// </summary>


    public class CaptchaHandler : IHttpHandler, IReadOnlySessionState
    {
        string _captchatext = string.Empty;
        int _height; int _width;

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //context.Response.ContentType = "image/png";
            //context.Response.BufferOutput = false;
            if (context.Request.Params["captchatext"] != null && context.Request.QueryString["captchatext"] != "")
                _captchatext = Convert.ToString(context.Request.Params["captchatext"]);
            if (context.Request.Params["height"] != null && context.Request.QueryString["height"] != "")
                _height = int.Parse(context.Request.Params["height"]);
            if (context.Request.Params["width"] != null && context.Request.QueryString["width"] != "")
                _width = int.Parse(context.Request.Params["width"]);


            DrawCaptcha(context, _captchatext, _width, _height);

        }

        public void DrawCaptcha(HttpContext context, string captchaText, int iWidth, int iHeight)
        {
            Random oRandom = new Random();

            int[] aFontEmSizes = { 15, 20, 25, 30, 35 };

            string[] aFontNames = new string[]
        {
            "Comic Sans MS",
            "Arial",
            "Times New Roman",
            "Georgia",
            "Verdana",
            "Geneva"
        };

            FontStyle[] aFontStyles = new FontStyle[]
        {  
            FontStyle.Bold,
            FontStyle.Italic,
            FontStyle.Regular,
            FontStyle.Strikeout,
            FontStyle.Underline
        };

            HatchStyle[] aHatchStyles = new HatchStyle[]
        {
            HatchStyle.BackwardDiagonal, HatchStyle.Cross, HatchStyle.DashedDownwardDiagonal, HatchStyle.DashedHorizontal,
            HatchStyle.DashedUpwardDiagonal, HatchStyle.DashedVertical, HatchStyle.DiagonalBrick, HatchStyle.DiagonalCross,
            HatchStyle.Divot, HatchStyle.DottedDiamond, HatchStyle.DottedGrid, HatchStyle.ForwardDiagonal, HatchStyle.Horizontal,
            HatchStyle.HorizontalBrick, HatchStyle.LargeCheckerBoard, HatchStyle.LargeConfetti, HatchStyle.LargeGrid,
            HatchStyle.LightDownwardDiagonal, HatchStyle.LightHorizontal, HatchStyle.LightUpwardDiagonal, HatchStyle.LightVertical,
            HatchStyle.Max, HatchStyle.Min, HatchStyle.NarrowHorizontal, HatchStyle.NarrowVertical, HatchStyle.OutlinedDiamond,
            HatchStyle.Plaid, HatchStyle.Shingle, HatchStyle.SmallCheckerBoard, HatchStyle.SmallConfetti, HatchStyle.SmallGrid,
            HatchStyle.SolidDiamond, HatchStyle.Sphere, HatchStyle.Trellis, HatchStyle.Vertical, HatchStyle.Wave, HatchStyle.Weave,
            HatchStyle.WideDownwardDiagonal, HatchStyle.WideUpwardDiagonal, HatchStyle.ZigZag
        };

            //Creates an output Bitmap
            Bitmap oOutputBitmap = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
            Graphics oGraphics = Graphics.FromImage(oOutputBitmap);
            oGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            //Create a Drawing area
            RectangleF oRectangleF = new RectangleF(0, 0, iWidth, iHeight);

            //Draw background (Lighter colors RGB 100 to 255)
            Brush oBrush = new HatchBrush(aHatchStyles[oRandom.Next(aHatchStyles.Length - 1)], Color.FromArgb((oRandom.Next(100, 255)), (oRandom.Next(100, 255)), (oRandom.Next(100, 255))), Color.White);
            oGraphics.FillRectangle(oBrush, oRectangleF);

            Matrix oMatrix = new Matrix();
            int i;
            for (i = 0; i <= captchaText.Length - 1; i++)
            {
                oMatrix.Reset();
                int iChars = captchaText.Length;
                int x = iWidth / (iChars + 1) * i;
                int y = iHeight / 2;

                //Rotate text Random
                oMatrix.RotateAt(oRandom.Next(-40, 40), new PointF(x, y));
                oGraphics.Transform = oMatrix;

                //Draw the letters with Randon Font Type, Size and Color
                oGraphics.DrawString
                (
                    //Text
                captchaText.Substring(i, 1),
                    //Random Font Name and Style
                new Font(aFontNames[oRandom.Next(aFontNames.Length - 1)], aFontEmSizes[oRandom.Next(aFontEmSizes.Length - 1)], aFontStyles[oRandom.Next(aFontStyles.Length - 1)]),
                    //Random Color (Darker colors RGB 0 to 100)
                new SolidBrush(Color.FromArgb(oRandom.Next(0, 100), oRandom.Next(0, 100), oRandom.Next(0, 100))),
                x,
                oRandom.Next(10, 40)
                );
                oGraphics.ResetTransform();
            }

            MemoryStream oMemoryStream = new MemoryStream();
            oOutputBitmap.Save(oMemoryStream, ImageFormat.Png);
            byte[] oBytes = oMemoryStream.GetBuffer();

            oOutputBitmap.Dispose();
            oMemoryStream.Close();

            context.Response.BinaryWrite(oBytes);
            context.Response.End();
        }

        //string getContentType(String path)
        //{
        //    switch (Path.GetExtension(path))
        //    {
        //        case ".bmp": return "Image/bmp";
        //        case ".gif": return "Image/gif";
        //        case ".jpg": return "Image/jpeg";
        //        case ".png": return "Image/png";
        //        default: break;
        //    }
        //    return "";
        //}

        //ImageFormat getImageFormat(String path)
        //{
        //    switch (Path.GetExtension(path))
        //    {
        //        case ".bmp": return ImageFormat.Bmp;
        //        case ".gif": return ImageFormat.Gif;
        //        case ".jpg": return ImageFormat.Jpeg;
        //        case ".png": return ImageFormat.Png;
        //        default: break;
        //    }
        //    return ImageFormat.Jpeg;
        //}

        //byte[] getResizedImage(String path, int width, int height)
        //{
        //    Bitmap imgIn = new Bitmap(path);
        //    double y = imgIn.Height;
        //    double x = imgIn.Width;

        //    double factor = 1;
        //    if (width > 0)
        //    {
        //        factor = width / x;
        //    }
        //    else if (height > 0)
        //    {
        //        factor = height / y;
        //    }
        //    System.IO.MemoryStream outStream =
        //    new System.IO.MemoryStream();
        //    Bitmap imgOut =
        //    new Bitmap((int)(x * factor), (int)(y * factor));
        //    Graphics g = Graphics.FromImage(imgOut);
        //    g.Clear(Color.White);
        //    g.DrawImage(imgIn, new Rectangle(0, 0, (int)(factor * x),
        //    (int)(factor * y)),
        //    new Rectangle(0, 0, (int)x, (int)y), GraphicsUnit.Pixel);

        //    imgOut.Save(outStream, getImageFormat(path));
        //    return outStream.ToArray();
        //}  


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

}