using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TourAgency.Converters
{
    class WeatherNumbersConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value.ToString();
            int degrees = 0;
            bool IsNUmber = int.TryParse(input, out degrees);
            if (IsNUmber && degrees > 0)
            {
                return "+" + input + "°C";
            }
            else if(IsNUmber)
            {
                return input + "°C";
            }
            return input;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value.ToString();
            int degrees = 0;
            if (input.Remove(0, 1) == "+" && int.TryParse(input.Remove(0, 1).Remove(input.Length - 4, 2), out degrees))
                return degrees;
            else if (int.TryParse(input.Remove(input.Length - 3, 2), out degrees))
                return degrees;
            return input;
        }
    }
}
