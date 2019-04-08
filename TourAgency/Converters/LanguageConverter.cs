using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;
using TourAgency.Properties;
using TourAgency.Utilities;

namespace TourAgency.Converters
{
    class LanguageConverter: IMultiValueConverter
    {
        public LanguageConverter()
        {
            dt.Tick+= DtOnTick;
        }

        private void DtOnTick(object sender, EventArgs eventArgs)
        {
            Session.CurrentSession.IsLoading = false;
            dt.Stop();
        }

        private DispatcherTimer dt = new DispatcherTimer(){Interval = TimeSpan.FromSeconds(1)};
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 0) return null;
             dt?.Start();
            if (values.Length>1 && values[1] == null) return null;
            if (Session.CurrentSession.DictionaryLanguage.ContainsKey(values?.Length == 1 ? parameter.ToString() : values[1]?.ToString()))
            switch (Session.CurrentSession.ChoosedLanguage.Name)
            {
                case "en":
                    return Session.CurrentSession
                        .DictionaryLanguage[values?.Length == 1 ? parameter.ToString() : values[1].ToString()].Item1;
                case "ch":
                    return Session.CurrentSession
                        .DictionaryLanguage[values?.Length == 1 ? parameter.ToString() : values[1].ToString()].Item2;
            }
            dt?.Start();
            return values.Length==1?parameter.ToString()
                :values[1].ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[0];
        }
    }
}
