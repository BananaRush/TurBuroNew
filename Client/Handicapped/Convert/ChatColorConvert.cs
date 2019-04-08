using ModelDate.Model.SignalR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace VSHIM.Control.Handicapped.Convert
{
    class ChatColorConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MsgType elm)
            {
                if (elm == MsgType.Received)
                {
                    return new SolidColorBrush(Color.FromArgb(255, 229, 229, 229));
                }
                else
                {
                    return new SolidColorBrush(Color.FromArgb(255, 179, 219, 247));
                }
            }

            return new SolidColorBrush(Color.FromArgb(255, 179, 219, 247));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
