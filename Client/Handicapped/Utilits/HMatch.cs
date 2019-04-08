using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VSHIM.Control.Handicapped.Utilits
{
    static class HMatch
    {
        public static int GetOffsetContent(int Procent)
        {
            double MinitorHeight = SystemParameters.PrimaryScreenHeight;

            return (int)(MinitorHeight / 100) * Procent;
        }

        public static double GetZoomMagnifier(int Procent)
        {
            double Coff1 = 0.955555;
            double Coff = 0.899999 / 200;
            return Coff1 - (Coff * Procent); 
        }

        public static int ValuePercentage(double FontSize, double Percentage)
        {
            return System.Convert.ToInt32(FontSize * (100 + Percentage) / 100);
        }

        public static double ValuePercentageWebBrowser(double FontSize, double Percentage)
        {
            Percentage = Percentage / 2;
            double OnePercent = (15 - FontSize) / 100.0;
            return FontSize + OnePercent * Percentage;
        }
    }
}
