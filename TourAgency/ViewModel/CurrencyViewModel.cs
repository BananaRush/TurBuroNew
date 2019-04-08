using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using TourAgency.Model;

namespace TourAgency.ViewModel
{
    class CurrencyViewModel : BaseViewModel
    {
        private const string SOURCE = "http://www.cbr.ru/scripts/XML_daily.asp";
        private Dictionary<string, char> currencySymbolDictionary = new Dictionary<string, char>()
                    { { "USD",'$' }, { "EUR",'€' }, { "CNY",'¥' } };

        public ObservableCollection<Currency> _currencies { get; set; }

        public CurrencyViewModel()
        {
            _currencies = new ObservableCollection<Currency>();
            try
            {
                GetCurrencies();
            }
            catch(Exception ex)
            {
               
            }

        }

        private ValCurs ParseCurrencies()
        {
            ValCurs valCurs = new ValCurs();
            XmlSerializer serializer = new XmlSerializer(typeof(ValCurs));
            string xmlDocument = string.Empty;
            using (WebClient wc = new WebClient())
            {
                xmlDocument = wc.DownloadString(SOURCE);
            }
            using (StringReader reader = new StringReader(xmlDocument))
            {
                if (!string.IsNullOrEmpty(xmlDocument))
                {
                    valCurs = (ValCurs)(serializer.Deserialize(reader));
                }
            }
            return valCurs;
        }

        private void GetCurrencies()
        {
            foreach (var currency in ParseCurrencies().Valute)
            {
                Currency curr = new Currency();
                if (currencySymbolDictionary.Keys.Contains(currency.CharCode))
                {
                    curr.IsDisplayed = true;
                    curr.Symbol = currencySymbolDictionary[currency.CharCode];
                    curr.Value = (int)(double.Parse(currency.Value) / (double)currency.Nominal);
                    _currencies.Add(curr);
                }
            }
        }
    }
}
