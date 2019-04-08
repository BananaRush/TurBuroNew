using Client.Interface;
using Client.Utilits;
using ModelData;
using ModelData.Model.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VSHIM.Control.WebControl;

namespace Client.ViewModel
{
    public class BoundObject
    {
        private Action<string> onClickButton;

        public BoundObject(Action<string> OnClickButton)
        {
            onClickButton = OnClickButton;
        }

        public void getNavigation(string arg = "")
        {
            onClickButton(arg);
        }
    }

    class InformationPageVM : Bandel
    {
        private string _info = string.Empty;
        private RelayCommand _loadCefSharp;
        private string _titleTop = string.Empty;
        private ChromiumBrowser _webBrowser = null;

        public InformationPageVM()
        {
            GetData();
        }

        public InformationPageVM(string title, string info)
        {
            TitleTop = title;
            Info = info;
        }

        public async void GetData()
        {
            if (Explorer.Information == null && Explorer.ButtonNav != null)
            {
                Explorer.Information = await WebApi.InfoPage.Get(Explorer.ButtonNav.Id);
            }

            SetData();
        }

        public void SetData()
        {
            try
            {
                Info = Explorer.Information.Info;
                TitleTop = Explorer.Information.Title;
            }
            catch { }
        }

        public override string TitleTop
        {
            get => _titleTop;
            set => SetProperty(ref _titleTop, value);
        }

        public string Info
        {
            get => _info;
            set => SetProperty(ref _info, value);
        }

        public override RelayCommand GoBackCommand
        {
            get
            {
                return _goBackCommand ?? (_goBackCommand = new RelayCommand(()=> 
                {
                    Explorer.Information = null;
                    App.GoBack();
                }));
            }
        }

        public RelayCommand LoadCefSharp
        {
            get
            {
                return _loadCefSharp ?? (_loadCefSharp = new RelayCommand(obj =>
                { 
                    if(obj is ChromiumBrowser elm)
                    {
                        _webBrowser = elm;
                        try
                        {
                            elm.JavascriptObjectRepository.Register("boundAsync", new BoundObject(PageInfoNav), true);
                        }
                        catch { }
                    }
                }));
            }
        }

        public async void PageInfoNav(string param)
        {
            try
            {
                int index = Convert.ToInt32(param);
                Information elm = await WebApi.InfoPage.GetId(index);
                if(elm != null)
                {
                    Explorer.Information = elm;
                    SetData();
                    _webBrowser.LoadHtml = Explorer.Information.Info;
                }
            }
            catch(FormatException e)
            {
                
            }
            catch
            {

            }
        }
    }
}
