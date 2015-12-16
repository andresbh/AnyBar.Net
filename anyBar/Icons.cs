using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace anyBar
{
    public class Icons
    {
        public static readonly IDictionary<string, Func<Icon>> Commands = new Dictionary<string, Func<Icon>>
        {
            {"white", () => Draw("#FCFCFC", "#424242")},
            {"red", () => Draw("#C60101")},
            {"orange", () => Draw("#F79E01")},
            {"yellow", () => Draw("#FCEB01")},
            {"green", () => Draw("#8DEB01")},
            {"cyan", () => Draw("#59F2CA")},
            {"blue", () => Draw("#5791E5")},
            {"purple", () => Draw("#8D19FF")},
            {"black", () => Draw("#4D4D4D")},
            {"question", () => Draw(6, 4, '?', "#4D4D4D")},
            {"exclamation", () => Draw(10, 6, '!', "#C60101")}
        };

        private static Icon Draw(float left, float top, char c, string color)
        {
            using (Bitmap bmp = new Bitmap(32, 32))
            {
                DrawCircle(color, bmp);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    Font stringFont = new Font("Arial", 16);
                    g.DrawString(c.ToString(), stringFont, new SolidBrush(ColorTranslator.FromHtml("white")), left, top);
                }
                return Icon.FromHandle(bmp.GetHicon());
            }
        }

        private static Icon Draw(string fill, string stroke = null)
        {
            using (Bitmap bmp = new Bitmap(32, 32))
            {
                DrawCircle(fill, bmp, stroke);
                return Icon.FromHandle(bmp.GetHicon());
            }
        }

        private static void DrawCircle(string color, Bitmap bmp, string stroke = null)
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Brush b = new SolidBrush(ColorTranslator.FromHtml(color)))
                    g.FillEllipse(b, 4, 4, 24, 24);
                if (!string.IsNullOrEmpty(stroke))
                {
                    using (Brush b = new SolidBrush(ColorTranslator.FromHtml(stroke)))
                        g.DrawEllipse(new Pen(b, 3), 4, 4, 24, 24 );
                }
            }
        }

        public static Icon FromImage(FileInfo file)
        {
            return Icon.FromHandle(new Bitmap(file.FullName).GetHicon());
        }
    }
}