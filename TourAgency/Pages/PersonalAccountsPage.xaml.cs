using StorageAPI.Models;
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
using TourAgency.Commands;
using TourAgency.Controls;
using TourAgency.Model.ModelWebApi;
using TourAgency.Utilities;

namespace TourAgency.Pages
{
    /// <summary>
    /// Логика взаимодействия для PersonalAccountsPage.xaml
    /// </summary>
    public partial class PersonalAccountsPage : Page
    {
        public PersonalAccountsPage()
        {
            InitializeComponent();
        }

        private ICommand _exitCommand;
        public ICommand ExitCommand => _exitCommand ?? (_exitCommand = new Command(a =>
        {
            Session.CurrentSession.User = null;
            (App.Current.MainWindow as MainWindow).NavigateCommand.Execute(new Tuple<NewsContentType, string>(NewsContentType.Page, "кабинет"));
        }));

        private void UIElement_OnTouchDown(object sender, TouchEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).Frame.Navigate(new FirstPage());
        }
    }
}
