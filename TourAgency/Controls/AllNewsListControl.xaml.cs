using System;
using System.Collections.Generic;
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
using TourAgency.Model;

namespace TourAgency.Controls
{
    /// <summary>
    /// Логика взаимодействия для AllNewsListControl.xaml
    /// </summary>
    public partial class AllNewsListControl : UserControl
    {
        public AllNewsListControl()
        {
            InitializeComponent();
            Loaded+= OnLoaded;
        }

        private ICommand _chooseCommand;

        public ICommand ChooseCommand => _chooseCommand ?? (_chooseCommand = new Command(a =>
        {
            if (a is int i)
            {
                (App.Current.MainWindow as MainWindow).Frame.Navigate(new SingleNewsControl() {SingleNewsId = i});
            }
        }));


        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            //News = await API.GetAllNews();
        }

        //public static readonly DependencyProperty NewsProperty = DependencyProperty.Register(
        //    "News", typeof(NewsModel), typeof(AllNewsListControl), new PropertyMetadata(default(NewsModel)));

        //public NewsModel News
        //{
        //    get => (NewsModel) GetValue(NewsProperty);
        //    set => SetValue(NewsProperty, value);
        //}

        private void BackTouch(object sender, TouchEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).Frame.GoBack();
        }
    }
}
