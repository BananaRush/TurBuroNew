using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace TourAgency.Converters
{
    [ValueConversion(typeof(bool), typeof(PointCollection))]
    class BoolToPointsCollectionConverter : IValueConverter
    {
        private PointCollection truePoints = new PointCollection()
                    {
                        new Point(0,0),
                        new Point(33,23),
                        new Point(0,46),
                        new Point(0,0)
                    };
        private PointCollection falsePoints = new PointCollection()
                    {
                        new Point(-33,23),
                        new Point(0,0),
                        new Point(0,46),
                        new Point(-33,23)
                    };
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                bool? input = value as bool?;
                if (input == true)
                    return truePoints;
                else return falsePoints;
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
                PointCollection points = value as PointCollection;
                if (points.SequenceEqual(truePoints))
                    return true;
                else if (points.SequenceEqual(falsePoints)) return false;
                else return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
