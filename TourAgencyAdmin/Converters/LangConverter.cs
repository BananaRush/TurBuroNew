using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TourAgencyAdmin.Utilities.ModelWebApi;

namespace TourAgencyAdmin.Converters
{
    class LangConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return (Lang)value== Lang.En;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? Lang.En : Lang.Ru;
        }
    }
}
