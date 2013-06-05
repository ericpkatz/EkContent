using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace EKContent.web.Utilities
{
    public class ImageHelper
    {
        public static ImageHelper Instance()
        {
            return new ImageHelper();
        }

        public Bitmap ResizeImage(Stream stream, int lnWidth = 150, int lnHeight = 150)
        {
                using(var inputBitmap = new Bitmap(stream))
                {
                    decimal lnRatio;
                    int lnNewWidth = 0;
                    int lnNewHeight = 0;

                    if (inputBitmap .Width < lnWidth && inputBitmap .Height < lnHeight)
                        return null;

                        lnRatio = inputBitmap.Width > inputBitmap.Height?(decimal) lnWidth/inputBitmap.Width : (decimal) lnHeight/inputBitmap.Height;
                        lnNewWidth = inputBitmap.Width > inputBitmap.Height ? lnWidth : (int)(inputBitmap.Width*lnRatio);
                        lnNewHeight = inputBitmap.Width > inputBitmap.Height?(int) (inputBitmap.Height*lnRatio) : lnHeight;


                    var bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
                    Graphics g = Graphics.FromImage(bmpOut);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
                    g.DrawImage(inputBitmap, 0, 0, lnNewWidth, lnNewHeight);
                    return bmpOut;
                }
        }
    }
}