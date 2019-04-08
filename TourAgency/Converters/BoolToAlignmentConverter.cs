using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TourAgency.Converters
{
    [ValueConversion(typeof(bool), typeof(HorizontalAlignment))]
    class BoolToAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                bool? input = value as bool?;
                if (input == true)
                    return HorizontalAlignment.Right;
                else return HorizontalAlignment.Left;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                HorizontalAlignment? input = value as HorizontalAlignment?;
                if (input == HorizontalAlignment.Left)
                    return false;
                else if (input == HorizontalAlignment.Right)
                    return true;
                else return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
