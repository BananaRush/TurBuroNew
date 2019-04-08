using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using SimpleKeyboard;
using StorageAPI.Models;
using StorageAPI.Models.Database;
using TourAgency.Model;
using TourAgency.Utilities;

namespace TourAgency.Controls
{
    /// <summary>
    /// Логика взаимодействия для MainNewsListControl.xaml
    /// </summary>
    public partial class MainNewsListControl : UserControl
    {
        public MainNewsListControl()
        {
            InitializeComponent();
            //Loaded+= OnLoaded;
         
        }

        private ICommand _chooseNewsCommand;
        private ICommand _allNewsCommand;

        public ICommand AllNewsCommand => _allNewsCommand ?? (_allNewsCommand = new Command(a =>
        {
            (App.Current.MainWindow as MainWindow).Frame.Navigate(new AllNewsListControl());
        }));

        public ICommand ChooseNewCommand => _chooseNewsCommand ?? (_chooseNewsCommand = new Command(a =>
        {
            if (a is int i)
            {
                (App.Current.MainWindow as MainWindow).Frame.Navigate(new SingleNewsControl() {SingleNewsId = i});
            }
        }));


        //private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        //{
        //    News = (await API.GetAllNews());
        //    if(News?.results.Count>=2)
        //    Session.CurrentSession.ActualNews = News.results[1]?.title;
        //}

        //public static readonly DependencyProperty NewsProperty = DependencyProperty.Register(
        //    "News", typeof(NewsModel), typeof(MainNewsListControl), new PropertyMetadata(default(NewsModel)));

        ////public NewsModel News
        ////{
        ////    get { return (NewsModel) GetValue(NewsProperty); }
        ////    set { SetValue(NewsProperty, value); }
        ////}

        //private void UIElement_OnTouchDown(object sender, TouchEventArgs e)
        //{
        //    AllNewsCommand.Execute(null);
        //}

        //private void OneNewsTouch(object sender, TouchEventArgs e)
        //{
        //    ChooseNewCommand.Execute(null);
        //}
    }
}
