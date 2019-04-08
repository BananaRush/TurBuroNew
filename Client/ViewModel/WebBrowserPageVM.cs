using CefSharp;
using CefSharp.Wpf;
using Client.Interface;
using Client.Utilits;
using ModelData;
using ModelData.Model.Database;
using System.Collections.Generic;
using System.Linq;

namespace Client.ViewModel
{
    class WebBrowserPageVM : Bandel
    {
        private RelayCommand _loadCefSharp;
        private string _adrres = string.Empty;
        private ChromiumWebBrowser _chromiumWeb = null;
        private List<UrlListInfo> listUrl = null;

        public WebBrowserPageVM()
        {
            GetData();
        }

        public WebBrowserPageVM(string title, string url)
        {
            TitleTop = title;
            Adrres = url;
        }

        public async void GetData()
        {
            int index = 0;
            if(Explorer.ButtonNav != null)
            {
                index = Explorer.ButtonNav.Id;
            }

            UrlInfo elm = await WebApi.WebBrowser.Get(index);

            if(elm != null)
            {
                listUrl = elm.UrlListInfos;
                TitleTop = elm.Name;
                Adrres = elm.Url;
                listUrl.Add(new UrlListInfo() { Url = elm.Url });
                if (_chromiumWeb != null)
                {
                    _chromiumWeb.RequestHandler = new VSHIM.Control.WebControl.BrowserRequestHandler(listUrl?.Select(r=> r.Url).ToList());
                }
            }


        }
        public string Adrres
        {
            get
            {
                return _adrres;
            }
            set
            {
                SetProperty(ref _adrres, value);
            }
        }

        public override RelayCommand GoBackCommand
        {
            get
            {
                return _goBackCommand ?? (_goBackCommand = new RelayCommand(() => 
                {
                    if (_chromiumWeb.CanGoBack)
                    {
                        _chromiumWeb.Back();
                    }
                    else
                    {
                        App.GoBack();
                    }
                }));
            }
        }

        public RelayCommand LoadCefSharp
        {
            get
            {
                return _loadCefSharp ?? (_loadCefSharp = new RelayCommand(obj => 
                {
                    if(obj is ChromiumWebBrowser elm)
                    {
                        _chromiumWeb = elm;
                        // Обрабатываем запросы
                        _chromiumWeb.RequestHandler = new VSHIM.Control.WebControl.BrowserRequestHandler(listUrl?.Select(r => r.Url).ToList());
                        // Запрет создание окнон
                        _chromiumWeb.LifeSpanHandler = new VSHIM.Control.WebControl.SampleLifeSpanHandler();
                        // Лтключаем контекстное меню
                        _chromiumWeb.MenuHandler = new VSHIM.Control.WebControl.CustomMenuHandler();
                    }
                }));
            }
        }
    }
}
