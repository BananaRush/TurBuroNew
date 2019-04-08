using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TourAgency.Utilities;

namespace TourAgency.Converters
{
    class PathToImageConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if(!File.Exists($"Content/{value.ToString()}"))
                try
                {
                    using (WebClient cl = new WebClient(){
                        Credentials = new NetworkCredential(Session.FtpUser, Session.FtpPass)
                    })

                    {
                        cl.DownloadFile(new Uri($"ftp://{Session.FtpServ}/{value.ToString()}"),
                            $"Content/{value.ToString()}");
                    }
                }
                catch
                {

                }
            
            return new Uri($"file:///{Path.GetFullPath($"Content/{value.ToString()}")}");

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
