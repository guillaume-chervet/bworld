using System.Drawing;

namespace Demo.Mvc.Core.Sites.Core
{
    public class ColorHelper
    {
        private static string HexConverter(Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        private static string RGBConverter(Color c)
        {
            return "RGB(" + c.R + "," + c.G + "," + c.B + ")";
        }

        public static string Grayer(string hexadecimal, int minus, int limitMin = 0, int limitMax = 255)
        {
            if (string.IsNullOrEmpty(hexadecimal))
            {
                return null;
            }

            var c = ColorTranslator.FromHtml(hexadecimal);

            var r = c.R - minus;
            if (r < limitMin)
            {
                r = limitMin;
            }
            else if (r > limitMax)
            {
                r = limitMax;
            }

            var g = c.G - minus;
            if (g < 0)
            {
                g = 0;
            }
            else if (g > limitMax)
            {
                g = limitMax;
            }

            var b = c.B - minus;
            if (b < limitMin)
            {
                b = limitMin;
            }
            else if (b > limitMax)
            {
                b = limitMax;
            }

            var newColor = Color.FromArgb(r, g, b);
            return RGBConverter(newColor);
        }
    }
}