using StorageAPI.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TourAgency.Commands;
using TourAgency.Model.ModelWebApi;
using TourAgency.Pages;
using TourAgency.Utilities;

namespace TourAgency.Controls
{
    /// <summary>
    /// Логика взаимодействия для AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow : UserControl
    {
        public AccountWindow()
        {
            InitializeComponent();
            Session.CurrentSession.KeyBoardVisibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            Session.CurrentSession.KeyBoardVisibility = Visibility.Collapsed;
            if ((App.Current.MainWindow as MainWindow).Frame.Content is PersonalAccountsPage)
                (App.Current.MainWindow as MainWindow).Frame.Navigate(new FirstPage());

        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            Authorization.Visibility = Visibility.Visible;
            Registration.Visibility = Visibility.Collapsed;
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            Registration.Visibility = Visibility.Visible;
            Authorization.Visibility = Visibility.Collapsed;
        }

        private ICommand _registrationCommand;
        private ICommand _authCommand;
        public ICommand RegistrationCommand => _registrationCommand ?? (_registrationCommand = new Command(async a =>
        {
            if (!Checked)
            {
                new MessageBox("Прочтите и примите условия обработки личных данных!").ShowDialog();
                return;
            }
            Session.CurrentSession.IsLoading = true;

            if (!string.IsNullOrEmpty(TextBoxLogin.Text) && !string.IsNullOrEmpty(TextBoxName.Text) &&
                !string.IsNullOrEmpty(PasswordBoxOne.Password) && !string.IsNullOrEmpty(PasswordBoxTwo.Password))
            {
                if (PasswordBoxOne.Password == PasswordBoxTwo.Password)
                {
                    var user = new ExtendedUser()
                    {
                        Login = TextBoxLogin.Text,
                        Name = TextBoxName.Text,
                        Password = PasswordBoxTwo.Password
                    };
                    if (await WebApi.Users.Post(user) >= 0)
                    {
                        Session.CurrentSession.User = user;
                        (App.Current.MainWindow as MainWindow).Frame.Navigate(new PersonalAccountsPage());
                        this.Visibility = Visibility.Collapsed;
                        Session.CurrentSession.KeyBoardVisibility = Visibility.Collapsed;

                    }
                    else
                    {
                        MessageBox msb = new MessageBox("Ошибка регистрации, попробуйте позже!");
                        msb.ShowDialog();
                    }
                }
                else
                {
                    MessageBox msb = new MessageBox("Введенные пароли не совпадают!");
                    msb.ShowDialog();

                }

            }
            else
            {
                MessageBox msb = new MessageBox("Не все поля заполнены!");
                msb.ShowDialog();
            }
            TextBoxLogin.Text = String.Empty;
            TextBoxName.Text = string.Empty;
            PasswordBoxOne.Password = String.Empty;
            PasswordBoxTwo.Password = string.Empty;
            Session.CurrentSession.IsLoading = false;

        }));

        public ICommand AuthCommand => _authCommand ?? (_authCommand = new Command(async a =>
        {
            Session.CurrentSession.IsLoading = true;

            if (!string.IsNullOrEmpty(TextBoxLoginAuth.Text) && !string.IsNullOrEmpty(PasswordBoxPassAuth.Password))
            {
                User user = null;//await WebApi.Users.Auth(TextBoxLoginAuth.Text, PasswordBoxPassAuth.Password);
                if (user!= null)
                {
                    Session.CurrentSession.User = user;
                    this.Visibility = Visibility.Collapsed;
                    Session.CurrentSession.KeyBoardVisibility = Visibility.Collapsed;
                    (App.Current.MainWindow as MainWindow).Frame.Navigate(new PersonalAccountsPage());

                }
                else
                {
                    MessageBox msb = new MessageBox("Неверный логин/пароль");
                    msb.ShowDialog();
                }
            }
            else
            {
                MessageBox msb = new MessageBox("Не все поля заполнены!");
                msb.ShowDialog();
            }
            TextBoxLoginAuth.Text= String.Empty;
            PasswordBoxPassAuth.Password = String.Empty;
            Session.CurrentSession.IsLoading = false;

        }));

        private void TextBoxLoginAuth_OnTouchDown(object sender, TouchEventArgs e)
        {
            if(sender is TextBox)
            ((TextBox) sender).Focus();
            if (sender is PasswordBox)
                ((PasswordBox) sender).Focus();
        }

        public static readonly DependencyProperty CheckedProperty = DependencyProperty.Register(
            "Checked", typeof(bool), typeof(AccountWindow), new PropertyMetadata(default(bool)));

        public bool Checked
        {
            get => (bool) GetValue(CheckedProperty);
            set => SetValue(CheckedProperty, value);
        }

        private void ContentElement_OnTouchDown(object sender, TouchEventArgs e)
        {
            LicenseConfim lc = new LicenseConfim();
            bool? res = lc.ShowDialog();
            switch (res)
            {
                case true:
                    Checked = true;
                    Session.CurrentSession.KeyBoardVisibility = Visibility.Visible;
                    break;
                case false:
                    Checked = false;
                    break;
                case null:
                    Session.CurrentSession.KeyBoardVisibility = Visibility.Visible;
                    break;
            }
        }
    }
}
