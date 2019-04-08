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
using TourAgency.Annotations;
using TourAgency.Model;

namespace TourAgency.Controls
{
    /// <summary>
    /// Логика взаимодействия для SingleNewsControl.xaml
    /// </summary>
    public partial class SingleNewsControl : UserControl, INotifyPropertyChanged
    {
        public SingleNewsControl()
        {
            InitializeComponent();
        }

        public SingleNewsControl(SingleNewsModel model)
        {
            InitializeComponent();
            SingleNews = model;
        }
        private async void GetNews(int id)
        {
            await API.GetSingleNews(id).ContinueWith(d =>
            {
                if (d.IsFaulted)
                    SingleNews = null;
                SingleNews = d.Result;
            });

        }

        public static readonly DependencyProperty SingleNewsIdProperty = DependencyProperty.Register(
            "SingleNewsId", typeof(int), typeof(SingleNewsControl), new PropertyMetadata(default(int), GetSingleNewsById));

        private static void GetSingleNewsById(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            SingleNewsControl singleNewsObject = (SingleNewsControl)dependencyObject;
            singleNewsObject.GetNews((int) dependencyPropertyChangedEventArgs.NewValue);
        }

        public int SingleNewsId
        {
            get => (int) GetValue(SingleNewsIdProperty);
            set => SetValue(SingleNewsIdProperty, value);
        }

        private SingleNewsModel _singleNews;

        public SingleNewsModel SingleNews
        {
            get => _singleNews;
            set
            {
                if (Equals(value, _singleNews)) return;
                _singleNews = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BackTouch(object sender, TouchEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).Frame.GoBack();
        }
    }
}
