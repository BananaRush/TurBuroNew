using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TourAgencyAdmin.Converters
{
    class PathToImageConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (value.ToString().Contains("Content/new_"))
                return Path.GetFullPath(value.ToString());
            return "ftp://fresa-bm_touragency:i8rn%2FbrK@ftp.fresa-bm.nichost.ru/" + value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
