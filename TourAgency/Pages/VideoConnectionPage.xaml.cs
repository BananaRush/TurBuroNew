using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using CefSharp;
using CefSharp.Wpf;
using Microsoft.AspNet.SignalR.Client;
using TourAgency.Annotations;
using TourAgency.Commands;
using TourAgency.Controls;
using TourAgency.Utilities;
using MessageBox = System.Windows.Forms.MessageBox;

namespace TourAgency.Pages
{
    /// <summary>
    /// Логика взаимодействия для VideoConnectionPage.xaml
    /// </summary>
    public partial class VideoConnectionPage : Page,INotifyPropertyChanged
    {
        public VideoConnectionPage(bool isCamers)
        {
            InitializeComponent();
            GridForBrowser.Children.Remove(VideoConnectionBlockControl);
            Session.CurrentSession.IsCameraUsed = true;
            IsCamers = isCamers;

            CefSettings settings = new CefSettings();

            settings.CefCommandLineArgs.Add("enable-media-stream", "1");
            settings.CefCommandLineArgs.Add("use-fake-ui-for-media-stream", "1");
            settings.CefCommandLineArgs.Add("enable-speech-input", "1");
            settings.CefCommandLineArgs.Add("enable-usermedia-screen-capture", "1");
            if (!Cef.IsInitialized)
                Cef.Initialize(settings);
            _chromiumWeb = new ChromiumWebBrowser(){Margin = new Thickness(0, 0, 0, 100) };
            Grid.SetColumn(_chromiumWeb, 1);
            _chromiumWeb.FrameLoadEnd += async (sender, args) =>
            {
                if (!args.Frame.IsMain) return;
                await _chromiumWeb.GetMainFrame().EvaluateScriptAsync("function FindElementsForHide(){" +
                                                                      "    var a = document.getElementById('sharing-div');" +
                                                                      "    if(a){" +
                                                                      "        a.style.display = 'none';" +
                                                                      "        document.getElementById('icons').style.display = 'none';" +
                                                                      "    }else{setTimeout(FindElementsForHide, 100);}" +
                                                                      "}" +
                                                                      "document.getElementById('confirm-join-button').click();" +
                                                                      "FindElementsForHide();");
                await Task.Delay(1000);
               Dispatcher.Invoke(()=>_chromiumWeb.Visibility = Visibility.Visible);
            };
            GridForBrowser.Children.Add(_chromiumWeb);

            Unloaded += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(_selectedKiosk)) _hubProxyCameraOnline.Invoke("Disconnect", _selectedKiosk);
            };

            _hubConnectionCameraOnline = new HubConnection("http://195.133.1.197:228");
            _hubProxyCameraOnline = _hubConnectionCameraOnline.CreateHubProxy("ClientCamHub");
            _hubConnectionCameraOnline.Start();
            AllKiosks.Remove(Session.CurrentSession.NameTerminal);
        }

        private ObservableCollection<string> _allKiosks;
        public ObservableCollection<string> AllKiosks
        {
            get => _allKiosks ?? (_allKiosks = new ObservableCollection<string>()
            {
                "Порт 1",
                "Порт 2",
                "Порт 3",
                "Порт 4",
                "Пулково 1",
                "Пулково 2",
                "Музей"
            });
            set
            {
                _allKiosks = value;
                OnPropertyChanged();
            }
        }

        private string _roomId;
        private HubConnection _hubConnectionCameraOnline;
        private IHubProxy _hubProxyCameraOnline;
        private ChromiumWebBrowser _chromiumWeb;
        private string _selectedKiosk;
        private HubConnection _hubConnection;


        public VideoConnectionPage()
        {
            InitializeComponent();
            _videoConnectionControl = VideoConnectionBlockControl;
            this.Unloaded += VideoConnectionPage_Unloaded;
        }

        private void VideoConnectionPage_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= VideoConnectionPage_Unloaded;
            this.VideoConnectionBlockControl.Dispose();
        }

        private readonly VideoConnectionControl _videoConnectionControl;

        private void VideoConnectionControl_OnVideoConnectionOnCallEnded(object sender)
        {
            Dispatcher.Invoke(() =>
            {
                IsCalling = false;
            });
        }

        private void VideoConnectionControl_OnVideoConnectionOnCallStarted(object sender)
        {
            Dispatcher.Invoke(() =>
            {
                IsCalling = true;
            });
        }


        public static readonly DependencyProperty IsCamersProperty = DependencyProperty.Register(
            "IsCamers", typeof(bool), typeof(VideoConnectionPage), new PropertyMetadata(default(bool)));

        public bool IsCamers
        {
            get => (bool) GetValue(IsCamersProperty);
            set => SetValue(IsCamersProperty, value);
        }

        private void VideoConnectionControl_OnVideoConnectionOnError(object message)
        {
            Dispatcher.Invoke(() =>
            {
                if ((string) message == "No operators online")
                {
                    Task.Run(async() =>
                    {
                        if (IsErrorVisible) return;
                        IsCalling = false;
                        IsErrorVisible = true;
                        await Task.Delay(4000);
                        IsErrorVisible = false;
                    });
                }
            });
        }

        private void VideoConnectionControl_OnVideoConnectionLoaded(object sender)
        {
            Dispatcher.Invoke(() =>
            {
                
            });
        }

        private ICommand _hangUpCommand;

        private ICommand HangUpCommand => _hangUpCommand ?? (_hangUpCommand = new Command(d =>
        {
            //TODO при нажатии на кнопку НАЗАД вызвать этот метод, если IsCalling = true. Не забыть просто VideoConnectionControl.DISPOSE
            IsCalling = false;
            _videoConnectionControl?.HangUp();
        }));

        private ICommand _makeCallCommand;

        private ICommand MakeCallCommand => _makeCallCommand ?? (_makeCallCommand = new Command(d =>
        {
            IsCalling = true;
            _videoConnectionControl?.MakeCall();
        }));

        private ICommand _startStopCallCommand;

        public ICommand StartStopCallCommand => _startStopCallCommand ?? (_startStopCallCommand = new Command(d =>
        {
            if (IsErrorVisible) return;
            if (!IsCalling) MakeCallCommand.Execute(null);
            else HangUpCommand.Execute(null);
        }));

        private bool _isCalling;

        public bool IsCalling
        {
            get => _isCalling;
            set
            {
                if (Equals(value, _isCalling)) return;
                _isCalling = value;
                OnPropertyChanged();
            }
        }

        private bool _isErrorVisible;

        public bool IsErrorVisible
        {
            get => _isErrorVisible;
            set
            {
                if (Equals(value, _isErrorVisible)) return;
                _isErrorVisible = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UIElement_OnTouchDown(object sender, TouchEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).Frame.GoBack();
        }

        private void TouchCommand(object sender, RoutedEventArgs e)
        {
            StartStopCallCommand.Execute(null);
        }

        private async void RadioButtonChecked(object sender, RoutedEventArgs e)
        {
            try
            {
             
                if (!string.IsNullOrEmpty(_selectedKiosk))
                    await _hubProxyCameraOnline.Invoke("Disconnect", _selectedKiosk);

                _roomId = CreateMD5(DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"));
                _chromiumWeb.Visibility = Visibility.Hidden;
                _chromiumWeb.Load($"https://appr.tc/r/{_roomId}?audio=false&video=false");

                _selectedKiosk = ((RadioButton)sender).Content.ToString();
                await _hubProxyCameraOnline.Invoke("Connect", _selectedKiosk, _roomId);

            }
            catch
            {
                // ignored
            }

        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash 
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string 
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

    }
}
