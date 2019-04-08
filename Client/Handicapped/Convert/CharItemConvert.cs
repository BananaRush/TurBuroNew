using ModelDate.Model.SignalR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace VSHIM.Control.Handicapped.Convert
{
    class CharItemConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MsgType elm)
            {
                if (elm == MsgType.Received)
                {
                    return HorizontalAlignment.Left;
                }
                else
                {
                    return HorizontalAlignment.Right;
                }
            }

            return HorizontalAlignment.Center;
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
