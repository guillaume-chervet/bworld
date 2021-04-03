using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Demo.Mvc.Core.Sites.Core.Command.File.Models;

namespace Demo.Mvc.Core.Sites.Core.Command.File
{
    public class ImageUtility
    {
        public static ResizeGdiResult ResizeGdi(Stream stream, ImageConfig size, ImageFormat imageFormat)
        {
            var resizeGdiResult = new ResizeGdiResult();

            var image = Image.FromStream(stream);

            var width = image.Width;
            var height = image.Height;

            const int sourceX = 0;
            const int sourceY = 0;
            const int destX = 0;
            const int destY = 0;

            var destW = 0;
            var destH = 0;

            if (size.MaxHeigth.HasValue && size.MaxWidth.HasValue)
            {
                float percent = 0, percentWidth = 0, percentHeight = 0;
                percentWidth = (size.MaxWidth.Value/(float) width);
                percentHeight = (size.MaxHeigth.Value/(float) height);
                if (percentWidth > 1)
                {
                    percentWidth = 1;
                }
                if (percentHeight > 1)
                {
                    percentHeight = 1;
                }

                if (percentHeight < percentWidth)
                {
                    percent = percentHeight;
                }
                else
                {
                    percent = percentWidth;
                }

                destW = (int) (width*percent);
                destH = (int) (height*percent);
            }
            else if (size.Width.HasValue && size.Heigth.HasValue)
            {
                destW = size.Width.Value;
                destH = size.Heigth.Value;
            }
            else
            {
                destW = width;
                destH = height;
            }

            var outputImageFormat = imageFormat;
            if (!string.IsNullOrEmpty(size.TypeMime))
            {
                outputImageFormat = GetImageFormat(size.TypeMime);
            }

            var mStream = new MemoryStream();
            resizeGdiResult.Stream = mStream;

            if (destW == 0 && destH == 0)
            {
                image.Save(mStream, outputImageFormat);
                resizeGdiResult.With = width;
                resizeGdiResult.Height = height;
                return resizeGdiResult;
            }

            using (var bitmap = new Bitmap(destW, destH, PixelFormat.Format32bppRgb))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.InterpolationMode = InterpolationMode.Default;
                    graphics.DrawImage(image,
                        new Rectangle(destX, destY, destW, destH),
                        new Rectangle(sourceX, sourceY, width, height),
                        GraphicsUnit.Pixel);
                }

                bitmap.Save(mStream, outputImageFormat);
            }

            mStream.Position = 0;

            resizeGdiResult.With = destW;
            resizeGdiResult.Height = destH;

            return resizeGdiResult;
        }

        public static ImageFormat GetImageFormat(string typeMime)
        {
            switch (typeMime)
            {
                case "image/gif":
                    return ImageFormat.Gif;
                case "image/jpeg":
                    return ImageFormat.Jpeg;
                case "image/png":
                    return ImageFormat.Png;
                default:
                    return null;
            }
        }

        public static string GetImageExtention(string typeMime)
        {
            if (!string.IsNullOrWhiteSpace(typeMime))
            {
                return typeMime.Split('/')[1];
            }

            return string.Empty;
        }
    }
}