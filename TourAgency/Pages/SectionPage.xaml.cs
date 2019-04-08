using StorageAPI.Models.Database;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TourAgency.Commands;
using TourAgency.Utilities;
using ModelData.Models.Database;

namespace TourAgency.Pages
{
    /// <summary>
    /// Логика взаимодействия для SectionPage.xaml
    /// </summary>
    public partial class SectionPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ICommand _GoBackCommand;
        private ICommand _GetNextItem;
        private ObservableCollection<Section> PSections = null;
        private List<Section> datelist = null;
        private NewsModel _model = null;

        public SectionPage(NewsModel elm)
        {
            InitializeComponent();
            _model = elm;
            this.Loaded += SectionPage_Loaded;
        }

        public ObservableCollection<Section> Sections
        {
            get => PSections;
            set
            {
                if(PSections != value)
                {
                    PSections = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand GoBackCommand => _GoBackCommand ?? (_GoBackCommand = new Command(obj =>
        {
            (App.Current.MainWindow as MainWindow).Frame.GoBack();
        }));

        public ICommand GetNextItem => _GetNextItem ?? (_GetNextItem = new Command(obj =>
        {
            
        }));

        private async void SectionPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= SectionPage_Loaded;
            if(_model != null)
            {
                datelist = await WebApi.SectionPage.Get(_model.Id);
                if(datelist != null)
                {
                    Sections = new ObservableCollection<Section>(datelist);
                }
                else
                {
                    Sections = new ObservableCollection<Section>();
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
