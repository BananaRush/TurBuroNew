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
    class ContentTypeConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return null;
            switch (parameter.ToString())
            {
                case "video":
                    if ((SliderContentType)value == SliderContentType.Video)
                        return true;
                    return false;
                case "photo":
                    if ((SliderContentType) value == SliderContentType.Image)
                        return true;
                    return false;
                    default:
                        return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return null;
            switch (parameter.ToString())
            {
                case "video":
                    if ((bool) value)
                        return SliderContentType.Video;
                    return SliderContentType.Image;
                case "photo":
                    if ((bool) value)
                        return SliderContentType.Image;
                    return SliderContentType.Video;
                    default:
                        return null;
            }
        }
    }
}
