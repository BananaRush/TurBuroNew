using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using TourAgency.Annotations;
using TourAgency.Model;

namespace TourAgency.Pages
{
    /// <summary>
    /// Логика взаимодействия для SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page, INotifyPropertyChanged
    {
        public SearchPage()
        {
            InitializeComponent();
        }


        public async void GetSearchResults(string text)
        {
            text = HttpUtility.UrlEncode(text);
            string address = $"https://yandex.ru/search/site/?html=1" +
                             $"&topdoc=http%3A%2F%2Fwww.visit-petersburg.ru%2Fsearch%2F%3Fsearchid%3D2225809%26text%3D{text}%26web%3D0" +
                             $"&encoding=" +
                             $"&tld=ru" +
                             $"&htmlcss=1.x" +
                             $"&updatehash=true" +
                             $"&searchid=2225809" +
                             $"&clid=" +
                             $"&text={text}" +
                             $"&web=0&p=&surl=&constraintid=&date=&within=" +
                             $"&from_day=&from_month=&from_year=&to_day=" +
                             $"&to_month=&to_year=&available=&priceLow=" +
                             $"&priceHigh=&categoryId=&l10n=ru";
            Random rand = new Random();
            CookieContainer cookieContainer = new CookieContainer();
            cookieContainer.Add(new Uri("https://yandex.ru"),
                new Cookie("yandexuid",
                    rand.Next(100000, 999999) + "" + rand.Next(100000, 999999) + "" + rand.Next(1000000, 9999999)));

            using (var handler = new HttpClientHandler { CookieContainer = cookieContainer })
            using (HttpClient httpClient = new HttpClient(handler))
            {
                await httpClient.GetAsync(address).ContinueWith(async r =>
                {
                    if (r.IsFaulted) return;
                    string result = await r.Result.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(result)) return;
                    //result = result.Replace(@"\/", "");
                    var items = Regex.Matches(result, @"(?<=(\<yass\\-li)).+?(?=(\<\\\/yass\\-li))");
                    ObservableCollection<SearchModel> temporaryCollection = new ObservableCollection<SearchModel>();
                    for (var item = 0; item < items.Count; item++)
                    {
                        string regexp = @"\<\/?[^\>]{0,}\/?\>";
                        var title = HttpUtility.HtmlDecode(Regex.Replace(Regex.Match(items[item].Value,
                            @"(?<=(\<yass\\-span\>)).+?(?=(\<\\\/yass\\-span\>))").Value, regexp, "")).Replace(@"\","");
                        var content = HttpUtility.HtmlDecode(Regex.Replace(Regex.Match(items[item].Value,
                            @"(?<=(class\=\""b\\-serp\\-item__text\""\>)).+?(?=(\<\\\/yass\\-div\>))").Value, regexp, "")).Replace(@"\", "");
                        
                        temporaryCollection.Add(new SearchModel()
                        {
                            Title = title,
                            Content = content
                        });
                    }
                    SearchResults = temporaryCollection;
                });
            }
            
        }

        private string _inputText;

        public string InputText
        {
            get => _inputText;
            set
            {
                if(Equals(value, _inputText)) return;
                _inputText = value;
                GetSearchResults(value);
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SearchModel> _searchResults;

        public ObservableCollection<SearchModel> SearchResults
        {
            get => _searchResults ?? (_searchResults = new ObservableCollection<SearchModel>());
            set
            {
                if (Equals(value, _searchResults)) return;
                _searchResults = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
