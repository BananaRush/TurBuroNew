using CefSharp;
using CefSharp.Wpf;
using ModelData.Model.Database;
using StorageAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TourAgency.Commands;
using TourAgency.Utilities;

namespace TourAgency.Pages
{
    /// <summary>
    /// Логика взаимодействия для InfoPage.xaml
    /// </summary>
    public partial class InfoPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ICommand _GoBackCommand;
        private NewsModel _model = null;
        private string _info = string.Empty;

        public InfoPage(NewsModel model)
        {
            InitializeComponent();;
            _model = model;
        }

        public ICommand GoBackCommand => _GoBackCommand ?? (_GoBackCommand = new Command(a =>
        {
            if (ChromiumWebBrowser.CanGoBack)
                ChromiumWebBrowser.Back();
            else
                (App.Current.MainWindow as MainWindow).Frame.Navigate(new FirstPage());
        }));

        private async void browser_IsBrowserInitialized(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_model != null)
            {
                Information info = await WebApi.InfoPage.Get(_model.Id);
                if (info != null)
                {
                    Info = info.Info;
                }
            }

            ((ChromiumWebBrowser)sender).MenuHandler = new VSHIM.Control.WebControl.CustomMenuHandler();
        }

        public string Info
        {
            get => _info;
            set
            {
                if(_info != value)
                {
                    _info = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
