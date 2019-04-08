using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace VSHIM.Control.Handicapped.Convert
{
    class MarginConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ScrollBarVisibility scrollBarVisibility = (ScrollBarVisibility)value;
            if(scrollBarVisibility == ScrollBarVisibility.Disabled || scrollBarVisibility == ScrollBarVisibility.Hidden)
            {
                return new Thickness(0, 0, 0, 0);
            }

            return new Thickness(0, 0, 60, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
