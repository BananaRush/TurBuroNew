using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SimpleKeyboard;
using TourAgency.Utilities;

namespace TourAgency.Pages
{
    /// <summary>
    /// Логика взаимодействия для LicenseConfim.xaml
    /// </summary>
    public partial class LicenseConfim : Window
    {
        public LicenseConfim(bool isPhotoPopup, string photoName)
        {
            InitializeComponent();
            IsPhotoPopup = isPhotoPopup;
            PhotoName = photoName;
            DownloadPhoto();
        }
        public LicenseConfim()
        {
            InitializeComponent();
            Session.CurrentSession.KeyBoardVisibility = Visibility.Collapsed;
        }

        private string PhotoName;

        private void DownloadPhoto()
        {
            Session.CurrentSession.IsLoading = true;
            WebClient wc = new WebClient(){ Credentials = new NetworkCredential(Session.FtpUser, Session.FtpPass) };
            if (!Directory.Exists("Content/EmotionShare"))
                Directory.CreateDirectory("Content/EmotionShare");
            wc.DownloadFileCompleted += (sender, args) =>
            {
                Session.CurrentSession.IsLoading = false;
                PathImage = Path.GetFullPath($"Content/EmotionShare/{PhotoName}");
            };
            wc.DownloadFileAsync(new Uri( $"ftp://{Session.FtpServ}/EmotionShare/{PhotoName}"), $"Content/EmotionShare/{PhotoName}");
        }

        public static readonly DependencyProperty IsPhotoPopupProperty = DependencyProperty.Register(
            "IsPhotoPopup", typeof(bool), typeof(LicenseConfim), new PropertyMetadata(default(bool)));

        public bool IsPhotoPopup
        {
            get => (bool) GetValue(IsPhotoPopupProperty);
            set => SetValue(IsPhotoPopupProperty, value);
        }

        public static readonly DependencyProperty PathImageProperty = DependencyProperty.Register(
            "PathImage", typeof(string), typeof(LicenseConfim), new PropertyMetadata(default(string)));

        public string PathImage
        {
            get => (string) GetValue(PathImageProperty);
            set => SetValue(PathImageProperty, value);
        }

        private ICommand _confimCommand;
        private ICommand _calcelCommand;

        public ICommand ConfimCommand => _confimCommand ?? (_confimCommand = new Command(a =>
        {
            DialogResult = true;
        }));

        public ICommand CancelCommand => _calcelCommand ?? (_calcelCommand = new Command(a =>
        {
            (App.Current.MainWindow as MainWindow).Frame.Navigate(new FirstPage());
            Session.CurrentSession.KeyBoardVisibility = Visibility.Collapsed;
            Session.CurrentSession.VisibilityAccount = Visibility.Collapsed;
            DialogResult = false;
        }));

        private void UIElement_OnTouchDown(object sender, TouchEventArgs e)
        {
            DialogResult = null;
        }

        private void cancelTouch(object sender, TouchEventArgs e)
        {
            if (!IsPhotoPopup)
                CancelCommand.Execute(null);
            else
                DialogResult = false;
        }

        private void ConfimTouch(object sender, TouchEventArgs e)
        {
            if (!IsPhotoPopup)
                ConfimCommand.Execute(null);
            else
                DialogResult = true;
        }

    }
}
