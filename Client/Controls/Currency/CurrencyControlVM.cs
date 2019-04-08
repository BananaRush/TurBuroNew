using Client.Controls.Currency.Model;
using Client.Utilits;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Client.Controls.Currency
{
    public class CurrencyControlVM : Bandel
    {
        private const string SOURCE = "http://www.cbr.ru/scripts/XML_daily.asp";
        private Dictionary<string, char> currencySymbolDictionary = new Dictionary<string, char>()
                    { { "USD",'$' }, { "EUR",'€' }, { "CNY",'¥' } };

        private ObservableCollection<CurrencyModel> _currencies { get; set; }

        public CurrencyControlVM()
        {
            Currencies = new ObservableCollection<CurrencyModel>();
            try
            {
                GetCurrencies();
            }
            catch (Exception ex)
            {

            }
        }

        private ValCursModel ParseCurrencies()
        {
            ValCursModel valCurs = new ValCursModel();
            XmlSerializer serializer = new XmlSerializer(typeof(ValCursModel));
            string xmlDocument = string.Empty;
            using (WebClient wc = new WebClient())
            {
                xmlDocument = wc.DownloadString(SOURCE);
            }

            using (StringReader reader = new StringReader(xmlDocument))
            {
                if (!string.IsNullOrEmpty(xmlDocument))
                {
                    valCurs = (ValCursModel)(serializer.Deserialize(reader));
                }
            }
            return valCurs;
        }

        private void GetCurrencies()
        {
            foreach (var currency in ParseCurrencies().Valute)
            {
                CurrencyModel curr = new CurrencyModel();
                if (currencySymbolDictionary.Keys.Contains(currency.CharCode))
                {
                    curr.IsDisplayed = true;
                    curr.Symbol = currencySymbolDictionary[currency.CharCode];
                    curr.Value = (int)(double.Parse(currency.Value) / (double)currency.Nominal);
                    Currencies.Add(curr);
                }
            }
        }

        public ObservableCollection<CurrencyModel> Currencies
        {
            get => _currencies;
            set
            {
                if(_currencies != value)
                {
                    _currencies = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
